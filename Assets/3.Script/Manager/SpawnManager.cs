using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //1. ������Ʈ ���� ����(z��, x��)   �ϳ��� ����
    //2. ���� ������(x�� ����)
    //3. Item ���� Ȯ�� ����
    //4. IsGameOver true ���� �� ���� ����v


    [SerializeField] private float spawnPosZ = 10f;       //Spawn Start Position
    private float spawnRangeX = 3.5f;
    private float spawnDistance = 1f;



    private void Start()
    {
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
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
        if (ScoreManager.Instance != null && ScoreManager.Instance.isStartGame)
        {
            StartCoroutine(SpawnBetween_Co());
        }
    }


    private void Create()
    {
        if (ScoreManager.Instance == null || ScoreManager.Instance.isGameOver)
        {
            return;
        }

        ScoreManager.Instance.isStartGame = true;
        int enemyIndex = Random.Range(0, 5);

        CarObject carObject = null;

        if (enemyIndex.Equals(0))
        {
            carObject = ObjectPoolingManager.Instance.YellowcarObjectPool.Dequeue();
        }
        else if (enemyIndex.Equals(1))
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
        else if (enemyIndex.Equals(4))
        {
            carObject = ObjectPoolingManager.Instance.BuscarObjectPool.Dequeue();
        }


        if (carObject != null)
        {
            //���� ��ġ
            Vector3 spawnPositionZ = carObject.transform.position + Vector3.forward * spawnPosZ;
            spawnPositionZ.x = Random.Range(-spawnRangeX, spawnRangeX);
            spawnPositionZ.y = 6.3f;
            spawnPosZ += spawnDistance;

            carObject.transform.position = spawnPositionZ;

            carObject.gameObject.SetActive(true);
        }

    }

    IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver)   //���� ���� �ƴ� �� Spawn
        {
            Create();
            yield return new WaitForSeconds(1f);
        }
    }
}
