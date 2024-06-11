using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    private RoadLoop roadRoop;
    private PlayerController player;
    [SerializeField] private float speed = 5f;
    private float boundaryZ =-8f;

    private void Awake()
    {
        roadRoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent < PlayerController>();
    }

    private void Update()
    {
        if (ScoreManager.Instance.isStartGame)
        {
            MoveObject();
            Boundary();

        }
    }

    private void MoveObject()
    {
        ScoreManager.Instance.isStartGame = true;
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    private void Boundary()
    {
        ScoreManager.Instance.isStartGame = true;
        if (transform.position.z < boundaryZ)
        {
            Destroy(gameObject);
        }
    }
}
