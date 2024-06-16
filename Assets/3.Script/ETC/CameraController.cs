using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public Vector3 offet;
    public float cameraSpeed = 5f;

    //4. 디버그를 위한 SerializeField를 해줍니다.
    [SerializeField] private bool isCameraMove = false;


    private void Start()
    {
        offet = transform.position;
    }

    private void Update()
    {
        //1. 먼저 업데이트를 선언해줍니다.
        CameraMove();
        //2. Update안에 카메라 무브를 선언해줍니다.
    }

    private void CameraMove()
    {
        //3. 조건에 Bool 값 하나만 더 추가해줍니다.
        if (transform.position.z < 12 && isCameraMove)
        {
            Debug.LogError("CameraMove : 실행");
            //Debug.Log($"isStartGame: {ScoreManager.Instance.isStartGame}, isTimeOver: {player.isTimeOver}, Camera Z Position: {transform.position.z}");

            //Time.deltaTime은 한 프레임과 프레임 사이의 시간인 것으로 알고 있습니다. 고로 아래와 같은 코드가 원하는 만큼 움직이는 것을 원한다면 Update와 같은 반복 호출에 포함되어 있어야 합니다. 이를 하기 위한 방법을 지금 작성해보도록 하겠습니다.
            transform.position += Vector3.back * cameraSpeed * Time.deltaTime;
        }
    }

    public IEnumerator RePos_Co()
    {
        Debug.LogError("RePos_Co : 실행");
        //CameraMove(); <- 지워줍니다.
        isCameraMove = true;
        // 카메라 무브 트루를 시켜줍니다.
        yield return new WaitForSeconds(3f);

        // 카메라 무브 폴스를 시켜줍니다.
        isCameraMove = false;
        transform.position = offet;
    }
}