using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEndScript : MonoBehaviour
{
    GameObject mainCamera;
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void OnTriggerEnter()
    {
        mainCamera.GetComponent<MainCameraController>().flipRotation = true;
    }
}
