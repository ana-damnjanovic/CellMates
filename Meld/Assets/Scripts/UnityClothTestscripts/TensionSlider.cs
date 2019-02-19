using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionSlider : MonoBehaviour
{
    public Slider TensionSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        TensionSlider = this;
    }

    public void SetTension(float playerDistance)
    {
        TensionSlider.value = playerDistance;
    }
}
