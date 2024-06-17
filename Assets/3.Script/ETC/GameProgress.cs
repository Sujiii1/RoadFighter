using UnityEngine;

public class GameProgress : MonoBehaviour
{
    [SerializeField] private Transform car;  // ���� Transform
    [SerializeField] private Transform startPoint;  // ���� ������ Transform
    [SerializeField] private Transform endPoint;  // ���� ���� Transform

    private float totalTime;  // �� ���� �ð�

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
        // ���൵�� ��� (0���� 1 ������ ��)
        float amount = ScoreManager.Instance.time / totalTime;

        UpdateCarPosition(amount);
    }

    private void UpdateCarPosition(float progress)
    {
        // �������� ���� ������ ���� ������ ����Ͽ� ���� ��ġ�� ������Ʈ
        car.position = Vector3.Lerp(startPoint.position, endPoint.position, progress);
    }
}
