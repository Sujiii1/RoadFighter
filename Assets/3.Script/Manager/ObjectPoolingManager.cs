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

    private void Awake()
    {
        if(Instance == null)
        {
            Debug.Log("ΩÃ±€≈Ê");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Debug.Log("ªÁ∂Û¡¸");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            YellowcarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[0], CarObjectsPrefab[0].transform.position, Quaternion.identity));
            GreencarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[1], CarObjectsPrefab[1].transform.position, Quaternion.identity));
            MintcarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[2], CarObjectsPrefab[2].transform.position, Quaternion.identity));
            BuscarObjectPool.Enqueue(Instantiate(CarObjectsPrefab[3], CarObjectsPrefab[3].transform.position, Quaternion.identity));
        }
    }
}
