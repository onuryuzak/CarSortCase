using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Onur.Template;
using PathCreation;
using UnityEngine.AI;

public class CarPathCreator : MonoBehaviour
{
    private PathCreator _pathCreator;

    public GridHolder[] gridHolders;
    [SerializeField] private float pathYOffset;
    private void Start()
    {
        gridHolders = GetComponentsInChildren<GridHolder>();
        _pathCreator = GetComponent<PathCreator>();
    }

    public List<GridHolder> GetEmptyGridHolder(Vector3 from)
    {
        return gridHolders.Where(x => x.GetAvailableGrid().Item1)
        .OrderBy(y => Mathf.Abs(y.transform.position.x - from.x))
        .ThenBy(x => Vector3.Distance(from, x.transform.position)).Reverse().ToList();
    }

    public (VertexPath, GridController) GetPath(Vector3 from, GridHolder[] carGridHolders)
    {
        (GridController, BezierPath) gridValueTuple = FindGridAndPath(from, carGridHolders);
        if (gridValueTuple.Item1)
        {
            gridValueTuple.Item1.Targeted(true);
            _pathCreator.bezierPath.GlobalNormalsAngle = 90;
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Free;
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            return (_pathCreator.path, gridValueTuple.Item1);
        }
        else return (null, null);
    }

    private (GridController, BezierPath) FindGridAndPath(Vector3 from, GridHolder[] carGridHolders)
    {
        GridController grid = null;
        Vector3 prevGridPos = from;
        foreach (var carGridHolder in carGridHolders)
        {
            if (grid) prevGridPos = grid.transform.position;
            (GridController, bool) valueTuple = carGridHolder.GetAvailableGrid();
            if (carGridHolder.roadEnd && valueTuple.Item1)
            {
                grid = valueTuple.Item1;
                AddSegmentToPath(from, grid, prevGridPos);
                break;
            }
            else if ((!valueTuple.Item2 && !carGridHolder.roadEnd) || carGridHolder.canCarGo)
            {
                grid = valueTuple.Item1 ? valueTuple.Item1 : grid;
                if (valueTuple.Item1) AddSegmentToPath(from, grid, prevGridPos);
                break;
            }
            else if (valueTuple.Item1)
            {
                grid = valueTuple.Item1;
                AddSegmentToPath(from, grid, prevGridPos);
            }
        }
        return (grid, _pathCreator.bezierPath);
    }

    private void AddSegmentToPath(Vector3 from, GridController grid, Vector3 prevGridPos)
    {
        if (prevGridPos == from && grid)
        {
            _pathCreator.bezierPath = new BezierPath(FindNavmeshCorners(from, grid.transform.position));
        }
        else if (prevGridPos != Vector3.zero && grid)
        {
            Vector3[] pathCorners = FindNavmeshCorners(prevGridPos, grid.transform.position);
            for (int i = 1; i < pathCorners.Length; i++)
            {
                _pathCreator.bezierPath.AddSegmentToEnd(pathCorners[i]);
            }
        }
    }

    private Vector3[] FindNavmeshCorners(Vector3 from, Vector3 to)
    {
        var path = new NavMeshPath();
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length; i++)
        {
            path.corners[i] = path.corners[i] + new Vector3(0, pathYOffset, 0);
        }
        return path.corners;
    }
}
