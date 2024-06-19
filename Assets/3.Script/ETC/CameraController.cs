using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public Vector3 offset;
    public float cameraSpeed = 10f;
    public float smoothTime = 1f;

    [SerializeField] private bool isCameraMove = false;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        offset = transform.position;

        if (playerController != null)
        {
            playerController.onWall += StartRePosCamera;
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.onWall -= StartRePosCamera;
        }
    }

    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        if (transform.position.z < 12 && isCameraMove)
        {
            //Time.deltaTime은 한 프레임과 프레임 사이의 시간. 고로 아래와 같은 코드가 원하는 만큼 움직이는 것을
            //원한다면 Update와 같은 반복 호출에 포함되어 있어야 함. 이를 하기 위한 방법 작성.
            transform.position += Vector3.forward * cameraSpeed * Time.deltaTime;
        }

    }

    private void StartRePosCamera(object sender, EventArgs args)
    {
        StartCoroutine(WaitAndMoveBack());
    }

    private IEnumerator WaitAndMoveBack()
    {
        isCameraMove = true;


        yield return new WaitForSeconds(3f);

        isCameraMove = false;
        playerController.transform.position = playerController.playerBasePosition;

        // 부드럽게 원래 위치
        while (Vector3.Distance(transform.position, offset) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime);
            yield return null;
        }
        transform.position = offset;
    }
}