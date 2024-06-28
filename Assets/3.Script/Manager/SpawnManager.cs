using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private CarObject carObject;
    [SerializeField] private PoolController poolController;
    [SerializeField] private StageManager stageManager;



    public float initialSpawnPosZ = 35f; // 초기 스폰 위치
    public float currentSpawnPosZ;       //Spawn Start Position
    private float spawnRangeX = 3.5f;
    private float spawnDistance = 1f;   // 오브젝트 간 스폰 거리


    // 큐에 접근하는 동안 충돌을 방지하기 위한 동기화 객체
    //lock 문을 사용하여 특정 코드 블록을 한 번에 하나의 스레드만 실행하도록 보장
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
            ResetCarObject();    // 게임이 시작된 상태라면 오브젝트를 초기화
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
            ResetCarObject();    // 게임이 시작된 상태라면 오브젝트를 초기화
        }
    }






    // 오브젝트를 생성하는 메서드
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










    #region   [생성 초기화]

    //게임 시작 시 스폰을 초기화
    public void SpawnStart()
    {
        currentSpawnPosZ = initialSpawnPosZ;    //스폰 위치 초기화

        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());  //스폰 코루틴 시작
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



    // 오브젝트를 특정 위치에 생성
    public void InitCarObject(CarObject carObject)
    {
        Vector3 spawnPositionZ = new Vector3
        {
            z = currentSpawnPosZ,
            x = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            y = 6.3f
        };

        carObject.transform.position = spawnPositionZ;  // 오브젝트 위치 설정
        carObject.gameObject.SetActive(true);            // 오브젝트 활성화

        currentSpawnPosZ += spawnDistance;              // 다음 스폰 위치 설정
                                                        // Debug.Log("carObject :" + carObject.transform.position.z);
    }


    //spawnDistance 20% 감소
    private void IncreaseSpawnDistance(object sender, EventArgs args)
    {
        if (spawnDistance > 0)
        {
            spawnDistance *= 0.8f;
            Debug.Log("New spawnDistance: " + spawnDistance);
        }
    }


    // 오브젝트를 일정 시간 간격으로 생성
    public IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver && ScoreManager.Instance.isStartGame)   //GameOver 아닐 때 Spawn
        {
            yield return new WaitForSeconds(1f);
            Create();
        }
    }
}
