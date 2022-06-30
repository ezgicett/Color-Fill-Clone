using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public int levelNum;// from inspector
    public bool isColored;
    public GameObject gameManager;
    public GameObject canvasManager;
    public GameObject Player;
    private int count;
    private bool isOnce;
 
    void Update()
    {
        if(Player.GetComponent<PlayerMove>().levelCount == levelNum )
        {
            if (Player.GetComponent<PlayerMove>().myDirection == PlayerMove.Direction.Stabile) //!gameManager.GetComponent<GameManager>().isLevelFinished && 
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<MeshRenderer>().enabled == true)
                    {
                        count++;
                    }
                }
                canvasManager.GetComponent<CanvasManager>().countOfColoredCubes = count;
            }


            if (count == 81 && !isOnce)
            {
                isOnce = true;
                StartCoroutine(WaitForFinish());
               
            }
            else
                count = 0;
        }
        
    }
    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(1f);
        Player.GetComponent<PlayerMove>().isLevelFinished = true;
        gameManager.GetComponent<GameManager>().isLevelFinished = true;
        GetComponent<BoxController>().enabled = false;
        count = 0;
    }
}
