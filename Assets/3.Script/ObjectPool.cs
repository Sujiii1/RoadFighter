using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance = null;

    //참조
    public PoolController poolController;

    //Bool
    public bool isPlayerOnWall = false;
    public bool isPoolInitialized = false;     // 풀 초기화 완료 플래그


    //Pool
    private Dictionary<CarType, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    [System.Serializable]
    public class Pool
    {
        // public GameObject prefab;
        public AssetReference prefabReference;
        public int size;
        public CarType carType;
    }

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

        //poolDictionary 초기화
        poolDictionary = new Dictionary<CarType, Queue<GameObject>>();

    }

    private void Start()
    {
        StartCoroutine(InitPools());
    }


    //풀 초기화
    /*   private void InitSpawn()
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
       }*/




    //Addressable 적용
    //로드된 프리팹을 관리



    private IEnumerator InitPools()
    {
        var coroutines = new List<Coroutine>();

        foreach (Pool pool in pools)
        {
            coroutines.Add(StartCoroutine(LoadPool(pool)));
        }

        foreach (var co in coroutines)
        {
            yield return co;
        }
        isPoolInitialized = true;
    }

    private IEnumerator LoadPool(Pool pool)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            var handle = pool.prefabReference.InstantiateAsync();
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject obj = handle.Result;
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);

                CarObject carObject = obj.GetComponent<CarObject>();
                if (carObject != null)
                {
                    carObject.SetCarType(pool.carType);
                }
                objectPool.Enqueue(obj);
            }
            else
            {
                Debug.LogError("load  Addressables prefab load Fail");
            }
        }

        if (!poolDictionary.ContainsKey(pool.carType))
        {
            poolDictionary.Add(pool.carType, objectPool);
        }
    }

    //Addressable에 pool 
    public void AddToPool(GameObject obj, CarType carType)
    {
        if (obj == null)
        {
            return;
        }

        obj.SetActive(false);
        if (!poolDictionary.ContainsKey(carType))
        {
            poolDictionary[carType] = new Queue<GameObject>();
        }
        poolDictionary[carType].Enqueue(obj);
    }




    //풀에서 오브젝트 스폰
    public GameObject SpawnFromPool(CarType carType, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(carType) || poolDictionary[carType].Count == 0 || !isPoolInitialized)
        {
            Debug.LogWarning($"No pool with car type: {carType}");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[carType].Dequeue();
        if (objectToSpawn == null)
        {
            Debug.LogWarning($"Object to spawn is null for car type: {carType}");
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[carType].Enqueue(objectToSpawn);
        Debug.Log($"Spawned object of type {carType} at {position}");

        return objectToSpawn;
    }


    //오브젝트 return
    public void EnqueueObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }



        obj.SetActive(false);
        CarObject carObject = obj.GetComponent<CarObject>();

        if (carObject != null)
        {
            CarType key = carObject.CarType;
            if (poolDictionary.ContainsKey(key))
            {
                poolDictionary[key].Enqueue(obj);
                Debug.Log($"Returned object of type {key} to pool");

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
                if (obj != null && obj.activeSelf)
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


