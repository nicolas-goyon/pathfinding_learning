using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCellDisplay<T> : GridElementDisplayer<T> where T : MapCell {
    public GameObject Display(GridElement<T> element, Vector3 position, float cellSize) {

        // Display as a 2D square
        GameObject cell = new("Cell");
        cell.transform.position = position;
        cell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        _ = CreateSquare(Vector3.zero, 1, new Color(0.5f, 0.5f, 0.5f), "Background", cell);



        // add a smaller square darker than the background to simulate a border
        float borderSize = 0.05f;
        float foregroundSize = (1f - 2 * borderSize);
        _ = CreateSquare(new Vector3(borderSize, borderSize, -2), foregroundSize, GetColor(element.Data), "Foreground", cell);


        // Add a text to display the value of the cell
        if (element.Data.type == CellType.FLOOR || element.Data.type == CellType.OBJECTIVE || element.Data.type == CellType.PLAYER) { 
            GameObject text = new("Text");
            text.transform.parent = cell.transform;
            text.transform.localPosition = new Vector3(0, 0, -1);
            TextMesh textMesh = text.AddComponent<TextMesh>();
            textMesh.text = (element.Data.value == int.MaxValue ? "inf" : element.Data.value.ToString());
            textMesh.characterSize = 1f;
            textMesh.fontSize = 50;

            textMesh.color = Color.black;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
        }
        return cell;

    }

    private Color GetColorGradient(int value) {
        Color inifity = Color.white;
        Color max = Color.red;
        Color min = Color.yellow;
        if (value == int.MaxValue) {
            return inifity;
        }

        return Color.Lerp(min, max, (float)value / 100);
    }


    private Color GetColor(MapCell cell) {
        switch (cell.type) {
            case CellType.WALL:
                return Color.black;
            case CellType.PLAYER:
                return Color.green;
            case CellType.OBJECTIVE:
                return Color.red;
            case CellType.PATH:
                return Color.blue;
            case CellType.VISITED:
                return Color.magenta;
            case CellType.FRONTIER:
                return Color.gray;
            case CellType.SELECTED:
                return Color.cyan;
            case CellType.FLOOR:
                return Color.white;
            default:
                return Color.cyan;
        }
        
    }


    public GameObject CreateSquare(Vector3 position, float cellSize, Color color, string name, GameObject parent) { 
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cell.transform.parent = parent.transform;
        cell.transform.localPosition = position;
        cell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        cell.GetComponent<Renderer>().material.color = color;
        cell.name = name;
        return cell;
    }
}
