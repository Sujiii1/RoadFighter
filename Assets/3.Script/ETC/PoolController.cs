using System;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnManager spawnManager;

    public Transform carPoolsParent;       // 풀링된 오브젝트의 부모 Transform
    public Vector3 startPosition;
    public float reSpawnPosition = 20f;     // 재스폰 위치

    [SerializeField] private float poolSpeed = 60f;  // 오브젝트 이동 속도
    public bool isPoolMove = false;

    private void Awake()
    {
        if (ObjectPoolingManager.Instance == null)
        {
            return;
        }

        // carPoolsParent를 ObjectPoolingManager의 Transform으로 설정
        carPoolsParent = ObjectPoolingManager.Instance.transform;

        // 현재 위치를 startPosition으로 설정
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.onWall += StartRePosPool;
        }


        // 게임이 시작되었으면 spawnManager의 ResetCarObject 메서드를 호출
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

        // 게임이 시작되었으면 spawnManager의 ResetCarObject 메서드를 호출
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            spawnManager.ResetCarObject();
        }
    }

    private void Start()
    {
        // carPoolsParent와 playerController가 존재하면 startPosition을 설정
        if (carPoolsParent != null && playerController != null)
        {
            startPosition = carPoolsParent.position;
        }

    }

    private void Update()
    {
        MoveCarPools();
    }


    // 오브젝트 이동
    public void MoveCarPools()
    {
        if (carPoolsParent != null)
        {
            if (carPoolsParent.position.z < 35 && isPoolMove)
            {
                carPoolsParent.position += Vector3.forward * poolSpeed * Time.deltaTime;
            }
        }
    }



    // onWall 이벤트 발생 시 호출되는 메서드
    private void StartRePosPool(object sender, EventArgs args)
    {
        //StartCoroutine(MoveCarPoolsCoroutine());
        isPoolMove = true;
    }


    /*    // 오브젝트를 재배치하는 코루틴
        private IEnumerator MoveCarPoolsCoroutine()
        {
            isPoolMove = true;

            yield return new WaitForSeconds(3f);
            Debug.Log("MoveCarPoolsCoroutine after WaitForSeconds");

            isPoolMove = false;                         // 오브젝트 이동 중지
            carPoolsParent.position = startPosition;    // carPoolsParent 위치 초기화

            if (spawnManager != null)
            {
                Debug.Log("MoveCarPoolsCoroutine Attempting to enter ResetCarObject lock");
                lock (spawnManager)
                {
                    Debug.Log("MoveCarPoolsCoroutine Entered ResetCarObject lock");
                    spawnManager.currentSpawnPosZ = reSpawnPosition;  // 초기 위치 재설정
                    spawnManager.ResetCarObject();  // 오브젝트 초기화
                }
            }
            else
            {
                Debug.Log("MoveCarPoolsCoroutine Exited ResetCarObject lock");
                Debug.Log("spawnManager null");

            }
            Debug.Log("MoveCarPoolsCoroutine finished");
        }*/
}
