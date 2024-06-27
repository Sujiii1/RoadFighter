using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private CarObject carObject;
    [SerializeField] private PoolController poolController;
    [SerializeField] private StageManager stageManager;



    public float initialSpawnPosZ = 35f; // �ʱ� ���� ��ġ
    public float currentSpawnPosZ;       //Spawn Start Position
    private float spawnRangeX = 3.5f;
    private float spawnDistance = 1f;   // ������Ʈ �� ���� �Ÿ�


    // ť�� �����ϴ� ���� �浹�� �����ϱ� ���� ����ȭ ��ü
    //lock ���� ����Ͽ� Ư�� �ڵ� ����� �� ���� �ϳ��� �����常 �����ϵ��� ����
    private readonly object lockObject = new object();


    private void Awake()
    {
        ObjectPoolingManager.Instance.poolController.spawnManager = this;
    }

    private void Start()
    {
        currentSpawnPosZ = initialSpawnPosZ;

        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());
        }
    }

    private void OnEnable()
    {
        if (ScoreManager.Instance.isStartGame)
        {
            ResetCarObject();    // ������ ���۵� ���¶�� ������Ʈ�� �ʱ�ȭ
        }

        if (stageManager != null)
        {
            stageManager.onStageUp += IncreaseSpawnDistance;
        }
        else
        {
            Debug.Log("OnEnable stageManager null");
        }
    }

    private void OnDisable()
    {
        if (stageManager != null)
        {
            stageManager.onStageUp -= IncreaseSpawnDistance;
        }
        else
        {
            Debug.Log("OnDisable stageManager null");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        if (ScoreManager.Instance.isStartGame)
        {
            ResetCarObject();    // ������ ���۵� ���¶�� ������Ʈ�� �ʱ�ȭ
        }
    }



    #region   [���� �ʱ�ȭ]

    //���� ���� �� ������ �ʱ�ȭ
    public void SpawnStart()
    {
        currentSpawnPosZ = initialSpawnPosZ;    //���� ��ġ �ʱ�ȭ

        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());  //���� �ڷ�ƾ ����
        }
    }

    // ������Ʈ�� �����ϴ� �޼���
    public void Create()
    {
        if (ScoreManager.Instance == null || ScoreManager.Instance.isGameOver)
        {
            return;
        }

        ScoreManager.Instance.isStartGame = true;
        int enemyIndex = UnityEngine.Random.Range(0, 6);

        CarObject carObject = null;

        if (enemyIndex.Equals(0) && ObjectPoolingManager.Instance.YellowcarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.YellowcarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainYellow.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(1) && ObjectPoolingManager.Instance.GreencarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.GreencarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainGreen.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(2) && ObjectPoolingManager.Instance.MintcarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.MintcarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainMint.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(3) && ObjectPoolingManager.Instance.BuscarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.BuscarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainBus.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(4) && ObjectPoolingManager.Instance.ScoreUpObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.ScoreUpObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainScoreUpItem.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(5) && ObjectPoolingManager.Instance.GotObjectPool.Count > 0)     // + GetItem 
        {
            carObject = ObjectPoolingManager.Instance.GotObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainGetItem.Enqueue(carObject);
        }

        if (carObject != null)
        {
            InitCarObject(carObject);
        }
        else
        {
            Debug.Log("carObject is null");
        }
    }


    public void ResetCarObject()
    {
        /*        lock (lockObject)
                {

                }*/

        // Debug.Log("Entered ResetCarObject lock");

        if (ObjectPoolingManager.Instance.RemainYellow.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainYellow.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainYellow.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.YellowcarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainGreen.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainGreen.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainGreen.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.GreencarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainMint.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainMint.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainMint.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.MintcarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainBus.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainBus.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainBus.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);

                }
                ObjectPoolingManager.Instance.BuscarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainScoreUpItem.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainScoreUpItem.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainScoreUpItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.ScoreUpObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainGetItem.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainGetItem.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainGetItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.GotObjectPool.Enqueue(carobject);
            }
        }
    }

    #endregion



    // ������Ʈ�� Ư�� ��ġ�� ����
    public void InitCarObject(CarObject carObject)
    {
        Vector3 spawnPositionZ = new Vector3
        {
            z = currentSpawnPosZ,
            x = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            y = 6.3f
        };

        carObject.transform.position = spawnPositionZ;  // ������Ʈ ��ġ ����
        carObject.gameObject.SetActive(true);            // ������Ʈ Ȱ��ȭ

        currentSpawnPosZ += spawnDistance;              // ���� ���� ��ġ ����
                                                        // Debug.Log("carObject :" + carObject.transform.position.z);
    }


    //spawnDistance 20% ����
    private void IncreaseSpawnDistance(object sender, EventArgs args)
    {
        spawnDistance *= 1.2f;
        Debug.Log("New spawnDistance: " + spawnDistance);
    }


    // ������Ʈ�� ���� �ð� �������� ����
    public IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver && ScoreManager.Instance.isStartGame)   //GameOver �ƴ� �� Spawn
        {
            yield return new WaitForSeconds(1f);
            Create();
        }
    }
}
