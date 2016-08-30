using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ###############################################################
// MazeGenerator is responsible for creating the layout of the maze 
// in the MapThatIce scenes. It generates a wall layout then 
// determines which terrain block goes in each position.
// This class is based off of a solution by 
// Michio Magic: http://forum.unity3d.com/threads/quick-maze-generator.173370/
// ###############################################################
public class MazeGenerator : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	public Material brick;
	public Vector2 currentTile {
		get { return _currentTile; }
		private set {
			if (value.x < 1 || value.x >= this.width - 1 || value.y < 1 || value.y >= this.height - 1) {
				throw new ArgumentException ("Width and Height must be greater than 2 to make a maze");
			}
			_currentTile = value;
		}
	}
	public int height;
	public static MazeGenerator Instance { get { return instance; } }
	public int scale = 6;
	public int width;
	private int blocksToRemove;
	public bool mazeGenerationComplete = false;
	private static MazeGenerator instance;
	private int[,] maze;
	private List<Vector2> offsets = new List<Vector2> { 
		new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) 
	};
	private System.Random random = new System.Random();
	private Stack<Vector2> tiletoTry = new Stack<Vector2>();
	private Vector2 _currentTile;

// ###############################################################
// Initialization Functions 
// ###############################################################
	void Start()  { 
		instance = this; 
		if (GameController.missionDiff != "Tutorial") {
			setupMaze();
			assignTerrainBlocks();
			createBorders();
		}
		mazeGenerationComplete = true;
	}

	public void setMaze(int[,] _maze_) {
		maze = _maze_;
	}

	public void assignTerrainBlocks () {
		for (int column = 0; column <= maze.GetUpperBound(0); column++)  {
			for (int row = 1; row <= maze.GetUpperBound(1) - 1; row++) {
				if (maze[column, row] == 1)  {
					string terrainForSpot = determineTerrainForSpot (
						blockToNorth (column, row), blockToNorthEast (column, row), 
						blockToEast (column, row), blockToSouthEast (column, row), 
						blockToSouth (column, row), blockToSouthWest (column, row), 
						blockToWest (column, row), blockToNorthWest (column, row)
					);
					GameObject block = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+terrainForSpot));
					block.transform.position = new Vector3 ((column * scale) - (scale / 2), 0, (row * scale) - (scale / 2));
					block.transform.parent = transform;
				}
			}
		}
	}
	
// ###############################################################
// MAGIC Functions 
// ###############################################################
	private void setupMaze () {
		initializeMazeArray ();
		populateMazeArray ();
		if (GameController.missionDiff != "Tutorial") removeBlocks (50);
	}

	private void initializeMazeArray () {
		maze = new int[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				maze [x, y] = 1;
			}
		}
	}

	private void populateMazeArray () {
		currentTile = Vector2.one;
		tiletoTry.Push(currentTile);
		maze = createMaze();
	}

	private void removeBlocks (int blocksToRemove) {
		int count = 0;
		int columns = maze.GetUpperBound (0);
		int rows = maze.GetUpperBound (1);
		while (count <= blocksToRemove) {
			int column = UnityEngine.Random.Range (0, columns + 1);
			int row = UnityEngine.Random.Range (0, rows + 1);
			if (maze [column, row] == 1) {
				maze [column, row] = 0;
				count++;
			}
		}
	}

	public int[,] createMaze() {
		List<Vector2> neighbors;
		while (tiletoTry.Count > 0) {
			maze[(int)currentTile.x, (int)currentTile.y] = 0;
			neighbors = getValidNeighbors(currentTile);
			if (neighbors.Count > 0) {
				tiletoTry.Push (currentTile);
				currentTile = neighbors [random.Next (neighbors.Count)];
			} else {
				currentTile = tiletoTry.Pop ();
			}
		}
		return maze;
	}

	private List<Vector2> getValidNeighbors(Vector2 centerTile) {
		List<Vector2> validNeighbors = new List<Vector2>();
		foreach (var offset in offsets) {
			Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);
			if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1) {
				if (maze [(int)toCheck.x, (int)toCheck.y] == 1 && hasThreeWallsIntact (toCheck)) {
					validNeighbors.Add (toCheck);
				}
			}
		}
		return validNeighbors;
	}

	private bool hasThreeWallsIntact(Vector2 Vector2ToCheck) {
		int intactWallCounter = 0;
		foreach (var offset in offsets) {
			Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);
			if (isInside (neighborToCheck) && maze [(int)neighborToCheck.x, (int)neighborToCheck.y] == 1) {
				intactWallCounter++;
			}
		}
		return intactWallCounter == 3;
	}

// ###############################################################
// Maze Block Functions 
// ###############################################################
	private bool isInside(Vector2 p) {
		return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
	}

	private bool blockToEast (int column, int row) {
		if (column == maze.GetUpperBound (0)) {
			return true;
		}
		return maze [column + 1, row] == 1;
	}

	private bool blockToNorth (int column, int row) {
		if (row == maze.GetUpperBound (1) - 1) {
			return false;
		}
		return maze [column, row + 1] == 1;
	}

	private bool blockToNorthEast (int column, int row) {
		if (row == maze.GetUpperBound (1) - 1) {
			return false;
		}
		if (column == maze.GetUpperBound (0)) {
			return true;
		}
		return maze [column + 1, row + 1] == 1;
	}

	private bool blockToNorthWest (int column, int row) {
		if (row == maze.GetUpperBound (1) - 1) {
			return false;
		}
		if (column == 0) {
			return true;
		}
		return maze [column - 1, row + 1] == 1;
	}

	private bool blockToSouth (int column, int row) {
		if (row == 1) {
			return false;
		}
		return maze [column, row - 1] == 1;
	}

	private bool blockToSouthEast (int column, int row) {
		if (row == 1) {
			return false;
		}
		if (column == maze.GetUpperBound (0)) {
			return true;
		}
		return maze [column + 1, row - 1] == 1;
	}

	private bool blockToSouthWest (int column, int row) {
		if (row == 1) {
			return false;
		}
		if (column == 0) {
			return true;
		}
		return maze [column - 1, row - 1] == 1;
	}

	private bool blockToWest (int column, int row) {
		if (column == 0) {
			return true;
		}
		return maze [column - 1, row] == 1;
	}

	private void buildFarLeftColumn (List<int> xValues, Transform parent) {
		GameObject first = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Ridge Side Block South"));
		first.transform.position = new Vector3 (xValues [0] - (scale / 2), 0, scale / 2);
		first.transform.parent = parent;
		for (int i = 2 * scale; i <= (height - 3) * scale; i += scale) {
			GameObject middle = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Middle Block"));
			middle.transform.position = new Vector3 (xValues[0] - (scale / 2), 0, i - (scale / 2));
			middle.transform.parent = parent;
		}
		GameObject last = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Ridge Side Block North"));
		last.transform.position = new Vector3 (xValues [0] - (scale / 2), 0, (height - 2) * scale - (scale / 2));
		last.transform.parent = parent;
	}

	private void buildFarRightColumn (List<int> xValues, Transform parent) {
		GameObject firstInFarRight = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Ridge Side Block South"));
		firstInFarRight.transform.position = new Vector3 (xValues [3] - (scale / 2), 0, scale / 2);
		firstInFarRight.transform.parent = parent;
		for (int i = 2 * scale; i <= (height - 3) * scale; i += scale) {
			GameObject middleInFarRight = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Middle Block"));
			middleInFarRight.transform.position = new Vector3 (xValues[3] - (scale / 2), 0, i - (scale / 2));
			middleInFarRight.transform.parent = parent;
		}
		GameObject lastInFarRight = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/Ridge Side Block North"));
		lastInFarRight.transform.position = new Vector3 (xValues [3] - (scale / 2), 0, (height - 2) * scale - (scale / 2));
		lastInFarRight.transform.parent = parent;
	}

	private void buildInnerLeftColumn (List<int> xValues, Transform parent) {
		setupFirstInnerLeft (xValues, parent);
		for (int i = 2 * scale; i <= (height - 3) * scale; i += scale) {
			setupMiddleInnerLeft (xValues, parent, i);
		}
		setupLastInnerLeft (xValues, parent);
	}

	private void buildInnerRightColumn(List<int> xValues, Transform parent) {
		setupFirstInnerRight (xValues, parent);
		for (int i = 2 * scale; i <= (height - 3) * scale; i += scale) {
			setupMiddleInnerRight (xValues, parent, i);
		}
		setupLastInnerRight (xValues, parent);
	}

	private void createBorders () {
		List<int> xValues = new List<int> () { -2 * scale, -scale, width * scale, width * scale + scale};
		Transform parent = GameObject.Find ("Generated Borders").transform;
		buildFarLeftColumn (xValues, parent);
		buildInnerLeftColumn (xValues, parent);
		buildInnerRightColumn (xValues, parent);
		buildFarRightColumn (xValues, parent);
	}

	private string determineFirstInnerLeftTerrrain () {
		if (maze [0, 1] == 1) {
			if (maze [0, 2] == 1) {
				return "Ridge Side Block South";
			} else {
				return "Ridge Side Block South Cove North East";
			}
		}
		return "Ridge Corner Block South East";
	}

	private string determineFirstInnerRightTerrain () {
		if (maze [maze.GetUpperBound (0), 1] == 1) {
			if (maze [maze.GetUpperBound (0), 2] == 1) {
				return "Ridge Side Block South";
			} else {
				return "Ridge Side Block South Cove North West";
			}
		}
		return "Ridge Corner Block South West";
	}

	private string determineLastInnerLeftTerrain () {
		if (maze [0, height - 2] == 1) {
			if (maze [0, height - 3] == 1) {
				return "Ridge Side Block North";
			} else {
				return "Ridge Side Block North Cove South East";
			}
		}
		return "Ridge Corner Block North East";
	}

	private string determineLastInnerRightTerrain () {
		if (maze [maze.GetUpperBound (0), height - 2] == 1) {
			if (maze [maze.GetUpperBound (0), height - 3] == 1) {
				return "Ridge Side Block North";
			} else {
				return "Ridge Side Block North Cove South West";
			}
		}
		return "Ridge Corner Block North West";
	}

	private string determineMiddleInnerLeftTerrain (int rowIdx) {
		if (maze [0, rowIdx] != 0) {
			if (maze [0, rowIdx - 1] == 1 && maze [0, rowIdx + 1] == 0) {
				return "Cove Corner Block North East";
			}
			if (maze [0, rowIdx - 1] == 0 && maze [0, rowIdx + 1] == 1) {
				return "Cove Corner Block South East";
			}
			if (maze [0, rowIdx - 1] == 1 && maze [0, rowIdx + 1] == 1) {
				return "Middle Block";
			} else {
				return "T Ridge Block East";
			}
		}
		return "Ridge Side Block East";
	}

	private string determineMiddleInnerRightTerrain (int rowIdx) {
		if (maze [maze.GetUpperBound (0), rowIdx] != 0) {
			if (maze [maze.GetUpperBound (0), rowIdx - 1] == 1 && maze [maze.GetUpperBound (0), rowIdx + 1] == 0) {
				return "Cove Corner Block North West";
			}
			if (maze [maze.GetUpperBound (0), rowIdx - 1] == 0 && maze [maze.GetUpperBound (0), rowIdx + 1] == 1) {
				return "Cove Corner Block South West";
			}
			if (maze [maze.GetUpperBound (0), rowIdx - 1] == 1 && maze [maze.GetUpperBound (0), rowIdx + 1] == 1) {
				return "Middle Block";
			} else {
				return "T Ridge Block West";
			}
		}
		return "Ridge Side Block West";
	}

	private string determineTerrainForSpot (bool north, bool northEast, bool east, bool southEast, bool south, bool southWest, bool west, bool northWest) {
		if (!north && east && !south && west) {
			return "Middle Wall Block Horizontal";
		}
		if (north && !east && south && !west) {
			return "Middle Wall Block Vertical";
		}
		if (!north && !east && !south && west) {
			return "End Wall Block East";
		}
		if (!north && east && !south && !west) {
			return "End Wall Block West";
		}
		if (north && !east && !south && !west) {
			return "End Wall Block South";
		}
		if (!north && !east && south && !west) {
			return "End Wall Block North";
		}
		if (north && east && !south && !west) {
			if (northEast) {
				return "Ridge Corner Block South West";
			} else {
				return "Corner Wall Block South West";
			}
		}
		if (north && !east && !south && west) {
			if (northWest) {
				return "Ridge Corner Block South East";
			} else {
				return "Corner Wall Block South East";
			}
		}
		if (!north && east && south && !west) {
			if (southEast) {
				return "Ridge Corner Block North West";
			} else {
				return "Corner Wall Block North West";
			}
		}
		if (!north && !east && south && west) {
			if (southWest) {
				return "Ridge Corner Block North East";
			} else {
				return "Corner Wall Block North East";
			}
		}
		if (!north && !east && !south && !west) {
			return "Single Block";
		}
		if (north && !east && south && west) {
			if (northWest && southWest) {
				return "Ridge Side Block East";
			} else {
				return "T Wall Block East";
			}
		}
		if (north && east && south && !west) {
			if (northEast && southEast) {
				return "Ridge Side Block West";
			} else {
				return "T Wall Block West";
			}
		}
		if (north && east && !south && west) {
			if (northEast && northWest) {
				return "Ridge Side Block South";
			} else {
				return "T Wall Block South";
			}
		}
		if (!north && east && south && west) {
			if (southEast && southWest) {
				return "Ridge Side Block North";
			} else {
				return "T Wall Block North";
			}
		}
		if (!northEast && !southEast) {
			return "T Ridge Block East";
		}
		if (!northWest && !southWest) {
			return "T Ridge Block West";
		}
		return "Middle Block";
	}

	private void setupFirstInnerLeft (List<int> xValues, Transform parent) {
		GameObject first = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineFirstInnerLeftTerrrain ()));
		first.transform.position = new Vector3 (xValues [1] - (scale / 2), 0, scale / 2);
		first.transform.parent = parent;
	}

	private void setupFirstInnerRight (List<int> xValues, Transform parent) {
		GameObject first = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineFirstInnerRightTerrain ()));
		first.transform.position = new Vector3 (xValues[2] - (scale / 2), 0, scale / 2);
		first.transform.parent = parent;
	}

	private void setupLastInnerLeft (List<int> xValues, Transform parent) {
		GameObject last = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineLastInnerLeftTerrain ()));
		last.transform.position = new Vector3 (xValues [1] - (scale / 2), 0, (height - 2) * scale - (scale / 2));
		last.transform.parent = parent;
	}

	private void setupLastInnerRight (List<int> xValues, Transform parent) {
		GameObject last = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineLastInnerRightTerrain ()));
		last.transform.position = new Vector3 (xValues [2] - (scale / 2), 0, (height - 2) * scale - (scale / 2));
		last.transform.parent = parent;
	}

	private void setupMiddleInnerLeft (List<int> xValues, Transform parent, int idx) {
		GameObject middle = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineMiddleInnerLeftTerrain (idx / scale)));
		middle.transform.position = new Vector3 (xValues[1] - (scale / 2), 0, idx - (scale / 2));
		middle.transform.parent = parent;
	}

	private void setupMiddleInnerRight (List<int> xValues, Transform parent, int idx) {
		GameObject middle = GameObject.Instantiate (Resources.Load<GameObject>("GamePrefabs/TerrainBlocks/"+determineMiddleInnerRightTerrain (idx / scale)));
		middle.transform.position = new Vector3 (xValues [2] - (scale / 2), 0, idx - (scale / 2));
		middle.transform.parent = parent;
	}
}
