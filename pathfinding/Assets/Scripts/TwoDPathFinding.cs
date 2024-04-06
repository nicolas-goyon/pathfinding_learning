using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TwoDPathFinding {
    private MyGrid<MapCell> grid;
    private readonly GridElement<MapCell> playerCell;
    private readonly GridElement<MapCell> objectiveCell;

    private List<PathFindingNode<GridElement<MapCell>>> visitedNodes;
    private List<PathFindingNode<GridElement<MapCell>>> frontierNodes;

    private PathFindingNode<GridElement<MapCell>> currentNode;

    private bool solved = false;
    private bool started = false;


    //private List<GridElement<MapCell>> solution;

    public TwoDPathFinding(MyGrid<MapCell> grid) {
        this.grid = grid;

        for (int x = 0; x < grid.gridArray.GetLength(0); x++) {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++) {
                GridElement<MapCell> cell = grid.GetGridElement(x, y);  
                if (cell.Data.type == CellType.PLAYER) {
                    playerCell = cell;
                }
                else if (cell.Data.type == CellType.OBJECTIVE) {
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


        List<GridElement<MapCell>> neighbors = grid.GetNeighbors(currentNode.element.Data.x, currentNode.element.Data.y);


        List<GridElement<MapCell>> validNeighbors = new();

        foreach (GridElement<MapCell> neighbor in neighbors) {
            if (visitedNodes.Exists(node => node.element == neighbor)) {
                continue;
            }

            if (frontierNodes.Exists(node => node.element == neighbor)) {
                continue;
            }

            if (neighbor.Data.type != CellType.WALL) {
                validNeighbors.Add(neighbor);
            }

            if (neighbor.Data.type == CellType.OBJECTIVE) {
                visitedNodes.Add(currentNode);
                visitedNodes.Add(new(neighbor, currentNode));
                solved = true;
                return;
            }
        }


        foreach (GridElement<MapCell> neighbor in validNeighbors) {
            neighbor.Data.SetType(CellType.FRONTIER);
            neighbor.SetValue((int)CalcEuristic(neighbor, objectiveCell));
            frontierNodes.Add(new(neighbor, currentNode));
        }

        visitedNodes.Add(currentNode);

        frontierNodes.Sort((node1, node2) => node1.element.Data.value.CompareTo(node2.element.Data.value));

    }

    public void EndStep() {
        if (solved) {
            List<GridElement<MapCell>> path = new();
            PathFindingNode<GridElement<MapCell>> currentNode = visitedNodes[visitedNodes.Count - 1];
            while (currentNode.parent != null) {
                path.Add(currentNode.element);
                //currentNode.element.Data.SetType(CellType.PATH);
                currentNode = currentNode.parent;
            }

            path.Add(playerCell);

            path.Reverse();

            foreach (GridElement<MapCell> cell in path) {
                if(cell.Data.type != CellType.PLAYER && cell.Data.type != CellType.OBJECTIVE) {
                    cell.Data.SetType(CellType.PATH);
                }
                cell.RefreshDisplay();
                Debug.Log(cell.Data.x + " " + cell.Data.y);
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

    private float CalcEuristic(GridElement<MapCell> origin, GridElement<MapCell> destination) { 
        return Vector2.Distance(new Vector2(origin.Data.x, origin.Data.y), new Vector2(destination.Data.x, destination.Data.y));
    }

    
}
