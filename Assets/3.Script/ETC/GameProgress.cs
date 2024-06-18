using System;
using System.Collections;
using UnityEngine;
public class GameProgress : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Transform car;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private float totalTime;  // �� ���� �ð�
    private float baseSpeed;  // �ʱ� �ӵ� ��
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
        // ���൵�� ��� (0���� 1 ������ ��)
        float amount = ScoreManager.Instance.time / totalTime;
        StartCoroutine(UpdateCarPositionSmothly(amount));
    }


    /* private void UpdateCarPosition(float progress)
     {
         //�������� ���� ������ ���� ������ ����Ͽ� ���� ��ġ�� ������Ʈ
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
