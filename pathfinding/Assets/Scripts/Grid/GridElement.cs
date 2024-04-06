using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement<T> where T : IGridElement {
    public int value { get {
            return data.value;
        }
        set {
            data.SetValue(value);
        }
    }

    public T data { get; private set; }

    public GameObject displayElement { get; private set; }
    public Vector2 position { get; private set; }

    public EventHandler OnValueChange;

    public GridElement(T value) {
        this.data = value;
    }

    public void SetValue(int value) {
        this.value = value;
        OnValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void SetData(T data) {
        this.data = data;
        OnValueChange?.Invoke(this, EventArgs.Empty);
    }

    public void display(GridElementDisplayer<T> displayer, Vector3 position, float cellSize) { 
        displayElement = displayer.Display(this, position, cellSize);
        this.position = new Vector2(position.x, position.y);
    }
    
}
