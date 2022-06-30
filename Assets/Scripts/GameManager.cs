using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isLevelFinished, isLevelFail;
    public GameObject Player;
    public GameObject particle;
    private bool isOncePlay;
    
    void Start()
    {
       
    }

    void Update()
    {
    }
    public void replayGame()
    {
        SceneManager.LoadScene(0);
    }

}
