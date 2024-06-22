using System;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    private PlayerController playerController;
    private RoadLoop roadRoop;
    public float speed = 5f;
    private float boundaryZ = -8f;


    public bool isZeroSpeed = false;

    private void Awake()
    {
        roadRoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    private void OnEnable()
    {
/*        if(playerController == null)
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.onWall += SpeedZero;*/
    }
    private void OnDisable()
    {
/*        if (playerController == null)
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.onWall -= SpeedZero;*/
    }



    private void FixedUpdate()
    {
        if (ScoreManager.Instance.isStartGame && !isZeroSpeed && !ObjectPoolingManager.Instance.isPlayerOnWall)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        ScoreManager.Instance.isStartGame = true;

        if (!isZeroSpeed)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
            //Boundary();
        }

    }

/*    private void Boundary()
    {
        if (transform.position.z < boundaryZ)
        {

        }
    }*/

    private void SpeedZero(object sender, EventArgs args)
    {
        //isZeroSpeed = true;
        if (isZeroSpeed)
        {
            speed = 0;
        }
    }
}
