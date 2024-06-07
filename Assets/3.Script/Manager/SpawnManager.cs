using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{


    //1. 오브젝트 랜덤 생성(z축, x축)   하나씩 랜덤
    //2. 랜덤 움직임(x축 랜덤) 
    //3. Item 나올 확률 적게

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

        //생성 위치
        Vector3 spawnPositionZ = enemyPrefebs[enemyIndex].transform.position + Vector3.forward * spawnPosZ;
        spawnPositionZ.x = Random.Range(-spawnRangeX, spawnRangeX);
        spawnPositionZ.y = 6.3f;
        spawnPosZ += spawnDistance;


        Instantiate(enemyPrefebs[enemyIndex], spawnPositionZ, enemyPrefebs[enemyIndex].transform.rotation);
    }
}
