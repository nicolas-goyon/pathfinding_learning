using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicCellDisplayer<T> :  GridElementDisplayer<T> where T : IGridElement { 
    public GameObject Display(GridElement<T> element, Vector3 position, float cellSize) { 

        // Display as a 2D square
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cell.transform.position = position;
        cell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        cell.name = "Cell";

        // make a background for the cell
        GameObject background = GameObject.CreatePrimitive(PrimitiveType.Quad);
        background.transform.position = new Vector3(position.x, position.y, -1);
        background.transform.localScale = new Vector3(cellSize, cellSize, 1);
        background.GetComponent<Renderer>().material.color = Color.white;
        background.name = "Background";

        background.transform.parent = cell.transform;


        // add a smaller square darker than the background to simulate a border
        float borderSize = 0.05f;
        float foregroundSize = cellSize * (1f - 2 * borderSize);


        GameObject foreground = GameObject.CreatePrimitive(PrimitiveType.Quad);
        foreground.transform.position = new Vector3(position.x + borderSize, position.y + borderSize, -2);
        foreground.transform.localScale = new Vector3(foregroundSize, foregroundSize, 1);
        foreground.GetComponent<Renderer>().material.color = getColorGradient(element.Value);
        foreground.name = "Foreground";

        // make the foreground a child of the cell
        foreground.transform.parent = cell.transform;
        
        return cell;

    }



    public Color getColorGradient(int value) {
        // Red above 100 gradient to yellow at 0 then green below 0
        if (value >= 100) {
            return Color.red;
        }
        else if (value > 0) {
            return Color.Lerp(Color.red, Color.yellow, value / 100f);
        }
        else {
            return Color.green;
        }
    }
}
