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
            //Time.deltaTime�� �� �����Ӱ� ������ ������ �ð�. ��� �Ʒ��� ���� �ڵ尡 ���ϴ� ��ŭ �����̴� ����
            //���Ѵٸ� Update�� ���� �ݺ� ȣ�⿡ ���ԵǾ� �־�� ��. �̸� �ϱ� ���� ��� �ۼ�.
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

        // �ε巴�� ���� ��ġ
        while (Vector3.Distance(transform.position, offset) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime);
            yield return null;
        }
        transform.position = offset;
    }
}