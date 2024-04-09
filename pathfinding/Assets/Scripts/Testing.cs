using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MyGrid<MapCell> grid;
    private MyGrid<MapCell> secondGrid;
    [SerializeField] private Bouton startButton;
    [SerializeField] private int Width = 20;
    [SerializeField] private int Height = 10;
    [SerializeField] private float Scale = 1;
    [SerializeField] private GameObject SecondGridOrigin;

    [SerializeField] private Bouton startSolver;
    private bool isSolverRunning = false;


    private TwoDPathFinding pathFinding;
    private TwoDPathFinding pathFinding2;

    private bool isPathfindingDone = false;
    private bool isPathfinding2Done = false;



    // Start is called before the first frame update
    private void Start()
    {
        startButton.OnClick += OnStart;
        //startSolver.OnClick += NextSolverStep;
        startSolver.OnClick += (sender, e) => {
            isSolverRunning = true;
        };
    }


    // Update is called once per frame
    private void Update() {
        // if previous step is was 500ms ago then do next step
        Time.fixedDeltaTime = 0.5f;
        if (isSolverRunning && !isPathfinding2Done && !isPathfindingDone) { 
            NextSolverStep(null, null);
        }



        if (grid == null || secondGrid == null) { 
            return;
        }

        if (!isSolverRunning) {
            if (Input.GetMouseButton(0)) {
                try {
                    Vector3 mouseWorldPosition = GetMouseWorldPosition();
                    grid.GetXY(mouseWorldPosition, out int x, out int y);
                    
                    GridElement<MapCell> element = grid.GetGridElement(mouseWorldPosition);


                    if (element.Data.Type == CellType.FLOOR || element.Data.Type == CellType.WALL) {
                        element.Data.SetBaseType(CellType.WALL);
                        element.RefreshDisplay();
                    }

                    GridElement<MapCell> element2 = secondGrid.GetGridElement(x,y);
                    if (element2.Data.Type == CellType.FLOOR || element2.Data.Type == CellType.WALL) {
                        element2.Data.SetBaseType(CellType.WALL);
                        element2.RefreshDisplay();
                    }
                }
                catch { }
            }
            if(Input.GetMouseButton(1)) {
                try {
                    Vector3 mouseWorldPosition = GetMouseWorldPosition();
                    grid.GetXY(mouseWorldPosition, out int x, out int y);

                    GridElement<MapCell> element = grid.GetGridElement(mouseWorldPosition);

                    if (element.Data.Type == CellType.FLOOR || element.Data.Type == CellType.WALL) {
                        element.Data.SetBaseType(CellType.FLOOR);
                        element.RefreshDisplay();
                    }

                    GridElement<MapCell> element2 = secondGrid.GetGridElement(x, y);
                    if (element2.Data.Type == CellType.FLOOR || element2.Data.Type == CellType.WALL) {
                        element2.Data.SetBaseType(CellType.FLOOR);
                        element2.RefreshDisplay();
                    }
                }
                catch { }
            }

        }

    }
    private Vector3 GetMouseWorldPosition() {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }

    private void OnStart(object sender, System.EventArgs e) { 
        Vector3 selfOrigin = this.transform.position;
        float cellSize = 10f * Scale;
        grid = new(Width, Height, cellSize, selfOrigin, (int x, int y) => new(GetCellType(x, y), x, y), new MapCellDisplay<MapCell>(Scale));
        pathFinding = new(grid, EuristicType.MANHATTAN);


        Vector3 secondGridOrigin = SecondGridOrigin.transform.position;
        secondGrid = new(Width, Height, cellSize, secondGridOrigin, (int x, int y) => new(GetCellType(x, y), x, y), new MapCellDisplay<MapCell>(Scale));
        pathFinding2 = new(secondGrid, EuristicType.VECTOR);
    }

    private void NextSolverStep(object sender, System.EventArgs e) {
        if (grid == null) return;
        isSolverRunning = true;
        if (pathFinding != null) { 
            if (!pathFinding.solved) {
                pathFinding.nextStep();
            }
            else if (!isPathfindingDone) {
                pathFinding.EndStep();
                isPathfindingDone = true;
            }
        }
        if (pathFinding2 != null) { 
            if (!pathFinding2.solved) {
                pathFinding2.nextStep();
            }
            else if (!isPathfinding2Done) {
                pathFinding2.EndStep();
                isPathfinding2Done = true;
            }
        }
    }

    private CellType GetCellType(int x, int y) {
        if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
            return CellType.WALL;
        if (x == 1 && y == 1) return CellType.PLAYER;
        if (x == Width - 2 && y == Height - 2) return CellType.OBJECTIVE;
        return CellType.FLOOR;
    }


}
