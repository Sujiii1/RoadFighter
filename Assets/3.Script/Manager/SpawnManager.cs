using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //1. 오브젝트 랜덤 생성(z축, x축)   하나씩 랜덤
    //2. 랜덤 움직임(x축 랜덤)
    //3. Item 나올 확률 적게
    //4. IsGameOver true 됐을 때 스폰 멈춤v

    //[SerializeField] private PlayerController playerController;
    public float initialSpawnPosZ = 35f; // 초기 스폰 위치
    public float currentSpawnPosZ;       //Spawn Start Position
    private float spawnRangeX = 3.5f;
    private float spawnDistance = 1f;


    /*    private void OnEnable()
        {
            if (playerController != null)
            {
                playerController.onWall += StopSpawn;
            }

        }
        private void OnDisable()
        {
            if (playerController != null)
            {
                playerController.onWall -= StopSpawn;
            }
        }*/


    private void Start()
    {
        currentSpawnPosZ = initialSpawnPosZ;

        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();


        if (ScoreManager.Instance.isStartGame)
        {
            ResetCarObject();
        }

    }

    public void ResetCarObject()
    {
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



        if (ObjectPoolingManager.Instance.RemainItem.Count > 0)
        {
            int count = ObjectPoolingManager.Instance.RemainItem.Count;
            for (int i = 0; i < count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.ItemObjectPool.Enqueue(carobject);
            }
        }
    }


    public void SpawnStart()
    {
        currentSpawnPosZ = initialSpawnPosZ; // 게임 시작 시 스폰 위치 초기화

        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());
        }
    }

    private void Create()
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

        else if (enemyIndex.Equals(5) && ObjectPoolingManager.Instance.ItemObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.ItemObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainItem.Enqueue(carObject);
        }

        if (carObject != null)
        {
            InitCarObject(carObject);
        }

    }

    public void InitCarObject(CarObject carObject)
    {
        //생성 위치
        //Vector3 spawnPositionZ = carObject.transform.position + Vector3.forward * currentSpawnPosZ;


        Vector3 spawnPositionZ = new Vector3();
        spawnPositionZ.z = currentSpawnPosZ;
        spawnPositionZ.x = UnityEngine.Random.Range(-spawnRangeX, spawnRangeX);
        spawnPositionZ.y = 6.3f;

        carObject.transform.position = spawnPositionZ;
        carObject.gameObject.SetActive(true);

        currentSpawnPosZ += spawnDistance;
    }


    public IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver)   //GameOver 아닐 때 Spawn
        {
            yield return new WaitForSeconds(1f);
            Create();
        }
    }


}