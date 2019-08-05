using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionSlider : MonoBehaviour
{
    public Slider tensionSlider;
    // Start is called before the first frame update
    private void Awake()
    {
        tensionSlider = this.GetComponent<Slider>();
    }

    public void SetTension(float playerDistance)
    {
        tensionSlider.value = playerDistance;
    }
}
