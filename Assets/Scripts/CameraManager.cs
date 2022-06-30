using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 offset, target;
    public GameObject Player;
    public bool isFollow;
    public GameObject[] levels;
    void Start()
    {
        offset = transform.position - Player.transform.position;
    }

    void Update()
    {
        if(isFollow)
        {
            target = Player.transform.position + offset;
            target = new Vector3(-0.23f, target.y, target.z);
            transform.position = Vector3.MoveTowards(transform.position, target, 20 * Time.deltaTime);
            
        }
    }
}
