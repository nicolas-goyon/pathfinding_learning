using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCell : IGridElement {
    public float BaseValue { get; private set; }
    public int value {
        get {
            if (BaseValue == float.MaxValue) {
                return int.MaxValue;
            }
            return (int) BaseValue;
        }
        private set {
            BaseValue = value;
        }
    }
    public CellType Type { get; private set; }
    public CellType BaseType { get; private set; }

    public int X { get; private set; }
    public int Y { get; private set; }

    public MapCell(CellType type, int x, int y) { 
        Type = type;
        BaseType = type;
        X = x;
        Y = y;
        BaseValue = float.MaxValue;
    }

    public void SetValue(int value) {
        this.value = value;
    }

    public void SetType(CellType type) {
        Type = type;
    }

    internal void SetBaseValue(float euristic) {
        BaseValue = euristic;
    }

    public void SetBaseType(CellType type) {
        BaseType = type;
        Type = type;
    }
}
