using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;


public class PlayerMove : MonoBehaviour
{
    private Vector3 startPos;
    private bool isOnce, isUp, isRight, isDown, isLeft;
    public GameObject gameManager, canvas;
    private int startIndexY = 4, startIndexX=8;
    private bool isTurn, isMoveFinish, isStabile, isLevelFail;
    private GameObject[,] array = new GameObject[9, 9];
    public List<int[]> locationsOfPlayer = new List<int[]>();
    public enum Direction { North, East, South, West, Stabile };
    public Direction myDirection;
    public bool isLevelFinished;
    public GameObject[] levels;
    private Sequence seq;
    public int levelCount;
    private bool isOnceFill, isOnceParticle, isOnceVibrate;

    void Start()
    {
        locationsOfPlayer.Add( new int[] {startIndexX,startIndexY });
        assignBoxes();
        isTurn = true;
    }

    void Update()
    {
        if(isLevelFinished)
        {
            levelCount++;
            if(levelCount < levels.Length)
            {
                seq = DOTween.Sequence();
                seq.Append(transform.DOMove(array[startIndexX, 4].transform.position, 1f));
                seq.Append(transform.DOMove(levels[levelCount].transform.GetChild(levels[levelCount].transform.childCount - 5).transform.position, 0.5f).OnComplete(controlCameraMove));
                Camera.main.GetComponent<CameraManager>().isFollow = true;

                assignBoxes();
                // clear and change to start values
                startIndexX = 8;
                startIndexY = 4;
                locationsOfPlayer.Clear();
                ClearInputs();
                isLevelFinished = false;
            }
        }
        else if(isLevelFail)
        {
            if(!isOnceVibrate)// once vibrating at fail statu
            {
                succesVib();
                isOnceVibrate = true;
            }
            StartCoroutine(WaitForFail());

            
        }
        else
        {
            if (Input.GetMouseButton(0)) // taking inputs and defining directions
            {
                if (!isOnce)
                {
                    startPos = Input.mousePosition;
                    isOnce = true;
                }
                float yChange = Mathf.Abs(Input.mousePosition.y - startPos.y);
                float xChange = Mathf.Abs(Input.mousePosition.x - startPos.x);
                if (yChange > xChange)
                {
                    isOnceFill = false;
                    if (Input.mousePosition.y > startPos.y)// up
                    {
                        if (isTurn && myDirection != Direction.South)
                        {
                            isUp = true;
                            myDirection = Direction.North;
                        }
                    }
                    else
                    {
                        if (isTurn && myDirection != Direction.North)
                        {
                            ClearInputs();
                            isDown = true;
                            myDirection = Direction.South;
                        }
                    }
                }
                else
                {
                    isOnceFill = false;
                    if (Input.mousePosition.x > startPos.x)
                    {
                        if (isTurn && myDirection != Direction.West)
                        {
                            ClearInputs();
                            isRight = true;
                            myDirection = Direction.East;

                        }
                    }
                    else if (Input.mousePosition.x < startPos.x)
                    {
                        if (isTurn && myDirection != Direction.East)
                        {
                            ClearInputs();
                            isLeft = true;
                            myDirection = Direction.West;

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
                if (!isMoveFinish)
                {
                    startIndexX -= 1;
                    startIndexX = Mathf.Clamp(startIndexX, 0, 8);
                    if (startIndexX == 0)
                        isStabile = true;

                    if (!isStabile)
                    {
                        for (int i = 0; i < locationsOfPlayer.Count; i++)
                        {
                            if (locationsOfPlayer[i][0] == startIndexX)
                            {
                                if (locationsOfPlayer[i][1] == startIndexY)
                                {
                                    isLevelFail = true;

                                }
                            }
                        }
                    }

                    isMoveFinish = true;
                }
                if (array[startIndexX, startIndexY].tag == "edge")
                {
                    myDirection = Direction.Stabile;
                    startIndexX += 1;
                }
                if(array[startIndexX, startIndexY].tag == "wall")
                {
                    myDirection = Direction.Stabile;
                    startIndexX += 1;
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, array[startIndexX, startIndexY].transform.position, 5 * Time.deltaTime);
            }

            else if (isRight)
            {
                isTurn = false;
                if (!isMoveFinish)
                {
                    startIndexY += 1;
                    startIndexY = Mathf.Clamp(startIndexY, 0, 8);
                    if (startIndexY == 8)
                        isStabile = true;

                    if (!isStabile) // if player crash on it's tail
                    {
                        for (int i = 0; i < locationsOfPlayer.Count; i++)
                        {
                            if (locationsOfPlayer[i][0] == startIndexX)
                            {
                                if (locationsOfPlayer[i][1] == startIndexY)
                                {
                                    isLevelFail = true;
                                }
                            }
                        }
                    }

                    isMoveFinish = true;
                }
                if (array[startIndexX, startIndexY].tag == "edge") // second level obstacles
                {
                    myDirection = Direction.Stabile;
                    startIndexY -= 1;
                }
                if (array[startIndexX, startIndexY].tag == "wall") // third level obstacles
                {
                    myDirection = Direction.Stabile;
                    startIndexY -= 1;
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, array[startIndexX, startIndexY].transform.position, 5 * Time.deltaTime);
            }
            else if (isLeft)
            {
                isTurn = false;
                if (!isMoveFinish)
                {
                    startIndexY -= 1;
                    startIndexY = Mathf.Clamp(startIndexY, 0, 8);
                    if (startIndexY == 0)
                        isStabile = true;

                    if (!isStabile) // if player crash on it's tail
                    {
                        for (int i = 0; i < locationsOfPlayer.Count; i++)
                        {
                            if (locationsOfPlayer[i][0] == startIndexX)
                            {
                                if (locationsOfPlayer[i][1] == startIndexY)
                                {
                                    isLevelFail = true;
                                }
                            }
                        }
                    }
                    isMoveFinish = true;
                }
                if (array[startIndexX, startIndexY].tag == "edge")
                {
                    myDirection = Direction.Stabile;
                    startIndexY += 1;
                }
                if (array[startIndexX, startIndexY].tag == "wall")
                {
                    myDirection = Direction.Stabile;
                    startIndexY += 1;
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, array[startIndexX, startIndexY].transform.position, 5 * Time.deltaTime);
            }

            else if (isDown)
            {
                isTurn = false;
                if (!isMoveFinish)
                {
                    startIndexX += 1;
                    startIndexX = Mathf.Clamp(startIndexX, 0, 8);
                    if (startIndexX == 8)
                        isStabile = true;

                    if (!isStabile)
                    {
                        for (int i = 0; i < locationsOfPlayer.Count; i++)
                        {
                            if (locationsOfPlayer[i][0] == startIndexX)
                            {
                                if (locationsOfPlayer[i][1] == startIndexY)
                                {
                                    Debug.Log("FAIL");
                                    isLevelFail = true;

                                }
                            }
                        }
                    }

                    isMoveFinish = true;
                }
                if (array[startIndexX, startIndexY].tag == "edge")
                {
                    startIndexX -= 1;
                    myDirection = Direction.Stabile;
                }
                if (array[startIndexX, startIndexY].tag == "wall")
                {
                    startIndexX -= 1;
                    myDirection = Direction.Stabile;
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, array[startIndexX, startIndexY].transform.position, 5 * Time.deltaTime);

            }
           
            if (transform.position == array[startIndexX, startIndexY].transform.position) // if player come to the wall
            {
                isTurn = true;
                isMoveFinish = false;
                if (isStabile) // next to the wall it has no direction sign, so it is stabile
                {
                    myDirection = Direction.Stabile;
                    isStabile = false;
                }
                locationsOfPlayer.Add(new int[] { startIndexX, startIndexY });
                array[startIndexX, startIndexY].GetComponent<MeshRenderer>().enabled = true; 
            }
            if (myDirection == Direction.Stabile && !isOnceFill)// if player touch the wall, fills the gaps
            {
                for (int i = 0; i <= locationsOfPlayer.Count - 1; i++)
                {
                    for (int j = locationsOfPlayer[i][0]; j < 9; j++)
                    {
                        array[j, locationsOfPlayer[i][1]].GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                locationsOfPlayer.Clear();
                isOnceFill = true;
            }
        }
       
    }
    public void controlCameraMove()
    {
        Camera.main.GetComponent<CameraManager>().isFollow = false;

    }
    private void ClearInputs()
    {
        isUp = false;
        isDown = false;
        isRight = false;
        isLeft = false;
    }

    private IEnumerator WaitForFail()
    {
        for (int i = 0; i < locationsOfPlayer.Count; i++)
        {
            array[locationsOfPlayer[i][0], locationsOfPlayer[i][1]].GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);

        }
        if(!isOnceParticle)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            isOnceParticle = true;
            yield return new WaitForSeconds(1f);
        }
        canvas.GetComponent<CanvasManager>().isLevelFail = true;
        
    }
    public void succesVib()
    {
        MMVibrationManager.Haptic(HapticTypes.Success);
    }
    private void assignBoxes()
    {
        int countOfBox = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                array[i, j] = levels[levelCount].transform.GetChild(countOfBox).gameObject;
                countOfBox++;
            }
        }
    }
}
