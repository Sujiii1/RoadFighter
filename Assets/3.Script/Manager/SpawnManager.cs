using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{


    //1. ������Ʈ ���� ����(z��, x��)   �ϳ��� ����
    //2. ���� ������(x�� ����)
    //3. Item ���� Ȯ�� ����

    [SerializeField] private RoadLoop roadLoop;

    //[SerializeField] private GameObject[] enemyPrefebs;
    private float spawnRangeX = 3.5f;
    private float spawnPosZ = 6f;       //Spawn Start Position
    private float spawnDistance = 0.5f;

    private void Awake()
    {
        roadLoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
    }

    private void Start()
    {
        if(roadLoop.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SpawnStart()
    {
        StartCoroutine(SpawnBetween_Co());
    }

    IEnumerator SpawnBetween_Co()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(1f);
        }
        
    }

    private void Create()
    {
        roadLoop.isStartGame = true;
        int enemyIndex = Random.Range(0, 4);
        //Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 6.3f, Random.Range(2, 12));

        CarObject carObject = null;


        if(enemyIndex.Equals(0))
        {
            carObject = ObjectPoolingManager.Instance.YellowcarObjectPool.Dequeue();
        }
        else if(enemyIndex.Equals(1))
        {
            carObject = ObjectPoolingManager.Instance.GreencarObjectPool.Dequeue();
        }
        else if (enemyIndex.Equals(2))
        {
            carObject = ObjectPoolingManager.Instance.MintcarObjectPool.Dequeue();
        }
        else if (enemyIndex.Equals(3))
        {
            carObject = ObjectPoolingManager.Instance.BuscarObjectPool.Dequeue();
        }



        //���� ��ġ
        Vector3 spawnPositionZ = carObject.transform.position + Vector3.forward * spawnPosZ;
        spawnPositionZ.x = Random.Range(-spawnRangeX, spawnRangeX);
        spawnPositionZ.y = 6.3f;
        spawnPosZ += spawnDistance;

        carObject.transform.position = spawnPositionZ;

        carObject.gameObject.SetActive(true);


/*        Instantiate(enemyPrefebs[enemyIndex], spawnPositionZ, enemyPrefebs[enemyIndex].transform.rotation);*/
    }
}
