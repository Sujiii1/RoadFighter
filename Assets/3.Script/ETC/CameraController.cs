using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public Vector3 offet;
    public float cameraSpeed = 5f;

    //4. ����׸� ���� SerializeField�� ���ݴϴ�.
    [SerializeField] private bool isCameraMove = false;


    private void Start()
    {
        offet = transform.position;
    }

    private void Update()
    {
        //1. ���� ������Ʈ�� �������ݴϴ�.
        CameraMove();
        //2. Update�ȿ� ī�޶� ���긦 �������ݴϴ�.
    }

    private void CameraMove()
    {
        //3. ���ǿ� Bool �� �ϳ��� �� �߰����ݴϴ�.
        if (transform.position.z < 12 && isCameraMove)
        {
            Debug.LogError("CameraMove : ����");
            //Debug.Log($"isStartGame: {ScoreManager.Instance.isStartGame}, isTimeOver: {player.isTimeOver}, Camera Z Position: {transform.position.z}");

            //Time.deltaTime�� �� �����Ӱ� ������ ������ �ð��� ������ �˰� �ֽ��ϴ�. ��� �Ʒ��� ���� �ڵ尡 ���ϴ� ��ŭ �����̴� ���� ���Ѵٸ� Update�� ���� �ݺ� ȣ�⿡ ���ԵǾ� �־�� �մϴ�. �̸� �ϱ� ���� ����� ���� �ۼ��غ����� �ϰڽ��ϴ�.
            transform.position += Vector3.back * cameraSpeed * Time.deltaTime;
        }
    }

    public IEnumerator RePos_Co()
    {
        Debug.LogError("RePos_Co : ����");
        //CameraMove(); <- �����ݴϴ�.
        isCameraMove = true;
        // ī�޶� ���� Ʈ�縦 �����ݴϴ�.
        yield return new WaitForSeconds(3f);

        // ī�޶� ���� ������ �����ݴϴ�.
        isCameraMove = false;
        transform.position = offet;
    }
}