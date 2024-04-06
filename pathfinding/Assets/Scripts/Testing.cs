using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private MyGrid<CellData> grid;
    [SerializeField] private Bouton startButton;


    [SerializeField] private int width = 4;
    [SerializeField] private int height = 2;

    // Start is called before the first frame update
    private void Start()
    {
        // grid = new MyGrid(4, 2, 10f, new Vector3(20, 0));
        startButton.OnClick += OnStart;
    }

    // Update is called once per frame
    private void Update() {
        if (grid != null) {
            if (Input.GetMouseButton(0)) { 
                try {
                    int value = grid.GetValue(GetMouseWorldPosition());
                    grid.SetValue(GetMouseWorldPosition(), value - 1);
                }
                catch {
                    Debug.Log("-");
                }
            }
            if (Input.GetMouseButton(1)) {
                try {
                    int value = grid.GetValue(GetMouseWorldPosition());
                    Debug.Log(value);
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
        grid = new MyGrid<CellData>(width, height, 10f, selfOrigin, (int x, int y) => new CellData(100));
    }
}
