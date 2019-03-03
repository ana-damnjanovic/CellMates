using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static float maxSeparation = 2.85f;
    public static float speed = 1;
    public static float topSpeed = 1.5f;
    public static float rayCastDistance = 0.75f;
    public static float maxSpringDistance = 1.5f;
    public static float jumpMagnitude = 60;
    public static float stickingJumpMagnitude = 300;
    public static float playerMass = 0.2f;
    public static float stickingPlayerMass = 1;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
    }
}
