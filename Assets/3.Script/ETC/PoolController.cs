using System;
using System.Collections;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnManager spawnManager;
    private Transform carPoolsParent;
    private Vector3 startPosition;

    [SerializeField] private float poolSpeed = 60f;
    [SerializeField] private bool isPoolMove;

    private void Awake()
    {
        //spawnManager = GameObject.FindGameObjectWithTag("Road").GetComponent<SpawnManager>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnManager = FindObjectOfType<SpawnManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (ObjectPoolingManager.Instance == null)
        {
            return;
        }
        carPoolsParent = ObjectPoolingManager.Instance.transform;
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.onWall += StartRePosPool;
        }
        else
        {
            Debug.LogError("playerController is not assigned");
        }
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.onWall -= StartRePosPool;
        }
    }

    private void Start()
    {


        if (carPoolsParent != null)
        {
            startPosition = carPoolsParent.position;
        }
    }

    private void Update()
    {
        MoveCarPools();
    }


    public void MoveCarPools()
    {
        if (carPoolsParent != null && carPoolsParent.position.z < 30 && isPoolMove)
        {
            carPoolsParent.position += Vector3.forward * poolSpeed * Time.deltaTime;
        }
    }

    private void StartRePosPool(object sender, EventArgs args)
    {
        StartCoroutine(MoveCarPoolsCoroutine());
    }

    private IEnumerator MoveCarPoolsCoroutine()
    {
        isPoolMove = true;

        yield return new WaitForSeconds(3f);

        isPoolMove = false;
        carPoolsParent.position = startPosition;
        //다시 돌아오지 말고 

        //초기화
        if (spawnManager != null)
        {
            spawnManager.ResetCarObject();


        }
    }




    //CarObject carObject = GetCarObject();
    //spawnManager.InitCarObject(carObject);
    //private CarObject GetCarObject()
    //{
    //    return new CarObject();
    //}

    /* private void ResetAllPooledObjects()
     {
         ResetCarObjects(ObjectPoolingManager.Instance.RemainYellow, ObjectPoolingManager.Instance.YellowcarObjectPool);
         ResetCarObjects(ObjectPoolingManager.Instance.RemainGreen, ObjectPoolingManager.Instance.GreencarObjectPool);
         ResetCarObjects(ObjectPoolingManager.Instance.RemainMint, ObjectPoolingManager.Instance.MintcarObjectPool);
         ResetCarObjects(ObjectPoolingManager.Instance.RemainBus, ObjectPoolingManager.Instance.BuscarObjectPool);
         ResetCarObjects(ObjectPoolingManager.Instance.RemainItem, ObjectPoolingManager.Instance.ItemObjectPool);
     }

     private void ResetCarObjects(Queue<CarObject> activeQueue, Queue<CarObject> poolQueue)
     {
         int count = activeQueue.Count;
         for (int i = 0; i < count; i++)
         {
             CarObject carObject = activeQueue.Dequeue();
             if (carObject != null)
             {
                 carObject.gameObject.SetActive(false);
                 poolQueue.Enqueue(carObject);
             }
         }
     }*/

}
