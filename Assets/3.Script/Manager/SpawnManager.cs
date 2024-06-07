using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{


    //1. ������Ʈ ���� ����(z��, x��)   �ϳ��� ����
    //2. ���� ������(x�� ����) 
    //3. Item ���� Ȯ�� ����

    private RoadLoop roadLoop;

    [SerializeField] private GameObject[] enemyPrefebs;
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

    IEnumerator SpawnBetween_Co()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(0.3f);
        }
        
    }

    private void Create()
    {
        roadLoop.isStartGame = true;
        int enemyIndex = Random.Range(0, enemyPrefebs.Length);
        //Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 6.3f, Random.Range(2, 12));

        //���� ��ġ
        Vector3 spawnPositionZ = enemyPrefebs[enemyIndex].transform.position + Vector3.forward * spawnPosZ;
        spawnPositionZ.x = Random.Range(-spawnRangeX, spawnRangeX);
        spawnPositionZ.y = 6.3f;
        spawnPosZ += spawnDistance;


        Instantiate(enemyPrefebs[enemyIndex], spawnPositionZ, enemyPrefebs[enemyIndex].transform.rotation);
    }
}
