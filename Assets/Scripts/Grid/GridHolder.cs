using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    public GridController[] _carGrids;
    public bool roadEnd;
    public bool canCarGo;

    private void Awake()
    {
        _carGrids = GetComponentsInChildren<GridController>();
    }

    public (GridController, bool) GetAvailableGrid()
    {
        GridController car = null;
        foreach (var carGrid in _carGrids)
        {
            if (!carGrid.IsEmpty)
                return (car, false);
            else car = carGrid;
        }
        return (car, true);
    }
}
