using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public CarObject carObject;
    [SerializeField] private PoolController poolController;
    public float initialSpawnPosZ = 35f; // �ʱ� ���� ��ġ
    public float currentSpawnPosZ;       //Spawn Start Position
    private float spawnRangeX = 3.5f;
    private float spawnDistance = 1f;   // ������Ʈ �� ���� �Ÿ�


    //  // ť�� �����ϴ� ���� �浹�� �����ϱ� ���� ����ȭ ��ü
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
        lock (lockObject)
        {
            // Debug.Log("Entered Create lock");

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
            else if (enemyIndex.Equals(5) && ObjectPoolingManager.Instance.GotObjectPool.Count > 0)     // + GotItem 
            {
                carObject = ObjectPoolingManager.Instance.GotObjectPool.Dequeue();
                ObjectPoolingManager.Instance.RemainGotItem.Enqueue(carObject);
            }

            if (carObject != null)
            {
                InitCarObject(carObject);
            }
        }
        //Debug.Log("Exited Create lock");
    }


    public void ResetCarObject()
    {

        //Debug.Log("Attempting to enter ResetCarObject lock");
        lock (lockObject)
        {
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

            if (ObjectPoolingManager.Instance.RemainGotItem.Count > 0)
            {
                int count = ObjectPoolingManager.Instance.RemainGotItem.Count;
                for (int i = 0; i < count; i++)
                {
                    CarObject carobject = ObjectPoolingManager.Instance.RemainGotItem.Dequeue();
                    if (carobject != null)
                    {
                        carobject.gameObject.SetActive(false);
                    }
                    ObjectPoolingManager.Instance.GotObjectPool.Enqueue(carobject);
                }
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

        carObject.transform.position = spawnPositionZ;  // ������Ʈ ��ġ ����s
        carObject.gameObject.SetActive(true);            // ������Ʈ Ȱ��ȭ

        currentSpawnPosZ += spawnDistance;              // ���� ���� ��ġ ����
    }


    // ������Ʈ�� ���� �ð� �������� ����
    public IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver)   //GameOver �ƴ� �� Spawn
        {
            yield return new WaitForSeconds(1f);
            Create();
        }
    }
}
