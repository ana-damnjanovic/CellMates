using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerModelController : MonoBehaviour
{

    public Camera camera;
    public Quaternion startingRotation;


    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var cameraRotation = Quaternion.LookRotation(camera.transform.position - transform.position);
        transform.rotation = cameraRotation;
    }
}
