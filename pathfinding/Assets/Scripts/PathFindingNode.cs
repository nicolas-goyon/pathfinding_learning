using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode<T> where T : GridElement<MapCell>{
    
    public PathFindingNode<T> parent;
    public T element;
    public float EuristicCost { 
        get {
            return element.Data.BaseValue;
        }
        private set {
            element.Data.SetBaseValue(value);
        }
    }


    public PathFindingNode(T element, PathFindingNode<T> parent = null) {
        this.parent = parent;
        this.element = element;
    }

    public void SetValue(int value) {
        element.Data.SetValue(value);
        element.RefreshDisplay();
    }

    public void SetType(CellType type) {
        element.Data.SetType(type);
        element.RefreshDisplay();
    }

    public void SetEuristic(float euristic) {
        EuristicCost = euristic;
        element.RefreshDisplay();
    }

}
