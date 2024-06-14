using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    public Vector3 offet;
    public float cameraSpeed = 5f;


    private void Start()
    {
        offet = transform.position;
    }

    private void Update()
    {
        //  StartCoroutine(RePos_Co());
    }

    private void CameraMove()
    {
        if (ScoreManager.Instance.isStartGame && player.isWall && transform.position.z < 12)
        {
            //Debug.Log($"isStartGame: {ScoreManager.Instance.isStartGame}, isTimeOver: {player.isTimeOver}, Camera Z Position: {transform.position.z}");
            transform.position += Vector3.forward * cameraSpeed * Time.deltaTime;
        }
    }

    public IEnumerator RePos_Co()
    {
        CameraMove();
        yield return new WaitForSeconds(3f);
        transform.position = offet;
    }
}