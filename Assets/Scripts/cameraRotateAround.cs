using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class cameraRotateAround : MonoBehaviour
{
    public GameObject targetRotateAround;

    public bool checkForRotateAround;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(targetRotateAround.transform.position, transform.up, 180);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(targetRotateAround.transform.position, -transform.up, 180);
        }
    }
}

