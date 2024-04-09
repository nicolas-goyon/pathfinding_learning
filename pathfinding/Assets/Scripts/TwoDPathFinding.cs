using System;
using System.Collections.Generic;
using UnityEngine;

public enum EuristicType {
    VECTOR,
    MANHATTAN
}

public class TwoDPathFinding {
    private MyGrid<MapCell> grid;
    private readonly GridElement<MapCell> playerCell;
    private readonly GridElement<MapCell> objectiveCell;

    private List<PathFindingNode<GridElement<MapCell>>> visitedNodes;
    private List<PathFindingNode<GridElement<MapCell>>> frontierNodes;

    private PathFindingNode<GridElement<MapCell>> currentNode;

    private readonly Func<GridElement<MapCell>, GridElement<MapCell>, float> CalcEuristic;



    public bool solved { get; private set; } = false;
    private bool started = false;


    //private List<GridElement<MapCell>> solution;

    public TwoDPathFinding(MyGrid<MapCell> grid, EuristicType euristic) { 
        this.grid = grid;
        CalcEuristic = euristic switch {
            EuristicType.VECTOR => CalcEuristicVector,
            EuristicType.MANHATTAN => CalcEuristicManhattan,
            _ => CalcEuristicVector
        };

        for (int x = 0; x < grid.GridArray.GetLength(0); x++) {
            for (int y = 0; y < grid.GridArray.GetLength(1); y++) {
                GridElement<MapCell> cell = grid.GetGridElement(x, y);  
                if (cell.Data.Type == CellType.PLAYER) {
                    playerCell = cell;
                }
                else if (cell.Data.Type == CellType.OBJECTIVE) {
                    objectiveCell = cell;
                }
            }
        }
    }

    public void StartStep() {
        float euristic = CalcEuristic(playerCell, objectiveCell);
        playerCell.SetValue((int) euristic);

        visitedNodes = new();
        frontierNodes = new List<PathFindingNode<GridElement<MapCell>>> {
            new(playerCell)
        };

        started = true;
    }

    public void SolveStep() {

        if (currentNode != null) {
            currentNode.element.Data.SetType(CellType.VISITED);
            currentNode.element.RefreshDisplay();
        }

        currentNode = frontierNodes[0];
        currentNode.element.Data.SetType(CellType.SELECTED);
        currentNode.element.RefreshDisplay();
        frontierNodes.RemoveAt(0);


        List<GridElement<MapCell>> neighbors = grid.GetNeighbors(currentNode.element.Data.X, currentNode.element.Data.Y);


        List<GridElement<MapCell>> validNeighbors = new();

        foreach (GridElement<MapCell> neighbor in neighbors) {
            if (visitedNodes.Exists(node => node.element == neighbor)) {
                continue;
            }

            if (frontierNodes.Exists(node => node.element == neighbor)) {
                continue;
            }

            if (neighbor.Data.Type != CellType.WALL) {
                validNeighbors.Add(neighbor);
            }

            if (neighbor.Data.Type == CellType.OBJECTIVE) {
                visitedNodes.Add(currentNode);
                visitedNodes.Add(new(neighbor, currentNode));
                solved = true;
                return;
            }
        }


        foreach (GridElement<MapCell> neighbor in validNeighbors) {
            PathFindingNode<GridElement<MapCell>> node = new(neighbor, currentNode);
            node.SetType(CellType.FRONTIER);
            node.SetEuristic(CalcEuristic(neighbor, objectiveCell));
            frontierNodes.Add(node);
        }

        visitedNodes.Add(currentNode);
        // TODO : Use a priority queue instead : https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-8.0
        frontierNodes.Sort((node1, node2) => node1.EuristicCost.CompareTo(node2.EuristicCost)); //

    }

    public void EndStep() {
        if (solved) {
            List<GridElement<MapCell>> path = new();
            PathFindingNode<GridElement<MapCell>> currentNode = visitedNodes[^1];
            while (currentNode.parent != null) {
                path.Add(currentNode.element);
                //currentNode.element.Data.SetType(CellType.PATH);
                currentNode = currentNode.parent;
            }

            path.Add(playerCell);

            path.Reverse();

            foreach (GridElement<MapCell> cell in path) {
                if(cell.Data.Type != CellType.PLAYER && cell.Data.Type != CellType.OBJECTIVE) {
                    cell.Data.SetType(CellType.PATH);
                }
                cell.RefreshDisplay();
                Debug.Log(cell.Data.X + " " + cell.Data.Y);
            }

        }
    }

    public void nextStep() {
        if (!started) {
            StartStep();
        }
        else if (!solved) {
            SolveStep();
        }
        else {
            EndStep();
        }
       
    }

    private float CalcEuristicVector(GridElement<MapCell> origin, GridElement<MapCell> destination) {
        // TODO : use square distance instead of square root 
        return Vector2.Distance(new Vector2(origin.Data.X, origin.Data.Y), new Vector2(destination.Data.X, destination.Data.Y));
        // using Manhattan distance
        // TODO : try use a cache : https://stackoverflow.com/questions/754233/is-it-there-any-lru-implementation-of-idictionary
        //return Mathf.Abs(origin.Data.X - destination.Data.X) + Mathf.Abs(origin.Data.Y - destination.Data.Y);
    }

    private float CalcEuristicManhattan(GridElement<MapCell> origin, GridElement<MapCell> destination) {
        return Mathf.Abs(origin.Data.X - destination.Data.X) + Mathf.Abs(origin.Data.Y - destination.Data.Y);
        
    }



    
}
