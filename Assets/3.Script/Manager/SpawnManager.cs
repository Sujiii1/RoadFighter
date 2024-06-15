using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    //1. ¿ÀºêÁ§Æ® ·£´ý »ý¼º(zÃà, xÃà)   ÇÏ³ª¾¿ ·£´ý
    //2. ·£´ý ¿òÁ÷ÀÓ(xÃà ·£´ý)
    //3. Item ³ª¿Ã È®·ü Àû°Ô
    //4. IsGameOver true µÆÀ» ¶§ ½ºÆù ¸ØÃãv

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
        ResetCarObject();
    }

    private void ResetCarObject()
    {
        if (ObjectPoolingManager.Instance.RemainYellow.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainYellow.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainYellow.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.YellowcarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainGreen.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainGreen.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainGreen.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.GreencarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainMint.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainMint.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainMint.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.MintcarObjectPool.Enqueue(carobject);
            }

        }

        if (ObjectPoolingManager.Instance.RemainBus.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainBus.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainBus.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.BuscarObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainEmpty.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainEmpty.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainEmpty.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.EmptyObjectPool.Enqueue(carobject);
            }
        }

        if (ObjectPoolingManager.Instance.RemainItem.Count > 0)
        {
            for (int i = 0; i < ObjectPoolingManager.Instance.RemainItem.Count; i++)
            {
                CarObject carobject = ObjectPoolingManager.Instance.RemainItem.Dequeue();
                if (carobject != null)
                {
                    carobject.gameObject.SetActive(false);
                }
                ObjectPoolingManager.Instance.ItemObjectPool.Enqueue(carobject);
            }
        }
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
        int enemyIndex = Random.Range(0, 6);

        CarObject carObject = null;

        if (enemyIndex.Equals(0) && ObjectPoolingManager.Instance.YellowcarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.YellowcarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainYellow.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(1) && ObjectPoolingManager.Instance.GreencarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.GreencarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainGreen.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(2) && ObjectPoolingManager.Instance.MintcarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.MintcarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainMint.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(3) && ObjectPoolingManager.Instance.BuscarObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.BuscarObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainBus.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(4) && ObjectPoolingManager.Instance.EmptyObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.EmptyObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainEmpty.Enqueue(carObject);
        }
        else if (enemyIndex.Equals(5) && ObjectPoolingManager.Instance.ItemObjectPool.Count > 0)
        {
            carObject = ObjectPoolingManager.Instance.ItemObjectPool.Dequeue();
            ObjectPoolingManager.Instance.RemainItem.Enqueue(carObject);
        }

        if (carObject != null)
        {
            InitCarObject(carObject);
        }

    }


    private void InitCarObject(CarObject carObject)
    {
        //»ý¼º À§Ä¡
        Vector3 spawnPositionZ = carObject.transform.position + Vector3.forward * spawnPosZ;
        spawnPositionZ.x = Random.Range(-spawnRangeX, spawnRangeX);
        spawnPositionZ.y = 6.3f;
        spawnPosZ += spawnDistance;

        carObject.transform.position = spawnPositionZ;
        carObject.gameObject.SetActive(true);
    }

    IEnumerator SpawnBetween_Co()
    {
        while (ScoreManager.Instance != null && !ScoreManager.Instance.isGameOver)   //GameOver ¾Æ´Ò ¶§ Spawn
        {
            yield return new WaitForSeconds(1f);
            Create();
            
        }
    }

}