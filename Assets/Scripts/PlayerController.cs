using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 startPos;
    private bool isOnce, isUp, isRight, isDown;
    public GameObject boxes;
    private int startIndex=4;
    private bool isTurn;
    private bool isOnceUp;
    void Start()
    {
     
    }

    void Update()
    {
        //if(startIndex % 9 == 0)
        //{
        //    //İSLEFT =FALSE;
        //}
        //if (startIndex % 9 == 8)
        //    isRight = false;
        //if (startIndex < 9)
        //    isDown = false;
        //if (startIndex > 71)
        //    isUp = false;



        if(Input.GetMouseButton(0))
        {
            if(!isOnce)
            {
                startPos = Input.mousePosition;
                isOnce = true;
            }
            float yChange = Mathf.Abs(Input.mousePosition.y - startPos.y);
            float xChange = Mathf.Abs(Input.mousePosition.x - startPos.x);
            if(yChange > xChange)
            {
                if (Input.mousePosition.y > startPos.y)// up
                {
                    isUp = true;

                }
                else
                {
                    ClearInputs();
                    isDown = true;

                }
            }
            else
            {
                if (Input.mousePosition.x > startPos.x)
                {
                    if(isTurn)
                    {
                        ClearInputs();
                        isRight = true;
                    }
                }
            }
            
            
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            isOnce = false;
        }
        if (isUp)
        {
            Debug.Log("A");
            if (!isOnceUp)
            {
                startIndex += 9;
                isOnceUp = true;
            }
            isTurn = false;
            transform.position = Vector3.MoveTowards(transform.position, boxes.transform.GetChild(startIndex).position, 5*Time.deltaTime);

            
        }
        if(isRight)
        {
            if (!isOnceUp)
            {
                startIndex += 1;
                isOnceUp = true;
            }
            isTurn = false;
            transform.position = Vector3.MoveTowards(transform.position, boxes.transform.GetChild(startIndex + 1).position, 5 * Time.deltaTime);

        }
        if(isDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, boxes.transform.GetChild(startIndex - 9).position, 5 * Time.deltaTime);

        }
        if (isRight && transform.position == boxes.transform.GetChild(startIndex + 1).position)
        {
            startIndex += 1;
            //startIndex = Mathf.Clamp(startIndex, 4, 76);
            boxes.transform.GetChild(startIndex).GetComponent<MeshRenderer>().enabled = true;
        }
        if (isDown && transform.position == boxes.transform.GetChild(startIndex - 9).position)
        {
            startIndex -= 9;
            //startIndex = Mathf.Clamp(startIndex, 4, 76);
            boxes.transform.GetChild(startIndex).GetComponent<MeshRenderer>().enabled = true;
            Debug.Log(startIndex);
        }
        if (isUp && transform.position == boxes.transform.GetChild(startIndex).position)
        {
            Debug.Log("d");

            isOnceUp = false;
            isTurn = true;
            boxes.transform.GetChild(startIndex).GetComponent<MeshRenderer>().enabled = true;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("playGround"))
        {
            Debug.Log("COLLİSİON");
            ClearInputs();
        }
    }
    private void ClearInputs()
    {
        isUp = false;
        isDown = false;
        isRight = false;
    }
}
