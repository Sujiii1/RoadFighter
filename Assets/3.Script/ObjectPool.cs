using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance = null;

    //Pool
    private Dictionary<CarType, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
        public CarType carType;
    }


    public PoolController poolController;
    public bool isPlayerOnWall = false;






    private void Awake()
    {
        #region  [SingleTone]
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

    }

    private void Start()
    {
        InitSpawn();
    }




    //풀 초기화
    private void InitSpawn()
    {
        poolDictionary = new Dictionary<CarType, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(this.transform); // 부모를 설정
                obj.SetActive(false);


                CarObject carObject = obj.GetComponent<CarObject>();
                if (carObject != null)
                {
                    carObject.SetCarType(pool.carType);
                }
                objectPool.Enqueue(obj);

            }
            if (!poolDictionary.ContainsKey(pool.carType))
            {
                poolDictionary.Add(pool.carType, objectPool);

            }

        }
    }

    //풀에서 오브젝트 스폰
    public GameObject SpawnFromPool(CarType carType, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(carType))
        {
            return null;
        }

        GameObject objectToSpawn = poolDictionary[carType].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[carType].Enqueue(objectToSpawn);

        return objectToSpawn;
    }


    //오브젝트 return
    public void EnqueueObject(GameObject obj)
    {
        obj.SetActive(false);
        CarObject carObject = obj.GetComponent<CarObject>();

        if (carObject != null)
        {
            CarType key = carObject.CarType;
            if (poolDictionary.ContainsKey(key))
            {
                poolDictionary[key].Enqueue(obj);
            }
            else
            {
                Debug.LogWarning("No pool car type: " + key);
            }
        }
        else
        {
            Debug.LogWarning("carObject is Null");
        }
    }


    // 모든 오브젝트 초기화
    public void ResetAllCarObject()
    {
        foreach (var key in poolDictionary.Keys)
        {
            Queue<GameObject> queue = poolDictionary[key];
            int count = queue.Count;


            for (int i = 0; i < count; i++)
            {
                GameObject obj = queue.Dequeue();

                // GameObject가 활성화되어 있는지 확인
                if (obj.activeSelf)
                {
                    // GameObject 비활성화 처리
                    obj.SetActive(false);
                }

                // 비활성화 처리한 오브젝트를 다시 풀에 넣기
                queue.Enqueue(obj);

            }
        }
    }
}


