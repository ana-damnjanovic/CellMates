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
        Vector3 result = Vector3.zero;
        if (X) result += Vector3.right;
        if (Y) result += Vector3.up;
        if (Z) result += Vector3.forward;
        return result;
    }
}

