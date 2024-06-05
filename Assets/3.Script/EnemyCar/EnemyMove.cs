using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private float xLimit = 3.5f;

    private void Update()
    {
        EnemyHorizontal();
    }
    private void EnemyHorizontal()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if(transform.position.x > xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, transform.position.z);
        }
        else if(transform.position.x < -xLimit)
        {
            transform.position = new Vector3(-xLimit, transform.position.y, transform.position.z);
        }
    }
}
