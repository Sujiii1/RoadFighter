using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance = null;

    [SerializeField] private CarObject[] CarObjectsPrefab;
    [SerializeField] private CarObject itemPrefab;

    // �ڵ��� ������Ʈ�� ������ ������Ʈ�� �����ϴ� ť(Queue) ����
    public Queue<CarObject> YellowcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> GreencarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> MintcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> BuscarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> ItemObjectPool = new Queue<CarObject>(); // ������ Ǯ �߰�


    // Dequeue�� �� �ٽ� ������ ���� �����ϴ� ť
    public Queue<CarObject> RemainYellow = new Queue<CarObject>();
    public Queue<CarObject> RemainGreen = new Queue<CarObject>();
    public Queue<CarObject> RemainMint = new Queue<CarObject>();
    public Queue<CarObject> RemainBus = new Queue<CarObject>();
    public Queue<CarObject> RemainItem = new Queue<CarObject>(); // ������ ���� ��� �߰�

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
        InitializeCarPools();   // �ڵ��� ������Ʈ Ǯ �ʱ�ȭ
        InitializeItemPool();   // ������ ������Ʈ Ǯ �ʱ�ȭ
    }


    // �ڵ��� ������Ʈ Ǯ �ʱ�ȭ �޼���
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
                // ���ο� �ڵ��� ������Ʈ ����
                CarObject newCar = Instantiate(CarObjectsPrefab[i], CarObjectsPrefab[i].transform.position, Quaternion.identity, transform);
                newCar.gameObject.SetActive(false);

                // �� �������� �ε����� ���� �ٸ� ť�� ������Ʈ�� �߰�
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


    // ������ ������Ʈ Ǯ �ʱ�ȭ �޼���
    private void InitializeItemPool()
    {
        if (itemPrefab == null)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            CarObject newItem = Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity, transform); // ���ο� ������ ������Ʈ ����
            newItem.gameObject.SetActive(false);     // ������ ������Ʈ�� ��Ȱ��ȭ
            ItemObjectPool.Enqueue(newItem);    // ������ Ǯ�� �߰�
        }
    }
}
