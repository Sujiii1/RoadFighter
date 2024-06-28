using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance = null;

    [SerializeField] private CarObject[] CarObjectsPrefab;
    [SerializeField] private CarObject[] itemPrefab;


    //Pool
    private IObjectPool<CarObject> YellowcarObjectPool;
    private IObjectPool<CarObject> GreencarObjectPool;
    private IObjectPool<CarObject> MintcarObjectPool;
    private IObjectPool<CarObject> BuscarObjectPool;
    private IObjectPool<CarObject> ScoreUpObjectPool;
    private IObjectPool<CarObject> GotObjectPool;


    // Dequeue된 후 다시 재사용을 위해 보관하는 큐
    public Queue<CarObject> RemainYellow = new Queue<CarObject>();
    public Queue<CarObject> RemainGreen = new Queue<CarObject>();
    public Queue<CarObject> RemainMint = new Queue<CarObject>();
    public Queue<CarObject> RemainBus = new Queue<CarObject>();
    public Queue<CarObject> RemainScoreUpItem = new Queue<CarObject>(); // 아이템 남은 목록 추가
    public Queue<CarObject> RemainGetItem = new Queue<CarObject>();     //무적 아이템


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
        InitializePools();
        InitializeItemPool();
    }

    private void InitializePools()
    {
        if (CarObjectsPrefab == null || CarObjectsPrefab.Length == 0)
        {
            return;
        }

        if (CarObjectsPrefab != null && CarObjectsPrefab.Length > 0)
        {
            for (int i = 0; i < Mathf.Min(CarObjectsPrefab.Length, 15); i++)
            {
                switch (i)
                {
                    case 0:
                        YellowcarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;
                    case 1:
                        GreencarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;
                    case 2:
                        MintcarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;
                    case 3:
                        BuscarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;
                    case 4:
                        ScoreUpObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;
                    case 5:
                        GotObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                        break;

                    default:
                        break;
                }
            }



            /*for (int i = 0; i < CarObjectsPrefab.Length; i++)
            {
                if (CarObjectsPrefab[i] == null)
                {
                    continue;
                }

                for (int j = 0; j < 10; j++)
                {
                    CarObject newCar = Instantiate(CarObjectsPrefab[i], CarObjectsPrefab[i].transform.position, Quaternion.identity, transform);
                    newCar.gameObject.SetActive(false);

                    switch (i)
                    {
                        case 0:
                            YellowcarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                            break;
                        case 1:
                            GreencarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                            break;
                        case 2:
                            MintcarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                            break;
                        case 3:
                            BuscarObjectPool = CreateObjectPool(CarObjectsPrefab[i]);
                            break;

                        default:
                            break;

                    }
                }*/
        }

    }

    private void InitializeItemPool()
    {
        if (itemPrefab == null || itemPrefab.Length == 0)
        {
            return;
        }

        if (itemPrefab != null && itemPrefab.Length > 0)
        {
            for (int i = 0; i < Mathf.Min(itemPrefab.Length, 15); i++)
            {
                switch (i)
                {
                    case 0:
                        ScoreUpObjectPool = CreateObjectPool(itemPrefab[i]);
                        break;
                    case 1:
                        GotObjectPool = CreateObjectPool(itemPrefab[i]);
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.Log("itemPrefab null");
        }

        /*  for (int i = 0; i < itemPrefab.Length; i++)
          {
              if (itemPrefab[i] == null)
              {
                  continue;
              }

              for (int j = 0; j < 10; j++)
              {
                  CarObject newItem = Instantiate(itemPrefab[i], itemPrefab[i].transform.position, Quaternion.identity, transform); // 새로운 아이템 오브젝트 생성
                  newItem.gameObject.SetActive(false);     // 생성된 오브젝트를 비활성화

                  // ScoreUpObjectPool.Enqueue(newItem);    // 아이템 풀에 추가

                  switch (i)
                  {
                      case 0:
                          ScoreUpObjectPool = CreateObjectPool(itemPrefab[i]);
                          break;
                      case 1:
                          GotObjectPool = CreateObjectPool(itemPrefab[i]);
                          break;

                      default:
                          break;
                  }
              }
          }*/
    }

    // 오브젝트 풀 생성
    private ObjectPool<CarObject> CreateObjectPool(CarObject prefab)
    {
        return new ObjectPool<CarObject>(
            createFunc: () => Instantiate(prefab, transform),
            actionOnGet: car => car.gameObject.SetActive(true),
            actionOnRelease: car => car.gameObject.SetActive(false),
            collectionCheck: false,
            defaultCapacity: 15,
            maxSize: 100
        );
    }


    //Pool Get/Return
    public CarObject GetyellowCar() => YellowcarObjectPool.Get();  // Pool Get
    public void ReturnYellowCar(CarObject carObject) => YellowcarObjectPool.Release(carObject);  //Pool Return

    public CarObject GetGreenCar() => GreencarObjectPool.Get();
    public void ReturnGreenCar(CarObject carObject) => GreencarObjectPool.Release(carObject);

    public CarObject GetMintCar() => MintcarObjectPool.Get();
    public void ReturnMintCar(CarObject carObject) => MintcarObjectPool.Release(carObject);

    public CarObject GetBusCar() => BuscarObjectPool.Get();
    public void ReturnBusCar(CarObject carObject) => BuscarObjectPool.Release(carObject);

    public CarObject GetScoreItem() => ScoreUpObjectPool.Get();
    public void ReturnScoreItem(CarObject carObject) => ScoreUpObjectPool.Release(carObject);

    public CarObject GetGotItem() => GotObjectPool.Get();
    public void ReturnGotItem(CarObject carObject) => GotObjectPool.Release(carObject);
}
