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
        if (playerController != null)
        {
            playerController.onWall += SpeedZero;
        }
    }
    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.onWall -= SpeedZero;
        }
    }



    private void FixedUpdate()
    {
        if (ScoreManager.Instance.isStartGame && !isZeroSpeed)
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
            Boundary();
        }

    }

    private void Boundary()
    {
        //ScoreManager.Instance.isStartGame = true;

        if (transform.position.z < boundaryZ)
        {
            Destroy(gameObject);
        }
    }

    private void SpeedZero(object sender, EventArgs args)
    {
        isZeroSpeed = true;
        speed = 0;
    }


    /* //Player Wall¿¡ Ãæµ¹
     private void StartMoveZ(object sender, EventArgs args)
     {
         //StartCoroutine(MoveStopCoroutine());

         //Debug.Log("StartMoveZ");
     }*/

    /*private IEnumerator MoveStopCoroutine()
    {
        //speed = originalSpeed;

        yield return new WaitForSeconds(1f);
        speed = 0;
    }*/
}
