using System;
using System.Collections;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnManager spawnManager;

    private Transform carPoolsParent;
    private Vector3 startPosition;
    private float reSpawnPosition = 20f;

    [SerializeField] private float poolSpeed = 60f;
    [SerializeField] private bool isPoolMove = false;



    private void Awake()
    {
        isPoolMove = false;

        if (ObjectPoolingManager.Instance == null)
        {
            return;
        }
        carPoolsParent = ObjectPoolingManager.Instance.transform;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.onWall -= StartRePosPool;
        }
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            spawnManager.ResetCarObject();
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.onWall -= StartRePosPool;
        }
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            spawnManager.ResetCarObject();
        }
    }



    private void Start()
    {
        if (carPoolsParent != null && playerController != null)
        {
            startPosition = carPoolsParent.position;
        }
        if (playerController != null)
        {
            playerController.onWall += StartRePosPool;
        }
    }

    private void Update()
    {
        MoveCarPools();
    }


    public void MoveCarPools()
    {
        if (carPoolsParent != null && carPoolsParent.position.z < 30 && isPoolMove)
        {
            carPoolsParent.position += Vector3.forward * poolSpeed * Time.deltaTime;
        }
    }

    private void StartRePosPool(object sender, EventArgs args)
    {
        StartCoroutine(MoveCarPoolsCoroutine());
    }

    private IEnumerator MoveCarPoolsCoroutine()
    {
        isPoolMove = true;

        yield return new WaitForSeconds(3f);

        isPoolMove = false;
        carPoolsParent.position = startPosition;

        //초기화
        if (spawnManager != null)
        {
            spawnManager.currentSpawnPosZ = reSpawnPosition;        //초기 위치 재설정
            spawnManager.ResetCarObject();
        }
    }
}
