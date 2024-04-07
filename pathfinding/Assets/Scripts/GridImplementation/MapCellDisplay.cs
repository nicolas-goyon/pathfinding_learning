using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCellDisplay<T> : GridElementDisplayer<T> where T : MapCell {

    private readonly GameObject group = new("Map");
    private readonly float scale;

    public MapCellDisplay(float scale) {
        this.scale = scale;
    }

    public GameObject Display(GridElement<T> element, Vector3 position, float cellSize) {

        // Display as a 2D square
        GameObject cell = new("Cell");
        cell.transform.parent = group.transform;
        cell.transform.localPosition = position;
        Vector3 localScale = new(cellSize, cellSize, 1);
        cell.transform.localScale = localScale;
        Color grey = new(0.5f, 0.5f, 0.5f);
        _ = CreateSquare(Vector3.zero, 1, grey, "Background", cell);



        // add a smaller square darker than the background to simulate a border
        float borderSize = 0.05f;
        float foregroundSize = (1f - 2 * borderSize);
        Vector3 localPosition = new(borderSize, borderSize, -2);
        _ = CreateSquare(localPosition, foregroundSize, GetColor(element.Data), "Foreground", cell);


        if (element.Data.BaseType == CellType.FLOOR ) { 
            GameObject text = new("Text");
            text.transform.parent = cell.transform;
            text.transform.localPosition = new(0, 0, -1);
            TextMesh textMesh = text.AddComponent<TextMesh>();
            float point1Round = Mathf.Floor(element.Data.BaseValue * 10) / 10;
            textMesh.text = (element.Data.BaseValue == float.MaxValue ? "inf" : point1Round.ToString());
            textMesh.characterSize = 1f;
            float baseFontSize = 50f;
            textMesh.fontSize = (int) (scale * baseFontSize);

            textMesh.color = Color.black;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
        }
        return cell;

    }

    private Color GetColor(MapCell cell) {
        return cell.Type switch {
            CellType.WALL => Color.black,
            CellType.PLAYER => Color.green,
            CellType.OBJECTIVE => Color.red,
            CellType.PATH => Color.blue,
            CellType.VISITED => Color.magenta,
            CellType.FRONTIER => Color.gray,
            CellType.SELECTED => Color.cyan,
            CellType.FLOOR => Color.white,
            _ => Color.cyan,
        };
    }


    public GameObject CreateSquare(Vector3 position, float cellSize, Color color, string name, GameObject parent) { 
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cell.transform.parent = parent.transform;
        cell.transform.localPosition = position;
        cell.transform.localScale = new(cellSize, cellSize, 1);
        cell.GetComponent<Renderer>().material.color = color;
        cell.name = name;
        return cell;
    }
}
