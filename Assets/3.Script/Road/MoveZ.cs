using System;
using System.Collections;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    private PlayerController player;
    private RoadLoop roadRoop;
    [SerializeField] private float speed = 5f;
    private float boundaryZ = -8f;

    private float originalSpeed;


    private void Awake()
    {
        roadRoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        originalSpeed = speed;
    }

    private void OnEnable()
    {
        player.onWall += StartMoveZ;
    }
    private void OnDisable()
    {
        player.onWall -= StartMoveZ;
    }



    private void FixedUpdate()
    {
        if (ScoreManager.Instance.isStartGame)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        ScoreManager.Instance.isStartGame = true;

        transform.Translate(Vector3.back * Time.deltaTime * speed);
        Boundary();
    }

    private void Boundary()
    {
        //ScoreManager.Instance.isStartGame = true;

        if (transform.position.z < boundaryZ)
        {
            Destroy(gameObject);
        }
    }



    //Player Wall¿¡ Ãæµ¹
    private void StartMoveZ(object sender, EventArgs args)
    {
        StartCoroutine(MoveStopCoroutine());
    }

    private IEnumerator MoveStopCoroutine()
    {
        //speed = originalSpeed;

        yield return new WaitForSeconds(1f);

        speed = 0;
    }
}
