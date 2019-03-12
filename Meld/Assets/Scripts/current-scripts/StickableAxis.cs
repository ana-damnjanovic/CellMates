using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickableAxis : MonoBehaviour
{
    // the axis we want to boost in the move script
    public bool X;
    public bool Y;
    public bool Z;

    public Vector3 GetStickableAxis()
    {
        if (X) return Vector3.right;
        if (Y) return Vector3.up;
        if (Z) return Vector3.forward;
        else return Vector3.zero;
    }
}
