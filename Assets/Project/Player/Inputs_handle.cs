using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs_handle : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 InputVector { get; private set; }

    public Vector3 MousePos { get; private set; }



    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var Vertical = Input.GetAxis("Vertical");
        InputVector = new Vector2(horizontal, Vertical);

        MousePos = Input.mousePosition;


    }
}
