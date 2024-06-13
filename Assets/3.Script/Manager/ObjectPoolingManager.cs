using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance = null;

    [SerializeField] private CarObject[] CarObjectsPrefab;
    [SerializeField] private CarObject itemPrefab; // �߰��� ������ ������

    public Queue<CarObject> YellowcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> GreencarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> MintcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> BuscarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> EmptyObjectPool = new Queue<CarObject>();
    public Queue<CarObject> ItemObjectPool = new Queue<CarObject>(); // ������ Ǯ �߰�

    public Queue<CarObject> RemainYellow = new Queue<CarObject>();
    public Queue<CarObject> RemainGreen = new Queue<CarObject>();
    public Queue<CarObject> RemainMint = new Queue<CarObject>();
    public Queue<CarObject> RemainBus = new Queue<CarObject>();
    public Queue<CarObject> RemainEmpty = new Queue<CarObject>();
    public Queue<CarObject> RemainItem = new Queue<CarObject>(); // ������ ���� ��� �߰�

    private void Awake()
    {
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
    }

    private void Start()
    {
        InitializeCarPools();
        InitializeItemPool();
    }

    private void InitializeCarPools()
    {
        if (CarObjectsPrefab == null || CarObjectsPrefab.Length == 0)
        {
            Debug.LogError("CarObjectsPrefab is not set or empty!");
            return;
        }

        for (int i = 0; i < CarObjectsPrefab.Length; i++)
        {
            if (CarObjectsPrefab[i] == null)
            {
                Debug.LogError($"CarObjectsPrefab[{i}] is null!");
                continue;
            }

            for (int j = 0; j < 15; j++)
            {
                CarObject newCar = Instantiate(CarObjectsPrefab[i], CarObjectsPrefab[i].transform.position, Quaternion.identity, transform);
                newCar.gameObject.SetActive(false);

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
                    case 4:
                        EmptyObjectPool.Enqueue(newCar);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void InitializeItemPool()
    {
        if (itemPrefab == null)
        {
            return;
        }

        for (int i = 0; i < 5; i++) // ���÷� 5���� �������� �����Ͽ� Ǯ�� �ִ´�.
        {
            CarObject newItem = Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity, transform);
            newItem.gameObject.SetActive(false);
            ItemObjectPool.Enqueue(newItem);
        }
    }

    // �߰������� �ʿ��� �޼������ ���⿡ �߰��� �� �ֽ��ϴ�.
}
