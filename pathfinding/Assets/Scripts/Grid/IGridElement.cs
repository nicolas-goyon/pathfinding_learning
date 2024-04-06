using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridElement
{
    public int value { get; }

    public void SetValue(int value);
}
