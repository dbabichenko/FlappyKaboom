using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator2D : MonoBehaviour
{
    [Header("Maze Settings")]
    [Min(2)] public int width = 10;
    [Min(2)] public int height = 10;
    [Tooltip("Size of each maze cell in world units.")]
    public float cellSize = 1f;
    [Tooltip("Visual thickness of each wall (local Y scale for horizontal walls).")]
    public float wallThickness = 0.1f;

    [Header("Prefabs & Hierarchy")]
    [Tooltip("Single wall prefab used for both horizontal and vertical walls.")]
    public GameObject wallPrefab;
    [Tooltip("Optional parent for the generated walls. If empty, walls are parented to this GameObject.")]
    public Transform mazeParent;

    [Header("Options")]
    [Tooltip("Open an entry and exit (west of (0,0) and east of (width-1,height-1)).")]
    public bool createEntranceAndExit = true;
    [Tooltip("Generate a new maze on Start().")]
    public bool generateOnStart = false;
    [Tooltip("Use a deterministic seed. Leave empty for fully random.")]
    public int seed = 0;
    public bool useSeed = false;

    private Cell[,] grid;

    private struct Cell
    {
        public bool visited;
        // Walls: N,E,S,W (all true = present)
        public bool N, E, S, W;

        public void Init()
        {
            visited = false;
            N = E = S = W = true;
        }
    }

    void Awake()
    {
        if (mazeParent == null) mazeParent = transform;
    }

    void Start()
    {
        if (generateOnStart) Generate();
    }

    [ContextMenu("Generate Maze")]
    public void Generate()
    {
        if (wallPrefab == null)
        {
            Debug.LogError("[MazeGenerator2D] Please assign a wallPrefab.");
            return;
        }

        ClearMaze();

        if (useSeed)
            Random.InitState(seed);
        else
            Random.InitState(System.Environment.TickCount);

        // 1) Init grid
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y].Init();

        // 2) Carve maze with DFS backtracking
        CarveDFS();

        // 3) Optional entrance/exit
        if (createEntranceAndExit)
        {
            // Entrance: west wall of (0,0)
            grid[0, 0].W = false;
            // Exit: east wall of (width-1,height-1)
            grid[width - 1, height - 1].E = false;
        }

        // 4) Build walls
        BuildWalls();

        // 5) Center the maze around this object (optional nicety)
        CenterMazeTransform();
    }

    [ContextMenu("Clear Maze")]
    public void ClearMaze()
    {
        // Destroys previously generated children (only the ones we created)
        var toDestroy = new List<Transform>();
        foreach (Transform child in mazeParent)
            toDestroy.Add(child);

#if UNITY_EDITOR
        // Safe in editor too
        foreach (var t in toDestroy)
            DestroyImmediate(t.gameObject);
#else
        foreach (var t in toDestroy)
            Destroy(t.gameObject);
#endif
    }

    // ---- Generation (DFS Backtracker) ----
    private void CarveDFS()
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int current = new Vector2Int(0, 0);
        grid[current.x, current.y].visited = true;
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Peek();
            var neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                // Choose random neighbor
                Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                // Knock down wall between current and next
                RemoveWallBetween(current, next);
                grid[next.x, next.y].visited = true;
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int c)
    {
        var list = new List<Vector2Int>();

        // Up (N)
        if (c.y + 1 < height && !grid[c.x, c.y + 1].visited)
            list.Add(new Vector2Int(c.x, c.y + 1));
        // Right (E)
        if (c.x + 1 < width && !grid[c.x + 1, c.y].visited)
            list.Add(new Vector2Int(c.x + 1, c.y));
        // Down (S)
        if (c.y - 1 >= 0 && !grid[c.x, c.y - 1].visited)
            list.Add(new Vector2Int(c.x, c.y - 1));
        // Left (W)
        if (c.x - 1 >= 0 && !grid[c.x - 1, c.y].visited)
            list.Add(new Vector2Int(c.x - 1, c.y));

        return list;
    }

    private void RemoveWallBetween(Vector2Int a, Vector2Int b)
    {
        Vector2Int d = b - a;

        if (d.x == 1 && d.y == 0)
        {
            // b is to the East of a
            grid[a.x, a.y].E = false;
            grid[b.x, b.y].W = false;
        }
        else if (d.x == -1 && d.y == 0)
        {
            // b is to the West of a
            grid[a.x, a.y].W = false;
            grid[b.x, b.y].E = false;
        }
        else if (d.x == 0 && d.y == 1)
        {
            // b is to the North of a
            grid[a.x, a.y].N = false;
            grid[b.x, b.y].S = false;
        }
        else if (d.x == 0 && d.y == -1)
        {
            // b is to the South of a
            grid[a.x, a.y].S = false;
            grid[b.x, b.y].N = false;
        }
    }

    // ---- Building walls from the carved grid ----
    private void BuildWalls()
    {
        // We place only unique walls to avoid duplicates:
        // For each cell, place North and West walls if present.
        // After the loop, place the South walls for y==0 row, and East walls for x==width-1 column.

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 center = CellCenter(x, y);

                // North wall (horizontal)
                if (grid[x, y].N)
                {
                    Vector2 pos = center + new Vector2(0f, cellSize * 0.5f);
                    CreateWall(pos, vertical: false);
                }

                // West wall (vertical)
                if (grid[x, y].W)
                {
                    Vector2 pos = center + new Vector2(-cellSize * 0.5f, 0f);
                    CreateWall(pos, vertical: true);
                }

                // East boundary for last column (vertical)
                if (x == width - 1 && grid[x, y].E)
                {
                    Vector2 pos = center + new Vector2(cellSize * 0.5f, 0f);
                    CreateWall(pos, vertical: true);
                }

                // South boundary for bottom row (horizontal)
                if (y == 0 && grid[x, y].S)
                {
                    Vector2 pos = center + new Vector2(0f, -cellSize * 0.5f);
                    CreateWall(pos, vertical: false);
                }
            }
        }
    }

    private Vector2 CellCenter(int x, int y)
    {
        // Bottom-left of the maze at (0,0); center each cell
        return new Vector2(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f);
    }

    private void CreateWall(Vector2 position, bool vertical)
    {
        GameObject wall = Instantiate(wallPrefab, mazeParent);
        wall.transform.position = new Vector3(position.x, position.y, 0f);

        // Rotate vertical walls by 90° around Z
        wall.transform.rotation = vertical
            ? Quaternion.Euler(0f, 0f, 90f)
            : Quaternion.identity;

        // Scale so the wall spans one cell in length, with the requested thickness.
        // Assumes the prefab's localScale = (1,1,1) represents a 1x1 unit.
        // Horizontal wall: X = cellSize (length), Y = wallThickness
        // Vertical wall: we rotated it, so same scale works.
        wall.transform.localScale = new Vector3(cellSize, wallThickness, 1f);
    }

    private void CenterMazeTransform()
    {
        // Move the parent so the maze is centered around this component's position
        // (purely visual; adjust to taste)
        Vector2 mazeSize = new Vector2(width * cellSize, height * cellSize);
        Vector3 offset = new Vector3(-mazeSize.x * 0.5f, -mazeSize.y * 0.5f, 0f);

        // Shift all child walls by offset so the overall maze is centered
        foreach (Transform child in mazeParent)
        {
            child.position += offset;
        }
    }
}
