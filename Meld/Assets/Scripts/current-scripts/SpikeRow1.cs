using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRow1 : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Go());
    }

    // Update is called once per frame
    IEnumerator Go()
    {
        while (true) {
            yield return new WaitForSeconds(3f);
            anim.speed = 0;
        }
    }
}
