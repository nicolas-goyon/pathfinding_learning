using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouton : MonoBehaviour
{

    public EventHandler OnClick;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void OnMouseDown() {
        OnClick?.Invoke(this, EventArgs.Empty);
    }
}
