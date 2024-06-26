using System;
using System.Collections;
using UnityEngine;


public class GameProgress : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private StageManager stageManager;

    [SerializeField] private Transform car;
    [SerializeField] private Transform startPoint;
    // [SerializeField] private Transform endPoint;

    //private float targetY = 1850f;
    [SerializeField] private float goalY = 1950f;
    [SerializeField] private float moveSpeed = 37f;


    private void Start()
    {
        if (playerController != null)
        {
            playerController.onCollision += StopProcess;
        }

        if (stageManager != null)
        {
            stageManager.onStageUp += InitCarPosition;
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
        if (!ScoreManager.Instance.isPauseScore && ScoreManager.Instance.isStartGame)
        {
            RoadProcess();
        }
    }


    private void InitCarPosition(object sender, EventArgs args)
    {
        car.position = startPoint.position;
        car.rotation = startPoint.rotation;
    }

    private void RoadProcess()
    {
        Vector3 moveDirection = (new Vector3(startPoint.position.x, goalY, startPoint.position.z) - car.position).normalized;
        car.Translate(moveDirection * Time.deltaTime * moveSpeed);

        if (!ScoreManager.Instance.isGameOver)
        {
            // car µµ´Þ
            if (car.position.y >= goalY)
            {
                moveSpeed = 0f;
            }
        }
        else //timeOver µÆÀ» ¶§
        {
            moveSpeed = 0f;
        }
    }

    //Àå¾Ö¹°¿¡ ºÎµúÇûÀ» ¶§ ¸ØÃß´Â Event
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


