using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Cloth>().ClearTransformMotion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
