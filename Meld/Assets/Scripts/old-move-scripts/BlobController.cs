using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{

    Cloth cloth;
    SphereCollider sphere;
    public GameObject player1;
    public GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        cloth = GetComponent<Cloth>();
        sphere = GetComponent<SphereCollider>();
        // player1 = cloth.sphereColliders[0].first;
        // player2 = cloth.sphereColliders[1].first;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var position1 = player1.transform.position;
        var position2 = player2.transform.position;
        var midpoint = (position1 - position2) / 2;

        // transform.position = midpoint;
        // Updating the position of the cloth to prevent it from breaking off
        if (Vector3.Distance(transform.position, midpoint) < 3)
        {
            cloth.ClearTransformMotion();
            cloth.transform.position = midpoint;
        }
        
        // Setting the 
        // var dir = position1 - position2;
        // transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}
