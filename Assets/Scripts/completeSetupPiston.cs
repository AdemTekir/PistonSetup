using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class completeSetupPiston : MonoBehaviour
{
    public GameObject setActiveCanvas;
    public int checkForCompleteSetup = 0;
    public bool checkForCloseUI = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (checkForCloseUI)
        {
            if (checkForCompleteSetup == 9)
            {
                CompleteUI();
            }
            else
            {
                setActiveCanvas.SetActive(false);
            }
        }
    }

    void CompleteUI()
    {
        if (checkForCompleteSetup == 9)
        {
            setActiveCanvas.SetActive(true);
        }
    }

    public void CloseUI()
    {
        checkForCloseUI = false;
        setActiveCanvas.SetActive(false);
    }
}
