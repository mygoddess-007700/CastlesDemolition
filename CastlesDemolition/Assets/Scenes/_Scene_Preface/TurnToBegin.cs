using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnToBegin : MonoBehaviour
{
    public float countTime = 0f;
    void Update()
    {
        CountTime();

    }

    void CountTime()
    {
        countTime += Time.deltaTime;
        if (countTime > 5.0f)
        {
            SceneManager.LoadScene("_Scene_Begin");
        }
    }
}
