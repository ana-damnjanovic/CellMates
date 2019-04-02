using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEndScript : MonoBehaviour
{
    private bool toggled = false;
    GameObject mainCamera;
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void OnTriggerEnter()
    {
        if (!toggled)
        {
            mainCamera.GetComponent<MainCameraController>().flipCamera();
            toggled = true;
        }
    }
}
