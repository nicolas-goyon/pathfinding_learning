using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode<T> where T : GridElement<MapCell>{
    
    public PathFindingNode<T> parent;
    public T element;


    public PathFindingNode(T element, PathFindingNode<T> parent = null) {
        this.parent = parent;
        this.element = element;
    }
}
