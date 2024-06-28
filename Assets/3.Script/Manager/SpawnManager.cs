using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private CarObject carObject;
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
        ObjectPool.Instance.poolController.spawnManager = this;

        if (poolController != null)
        {
            poolController = GameObject.FindGameObjectWithTag("ObjectPooling").GetComponent<PoolController>();
        }
        else
        {
            Debug.Log("Awake poolController null");
        }
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
        /*        else
                {
                    Debug.Log("OnEnable stageManager null");
                }*/
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

        /* if (enemyIndex.Equals(0) && ObjectPoolingManager.Instance.YellowcarObjectPool.Count > 0)
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
         }*/

        switch (enemyIndex)
        {
            case 0:
                carObject = ObjectPool.Instance.GetyellowCar();
                ObjectPool.Instance.RemainYellow.Enqueue(carObject);
                break;
            case 1:
                carObject = ObjectPool.Instance.GetGreenCar();
                ObjectPool.Instance.RemainGreen.Enqueue(carObject);
                break;
            case 2:
                carObject = ObjectPool.Instance.GetMintCar();
                ObjectPool.Instance.RemainMint.Enqueue(carObject);
                break;
            case 3:
                carObject = ObjectPool.Instance.GetBusCar();
                ObjectPool.Instance.RemainBus.Enqueue(carObject);
                break;
            case 4:
                carObject = ObjectPool.Instance.GetScoreItem();
                ObjectPool.Instance.RemainScoreUpItem.Enqueue(carObject);
                break;
            case 5:
                carObject = ObjectPool.Instance.GetGotItem();
                ObjectPool.Instance.RemainGetItem.Enqueue(carObject);
                break;
            default:
                break;

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


    public void ResetCarObject()
    {
        if (ObjectPool.Instance.RemainYellow.Count > 0)
        {
            int count = ObjectPool.Instance.RemainYellow.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainYellow.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPool.Instance.ReturnYellowCar(carobject);
            }
        }

        if (ObjectPool.Instance.RemainGreen.Count > 0)
        {
            int count = ObjectPool.Instance.RemainGreen.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainGreen.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPool.Instance.ReturnGreenCar(carobject);
            }
        }

        if (ObjectPool.Instance.RemainMint.Count > 0)
        {
            int count = ObjectPool.Instance.RemainMint.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainMint.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPool.Instance.ReturnMintCar(carobject);
            }
        }

        if (ObjectPool.Instance.RemainBus.Count > 0)
        {
            int count = ObjectPool.Instance.RemainBus.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainBus.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);

                }
                ObjectPool.Instance.ReturnBusCar(carobject);
            }
        }

        if (ObjectPool.Instance.RemainScoreUpItem.Count > 0)
        {
            int count = ObjectPool.Instance.RemainScoreUpItem.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainScoreUpItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPool.Instance.ReturnScoreItem(carobject);
            }
        }

        if (ObjectPool.Instance.RemainGetItem.Count > 0)
        {
            int count = ObjectPool.Instance.RemainGetItem.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPool.Instance.RemainGetItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPool.Instance.ReturnGotItem(carobject);
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
        if (spawnDistance > 0)
        {
            spawnDistance *= 0.8f;
            Debug.Log("New spawnDistance: " + spawnDistance);
        }
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
