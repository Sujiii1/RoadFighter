using System;
using System.Collections;
using UnityEngine;
public class GameProgress : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Transform car;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    //private float targetY = 1850f;
    [SerializeField] private float goalY = 1850f;
    [SerializeField] private float moveSpeed = 37f;



    private void Start()
    {
        if (playerController != null)
        {
            playerController.onCollision += StopProcess;
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.onCollision -= StopProcess;
        }
    }

    private void Update()
    {
        if (!ScoreManager.Instance.isPauseScore)
        {
            RoadProcess();
        }
    }

    private void RoadProcess()
    {
        Vector3 moveDirection = (new Vector3(startPoint.position.x, goalY, startPoint.position.z) - car.position).normalized;
        car.Translate(moveDirection * Time.deltaTime * moveSpeed);

        if (!ScoreManager.Instance.isGameOver)
        {
            // car 도달
            if (car.position.y >= goalY)
            {
                Debug.Log("도달");
                moveSpeed = 0f;
            }
        }
        else
        {
            moveSpeed = 0f;
        }
    }

    //장애물에 부딪혔을 때 멈추는 Event
    private void StopProcess(object sender, EventArgs args)
    {
        StartCoroutine(PauseProcessCoroutine(0.5f));
    }


    private IEnumerator PauseProcessCoroutine(float pauseDuration)
    {
        ScoreManager.Instance.isPauseScore = true;
        yield return new WaitForSeconds(pauseDuration);
        ScoreManager.Instance.isPauseScore = false;
    }
}


