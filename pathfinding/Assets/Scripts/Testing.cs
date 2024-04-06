using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MyGrid<MapCell> grid;
    [SerializeField] private Bouton startButton;


    [SerializeField] private Bouton startSolver;
    private bool isSolverRunning = false;

    private Timer timer;


    private TwoDPathFinding pathFinding;
    

    // Start is called before the first frame update
    private void Start()
    {
        // grid = new MyGrid(4, 2, 10f, new Vector3(20, 0));
        startButton.OnClick += OnStart;
        startSolver.OnClick += OnStartSolver;
    }

    // Update is called once per frame
    private void Update() {
        if (grid != null) {
            if (Input.GetMouseButtonDown(1)) {
                try {
                    int value = grid.GetValue(GetMouseWorldPosition());
                    CellType type = grid.getData(GetMouseWorldPosition()).type;

                    Debug.Log("Value: " + value + " Type: " + type);
                }
                catch {
                    Debug.Log("-");

                }
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
        grid = new(map.GetLength(0), map.GetLength(1), 10f, selfOrigin, (int x, int y) => new MapCell(GetCellType(map[x, y]), x, y), new MapCellDisplay<MapCell>());
        pathFinding = new(grid);
    }

    private void OnStartSolver(object sender, System.EventArgs e) {
        //if (isSolverRunning) {
        //    isSolverRunning = false;
        //    this.timer.Stop();
        //}
        //isSolverRunning = true;
        //float loopTimeMs = 1000;
        //this.timer = new(loopTimeMs);
        //this.timer.Elapsed += (object sender, ElapsedEventArgs e) => {
        pathFinding.nextStep();
        //};
        //this.timer.Start();
    }

    private CellType GetCellType(char c) {
        switch (c) {
            case 'W':
                return CellType.WALL;
            case 'F':
                return CellType.FLOOR;
            case 'P':
                return CellType.PLAYER;
            case 'O':
                return CellType.OBJECTIVE;
            default:
                return CellType.FLOOR;
        }
    }










    private readonly char[,] uiFriendlyMap = new char[,] {
        {'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W','W', 'W', 'W', 'W','W', 'W', 'W', 'W' } // 0
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F','F', 'W', 'F', 'F','F', 'F', 'O', 'W' } // 1
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'F', 'F','F', 'W', 'F', 'F','F', 'F', 'F', 'W' } // 2
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'W', 'F','F', 'W', 'F', 'F','F', 'F', 'F', 'W' } // 3
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'W', 'F','F', 'W', 'F', 'F','F', 'F', 'F', 'W' } // 4
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'W', 'F','F', 'W', 'F', 'F','F', 'F', 'F', 'W' } // 5
        ,{'W', 'F', 'F', 'F', 'F', 'F', 'W', 'F','F', 'F', 'F', 'F','F', 'F', 'F', 'W' } // 6
        ,{'W', 'P', 'F', 'F', 'F', 'F', 'W', 'F','F', 'F', 'F', 'F','F', 'F', 'F', 'W' } // 7
        ,{'W', 'W', 'W', 'W', 'W', 'W', 'W', 'W','W', 'W', 'W', 'W','W', 'W', 'W', 'W' } // 8
    };





    private readonly char[,] map;

    Testing() {
        
        char[,] newMap = new char[uiFriendlyMap.GetLength(1), uiFriendlyMap.GetLength(0)];
        for (int x = 0; x < uiFriendlyMap.GetLength(0); x++) {
            for (int y = 0; y < uiFriendlyMap.GetLength(1); y++) {
                newMap[y, x] = uiFriendlyMap[uiFriendlyMap.GetLength(0) - 1 - x,  y];
            }
        }

        map = newMap;
    }


}
