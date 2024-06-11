using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance = null;



    [SerializeField] private CarObject[] CarObjectsPrefab;


    public Queue<CarObject> YellowcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> GreencarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> MintcarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> BuscarObjectPool = new Queue<CarObject>();
    public Queue<CarObject> EmptyObjectPool = new Queue<CarObject>();



    private void Awake()
    {
        #region [SingleTone]
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Debug.Log("Instance»ç¶óÁü");
            Destroy(gameObject);
            return;
        }
        #endregion

    }

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            YellowcarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[0], CarObjectsPrefab[0].transform.position, Quaternion.identity));
            GreencarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[1], CarObjectsPrefab[1].transform.position, Quaternion.identity));
            MintcarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[2], CarObjectsPrefab[2].transform.position, Quaternion.identity));
            BuscarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[3], CarObjectsPrefab[3].transform.position, Quaternion.identity));
            EmptyObjectPool.Enqueue(Instantiate(CarObjectsPrefab[4], CarObjectsPrefab[4].transform.position, Quaternion.identity));

        }
    }

    private void CreatAndEnqueueCar(CarObject carObject, Queue<CarObject> pool)
    {
        CarObject newCar = Instantiate(carObject, carObject.transform.position, Quaternion.identity);
        newCar.gameObject.SetActive(false);
        pool.Enqueue(newCar);
    }
}
