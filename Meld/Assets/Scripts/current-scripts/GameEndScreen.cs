using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour
{

    AsyncOperation gameEndScene;

    void Start()
    {
        StartCoroutine("LoadGameEnd");
    }

    IEnumerator LoadLab()
    {
        labScene = SceneManager.LoadSceneAsync("Lab");
        labScene.allowSceneActivation = false;
        yield return null;
    }

    void Update()
    {
        GameObject.FindWithTag("UI").transform.Find("Panel").gameObject.transform.Find("P1_Z").gameObject.GetComponent<Image>().enabled = !Input.GetButton("JumpP1");
        GameObject.FindWithTag("UI").transform.Find("Panel").gameObject.transform.Find("P1_Z_Pressed").gameObject.GetComponent<Image>().enabled = Input.GetButton("JumpP1");
        GameObject.FindWithTag("UI").transform.Find("Panel").gameObject.transform.Find("P2_Z").gameObject.GetComponent<Image>().enabled = !Input.GetButton("JumpP2");
        GameObject.FindWithTag("UI").transform.Find("Panel").gameObject.transform.Find("P2_Z_Pressed").gameObject.GetComponent<Image>().enabled = Input.GetButton("JumpP2");

        if (Input.GetButton("JumpP1") && Input.GetButton("JumpP2"))
        {
            labScene.allowSceneActivation = true;
        }
    }
}

