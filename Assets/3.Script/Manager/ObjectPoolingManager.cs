using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance = null;

    [SerializeField] private CarObject[] CarObjectsPrefab;
    [SerializeField] private CarObject itemPrefab;

    // 자동차 오브젝트와 아이템 오브젝트를 관리하는 큐(Queue) 선언
    public Queue<CarObject> YellowcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> GreencarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> MintcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> BuscarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> ItemObjectPool = new Queue<CarObject>(); // 아이템 풀 추가


    // Dequeue된 후 다시 재사용을 위해 보관하는 큐
    public Queue<CarObject> RemainYellow = new Queue<CarObject>();
    public Queue<CarObject> RemainGreen = new Queue<CarObject>();
    public Queue<CarObject> RemainMint = new Queue<CarObject>();
    public Queue<CarObject> RemainBus = new Queue<CarObject>();
    public Queue<CarObject> RemainItem = new Queue<CarObject>(); // 아이템 남은 목록 추가

    public PoolController poolController;

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
        InitializeCarPools();   // 자동차 오브젝트 풀 초기화
        InitializeItemPool();   // 아이템 오브젝트 풀 초기화
    }


    // 자동차 오브젝트 풀 초기화 메서드
    private void InitializeCarPools()
    {
        if (CarObjectsPrefab == null || CarObjectsPrefab.Length == 0)
        {
            return;
        }

        for (int i = 0; i < CarObjectsPrefab.Length; i++)
        {
            if (CarObjectsPrefab[i] == null)
            {
                continue;
            }

            for (int j = 0; j < 15; j++)
            {
                // 새로운 자동차 오브젝트 생성
                CarObject newCar = Instantiate(CarObjectsPrefab[i], CarObjectsPrefab[i].transform.position, Quaternion.identity, transform);
                newCar.gameObject.SetActive(false);

                // 각 프리팹의 인덱스에 따라 다른 큐에 오브젝트를 추가
                switch (i)
                {
                    case 0:
                        YellowcarObjectPool.Enqueue(newCar);
                        break;
                    case 1:
                        GreencarObjectPool.Enqueue(newCar);
                        break;
                    case 2:
                        MintcarObjectPool.Enqueue(newCar);
                        break;
                    case 3:
                        BuscarObjectPool.Enqueue(newCar);
                        break;


                    default:
                        break;
                }
            }
        }
    }


    // 아이템 오브젝트 풀 초기화 메서드
    private void InitializeItemPool()
    {
        if (itemPrefab == null)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            CarObject newItem = Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity, transform); // 새로운 아이템 오브젝트 생성
            newItem.gameObject.SetActive(false);     // 생성된 오브젝트를 비활성화
            ItemObjectPool.Enqueue(newItem);    // 아이템 풀에 추가
        }
    }
}
