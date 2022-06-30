using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public float countOfColoredCubes;
    private float barProgressValue;
    public GameObject[] levelsProgressBar;
    public GameObject Player;
    public bool isLevelFinished, isLevelFail;
    private bool isOncePlay;
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log("LC: "+ Player.GetComponent<PlayerMove>().levelCount);
        //Debug.Log(countOfColoredCubes);
        if(Player.GetComponent<PlayerMove>().levelCount < Player.GetComponent<PlayerMove>().levels.Length)
        {
            barProgressValue = Mathf.MoveTowards(barProgressValue, countOfColoredCubes / 81f, Time.deltaTime);

            levelsProgressBar[Player.GetComponent<PlayerMove>().levelCount].GetComponent<Image>().fillAmount = barProgressValue;
        }
        if (Player.GetComponent<PlayerMove>().levelCount == 3)
        {
            Debug.Log("WONNNNN");
            transform.GetChild(3).gameObject.SetActive(true);
        }

        //levelsProgressBar[Player.GetComponent<PlayerMove>().levelCount].GetComponent<Image>().fillAmount = countOfColoredCubes /81f ;

        if (isLevelFail)
        {
            if (!isOncePlay)
            {
                transform.GetChild(2).gameObject.SetActive(true);
                //transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>().Play();
                //particle.GetComponent<ParticleSystem>().Play();
                isOncePlay = true;
            }
        }
    }
}
