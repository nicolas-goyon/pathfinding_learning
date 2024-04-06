using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyGrid<T> where T : IGridElement
{

    public int width { get; private set; }
    public int height { get; private set; }
    private Vector3 originPosition;
    private float cellSize;
    public GridElement<T>[,] gridArray { get; private set; }

    public MyGrid(int width, int height, float cellsSize, Vector3 originPosition, Func<int, int, T> baseValInitFunc, GridElementDisplayer<T> displayer) { 
        this.width = width;
        this.height = height;
        this.cellSize = cellsSize;
        this.originPosition = originPosition;
        
        gridArray = new GridElement<T>[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                gridArray[x, y] = new GridElement<T>(baseValInitFunc(x,y));
                gridArray[x, y].Display(displayer, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, cellSize);
            }
        }

    }

    public void drawLines() {
        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        }
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }


    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }



    public void SetValue(int x, int y, int value) { 
        if (x < 0 || y < 0 || x >= width || y >= height) {
            throw new Exception("Index out of bounds");
        }

        gridArray[x, y].SetValue(value);

    }

    public void SetValue(Vector3 worldPosition, int value) {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y) {
        if ((x < 0 || y < 0 || x >= width || y >= height)) {
            throw new Exception("Index out of bounds");
        }
        
        return gridArray[x, y].Value;
    }

    public int GetValue(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }


    public void setData(int x, int y, T data) {
        if (x < 0 || y < 0 || x >= width || y >= height) {
            throw new Exception("Index out of bounds");
        }

        gridArray[x, y].SetData(data);
    }

    public void setData(Vector3 worldPosition, T data) {
        GetXY(worldPosition, out int x, out int y);
        setData(x, y, data);
    }

    public T getData(int x, int y) {
        if ((x < 0 || y < 0 || x >= width || y >= height)) {
            throw new Exception("Index out of bounds");
        }

        return gridArray[x, y].Data;
    }

    public T getData(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return getData(x, y);
    }

    public List<GridElement<T>> GetNeighbors(int x, int y) {
        List<GridElement<T>> neighbors = new();

        if (x - 1 >= 0) {
            neighbors.Add(GetGridElement(x - 1, y));
        }
        if (x + 1 < width) {
            neighbors.Add(GetGridElement(x + 1, y));
        }
        if (y - 1 >= 0) {
            neighbors.Add(GetGridElement(x, y - 1));
        }
        if (y + 1 < height) {
            neighbors.Add(GetGridElement(x, y + 1));
        }

        return neighbors;
    }

    public GridElement<T> GetGridElement(int x, int y) {
        return gridArray[x, y];
    }





}
