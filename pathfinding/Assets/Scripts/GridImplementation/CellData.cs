using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData : IGridElement { 

    public int value { get; private set; }

    public CellData(int value) {
        this.value = value;
    }

    public void SetValue(int value) {
        this.value = value;
    }
}
