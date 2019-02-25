using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance;
    public Vector3 lastCheckpointPositionp1;
    public Vector3 lastCheckpointPositionp2;
    public Vector3 lastCheckpointPositionmem;
    public Vector3 lastCheckpointPositionmemsphere;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else {
            Destroy(gameObject);
        }
    }

}
