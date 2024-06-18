using System;
using System.Collections;
using UnityEngine;
public class GameProgress : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Transform car;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private float totalTime;  // 총 게임 시간
    private float baseSpeed;  // 초기 속도 값
                              // private bool isSpeedReduced = false;


    private void Start()
    {
        totalTime = ScoreManager.Instance.time;

        if (playerController != null)
        {
            playerController.onCollision += StopProcess;
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.onCollision -= StopProcess;
        }
    }


    private void FixedUpdate()
    {
        if (!ScoreManager.Instance.isPauseScore)
        {
            RoadProcess();
        }
    }

    private void CurrentPos(float currentPoint)
    {
        currentPoint = currentPoint * baseSpeed;
    }


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


    private void RoadProcess()
    {
        // 진행도를 계산 (0에서 1 사이의 값)
        float amount = ScoreManager.Instance.time / totalTime;
        StartCoroutine(UpdateCarPositionSmothly(amount));
    }


    /* private void UpdateCarPosition(float progress)
     {
         //시작점과 끝점 사이의 선형 보간을 사용하여 차의 위치를 업데이트
         car.position = Vector3.Lerp(startPoint.position, endPoint.position, progress);
     }*/

    IEnumerator UpdateCarPositionSmothly(float targetProgress)
    {
        Vector3 currentPos = car.position;
        Vector3 targetPos = Vector3.Lerp(startPoint.position, endPoint.position, targetProgress);
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            car.position = Vector3.Lerp(currentPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        car.position = targetPos;

    }
}
