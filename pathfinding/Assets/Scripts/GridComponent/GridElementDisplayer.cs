using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GridElementDisplayer<T> where T : IGridElement
{
    public GameObject Display(GridElement<T> element, Vector3 position, float cellSize);
}
