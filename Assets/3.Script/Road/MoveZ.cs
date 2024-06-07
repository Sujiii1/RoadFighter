using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZ : MonoBehaviour
{
    private RoadLoop roadRoop;
    [SerializeField] private float speed = 5f;
    private float boundaryZ =-8f;

    private void Awake()
    {
        roadRoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
    }

    private void Update()
    {
        if (roadRoop.isStartGame)
        {
            MoveObject();
            Boundary();
        }

    }

    private void MoveObject()
    {
        roadRoop.isStartGame = true;
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }

    private void Boundary()
    {
        roadRoop.isStartGame = true;
        if (transform.position.z < boundaryZ)
        {
            Destroy(gameObject);
        }
    }
}
