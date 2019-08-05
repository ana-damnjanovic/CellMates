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
    
    public static string player1Tag = "Player1";
    public static string p1HorizontalInput = "HorizontalP1";
    public static string p1VerticalInput = "VerticalP1";
    public static string p1StickButton = "StickP1";
    public static string p1JumpButton = "JumpP1";

    public static string player2Tag = "Player2";
    public static string p2HorizontalInput = "HorizontalP2";
    public static string p2VerticalInput = "VerticalP2";
    public static string p2StickButton = "StickP2";
    public static string p2JumpButton = "JumpP2";

    void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
    }
}
