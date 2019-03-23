using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButtonScript : MonoBehaviour
{
    public GameObject mazeDoor;
    public GameObject mazeBall;
    private Light redLight;
    private Light greenLight;
    private AudioSource source;

    void Start()
    {
        mazeDoor = GameObject.Find("MazeDoor");
        mazeBall = GameObject.Find("MazeBall");
        source = gameObject.GetComponent<AudioSource>();
        redLight = gameObject.transform.GetChild(0).GetComponent<Light>();
        greenLight = gameObject.transform.GetChild(1).GetComponent<Light>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == mazeBall)
        {
            mazeBall.transform.position = gameObject.GetComponent<Renderer>().bounds.center + new Vector3(0, mazeBall.transform.localScale.y / 2, 0);
            mazeBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        mazeDoor.SetActive(false);
        redLight.enabled = false;
        greenLight.enabled = true;
        source.Play();
    }

    void OnCollisionExit(Collision collision)
    {
        mazeDoor.SetActive(true);
        redLight.enabled = true;
        greenLight.enabled = false;
    }
}
