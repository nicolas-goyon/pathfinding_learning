using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyGrid<T> where T : IGridElement
{

    public readonly int Width;
    public readonly int Height;
    private readonly Vector3 originPosition;
    private readonly float cellSize;
    public GridElement<T>[,] GridArray { get; private set; }

    public MyGrid(int width, int height, float cellsSize, Vector3 originPosition, Func<int, int, T> baseValInitFunc, GridElementDisplayer<T> displayer) { 
        Width = width;
        Height = height;
        cellSize = cellsSize;
        this.originPosition = originPosition;
        
        GridArray = new GridElement<T>[width, height];

        for (int x = 0; x < GridArray.GetLength(0); x++) {
            for (int y = 0; y < GridArray.GetLength(1); y++) {
                GridArray[x, y] = new GridElement<T>(baseValInitFunc(x,y));
                GridArray[x, y].Display(displayer, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, cellSize);
            }
        }

    }

    public void DrawLines() {
        for (int x = 0; x < GridArray.GetLength(0); x++) {
            for (int y = 0; y < GridArray.GetLength(1); y++) {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
            Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
        }
        Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 100f);
    }


    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + originPosition;
    }



    public void SetValue(int x, int y, int value) { 
        if (x < 0 || y < 0 || x >= Width || y >= Height) {
            throw new Exception("Index out of bounds");
        }

        GridArray[x, y].SetValue(value);

    }

    public void SetValue(Vector3 worldPosition, int value) {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y) {
        if ((x < 0 || y < 0 || x >= Width || y >= Height)) {
            throw new Exception("Index out of bounds");
        }
        
        return GridArray[x, y].Value;
    }

    public int GetValue(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }


    public void SetData(int x, int y, T data) {
        if (x < 0 || y < 0 || x >= Width || y >= Height) {
            throw new Exception("Index out of bounds");
        }

        GridArray[x, y].SetData(data);
    }

    public void SetData(Vector3 worldPosition, T data) {
        GetXY(worldPosition, out int x, out int y);
        SetData(x, y, data);
    }

    public T GetData(int x, int y) {
        if ((x < 0 || y < 0 || x >= Width || y >= Height)) {
            throw new Exception("Index out of bounds");
        }

        return GridArray[x, y].Data;
    }

    public T GetData(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetData(x, y);
    }

    public List<GridElement<T>> GetNeighbors(int x, int y) {
        List<GridElement<T>> neighbors = new();

        if (x - 1 >= 0) {
            neighbors.Add(GetGridElement(x - 1, y));
        }
        if (x + 1 < Width) {
            neighbors.Add(GetGridElement(x + 1, y));
        }
        if (y - 1 >= 0) {
            neighbors.Add(GetGridElement(x, y - 1));
        }
        if (y + 1 < Height) {
            neighbors.Add(GetGridElement(x, y + 1));
        }

        return neighbors;
    }

    public GridElement<T> GetGridElement(int x, int y) {
        return GridArray[x, y];
    }

    public GridElement<T> GetGridElement(Vector3 position) {
        GetXY(position, out int x, out int y);
        return GetGridElement(x, y);
    }





}
