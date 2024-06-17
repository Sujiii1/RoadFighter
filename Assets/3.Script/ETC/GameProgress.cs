using UnityEngine;

public class GameProgress : MonoBehaviour
{
    [SerializeField] private Transform car;  // 차의 Transform
    [SerializeField] private Transform startPoint;  // 차의 시작점 Transform
    [SerializeField] private Transform endPoint;  // 차의 끝점 Transform

    private float totalTime;  // 총 게임 시간

    private void Start()
    {
        totalTime = ScoreManager.Instance.time;
    }

    void Update()
    {
        RoadProcess();
    }

    private void RoadProcess()
    {
        // 진행도를 계산 (0에서 1 사이의 값)
        float amount = ScoreManager.Instance.time / totalTime;

        UpdateCarPosition(amount);
    }

    private void UpdateCarPosition(float progress)
    {
        // 시작점과 끝점 사이의 선형 보간을 사용하여 차의 위치를 업데이트
        car.position = Vector3.Lerp(startPoint.position, endPoint.position, progress);
    }
}
