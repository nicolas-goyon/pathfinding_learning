using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : IGridElement {

    public int value { get; private set; }
    public CellType type { get; private set; }

    public int x { get; private set; }
    public int y { get; private set; }

    public MapCell(CellType type, int x, int y) { 
        this.type = type;
        this.x = x;
        this.y = y;
        this.value = int.MaxValue;
    }

    public void SetValue(int value) {
        this.value = value;
    }

    public void SetType(CellType type) {
        this.type = type;
    }
}
