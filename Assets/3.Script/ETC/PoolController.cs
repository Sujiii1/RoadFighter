using System;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public PlayerController playerController;
    public SpawnManager spawnManager;

    public Transform carPoolsParent;       // Ǯ���� ������Ʈ�� �θ� Transform
    public Vector3 startPosition;
    public float reSpawnPosition = 20f;     // �罺�� ��ġ

    [SerializeField] private float poolSpeed = 60f;  // ������Ʈ �̵� �ӵ�
    public bool isPoolMove = false;

    private void Awake()
    {
        if (ObjectPoolingManager.Instance == null)
        {
            return;
        }

        // carPoolsParent�� ObjectPoolingManager�� Transform���� ����
        carPoolsParent = ObjectPoolingManager.Instance.transform;

        // ���� ��ġ�� startPosition���� ����
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.onWall += StartRePosPool;
        }


        // ������ ���۵Ǿ����� spawnManager�� ResetCarObject �޼��带 ȣ��
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

        // ������ ���۵Ǿ����� spawnManager�� ResetCarObject �޼��带 ȣ��
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            spawnManager.ResetCarObject();
        }
    }

    private void Start()
    {
        // carPoolsParent�� playerController�� �����ϸ� startPosition�� ����
        if (carPoolsParent != null && playerController != null)
        {
            startPosition = carPoolsParent.position;
        }

    }

    private void Update()
    {
        MoveCarPools();
    }


    // ������Ʈ �̵�
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



    // onWall �̺�Ʈ �߻� �� ȣ��Ǵ� �޼���
    private void StartRePosPool(object sender, EventArgs args)
    {
        //StartCoroutine(MoveCarPoolsCoroutine());
        isPoolMove = true;
    }


    /*    // ������Ʈ�� ���ġ�ϴ� �ڷ�ƾ
        private IEnumerator MoveCarPoolsCoroutine()
        {
            isPoolMove = true;

            yield return new WaitForSeconds(3f);
            Debug.Log("MoveCarPoolsCoroutine after WaitForSeconds");

            isPoolMove = false;                         // ������Ʈ �̵� ����
            carPoolsParent.position = startPosition;    // carPoolsParent ��ġ �ʱ�ȭ

            if (spawnManager != null)
            {
                Debug.Log("MoveCarPoolsCoroutine Attempting to enter ResetCarObject lock");
                lock (spawnManager)
                {
                    Debug.Log("MoveCarPoolsCoroutine Entered ResetCarObject lock");
                    spawnManager.currentSpawnPosZ = reSpawnPosition;  // �ʱ� ��ġ �缳��
                    spawnManager.ResetCarObject();  // ������Ʈ �ʱ�ȭ
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
