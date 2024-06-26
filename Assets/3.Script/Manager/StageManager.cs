using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour       // Z : -1346
{
    [SerializeField] private PoolController poolController;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpawnManager spawnManager;

    //Event
    public event EventHandler onStageUp;

    //Stage ����
    private int stage = 1;

    //Effect
    [SerializeField] private GameObject stageUp;
    private WaitForSeconds activeTime = new WaitForSeconds(2f);


    private void Awake()
    {
        if (poolController != null)
        {
            poolController = GameObject.FindGameObjectWithTag("ObjectPooling").GetComponent<PoolController>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GoalPoint"))
        {
            //Event
            onStageUp?.Invoke(this, EventArgs.Empty);

            ObjectPoolingManager.Instance.isPlayerOnWall = true;
            poolController.isPoolMove = true;
            IncreaseStage();
        }
    }

    private void IncreaseStage()
    {
        stage++;
        Debug.Log("CurrentStage : " + stage);

        stageUp.SetActive(true);
        ScoreManager.Instance.ResetTime();
        StartCoroutine(Wait_Co());
    }

    public void InitRespawn()
    {
        ObjectPoolingManager.Instance.isPlayerOnWall = false;

        if (poolController.spawnManager != null)
        {
            lock (spawnManager) // Lock to prevent conflicts
            {
                spawnManager.currentSpawnPosZ = poolController.reSpawnPosition;
                spawnManager.ResetCarObject();
                poolController.carPoolsParent.position = poolController.startPosition;
            }
        }
        poolController.isPoolMove = false;
    }


    private IEnumerator Wait_Co()
    {
        yield return activeTime;

        stageUp.SetActive(false);
        InitRespawn();
        // playerController.InitRespawn();
        poolController.isPoolMove = false;
    }
}
