using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintCanvas : MonoBehaviour
{
    public Canvas hintCanvas;   
    
    // Start is called before the first frame update
    private void Awake()
    {
        hintCanvas = this.GetComponent<Canvas>();
        hintCanvas.enabled = false;
    }

    public void DisplayHint()
    {
        hintCanvas.enabled = true;
    }

    public void HideHint()
    {
        hintCanvas.enabled = false;
    }
}
