using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement<T> where T : IGridElement {
    public int Value { get {
            return Data.value;
        }
        set {
            Data.SetValue(value);
        }
    }

    public T Data { get; private set; }

    public GameObject DisplayElement { get; private set; }
    public Vector2 Position { get; private set; }

    private GridElementDisplayer<T> displayer;
    private float cellSize;

    public GridElement(T value) {
        this.Data = value;
    }

    public void SetValue(int value) {
        this.Value = value;
        Display(displayer, new(Position.x, Position.y), cellSize);
    }

    public void SetData(T data) {
        this.Data = data;
        Display(displayer, new(Position.x, Position.y), cellSize);
    }

    public void Display(GridElementDisplayer<T> displayer, Vector3 position, float cellSize) {
        if (DisplayElement != null) {
            GameObject.Destroy(DisplayElement);
        }
        this.DisplayElement = displayer.Display(this, position, cellSize);
        this.displayer = displayer;
        this.cellSize = cellSize;
        this.Position = new(position.x, position.y);
    }

    public void RefreshDisplay() {
        Display(displayer, new(Position.x, Position.y), cellSize);
    }
    
}
