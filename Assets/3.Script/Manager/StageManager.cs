using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour       // Z : -1346
{
    [SerializeField] private PoolController poolController;
    [SerializeField] private PlayerController playerController;
    //Event
    public event EventHandler onStageUp;

    //Stage 저장
    private int stage = 1;

    //Effect
    [SerializeField] private GameObject stageUp;
    private WaitForSeconds activeTime = new WaitForSeconds(2f);


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GoalPoint"))
        {
            onStageUp?.Invoke(this, EventArgs.Empty);
            poolController.isPoolMove = true;
            IncreaseStage();
        }
    }

    private void IncreaseStage()
    {
        stage++;

        stageUp.SetActive(true);
        ScoreManager.Instance.ResetTime();
        StartCoroutine(Wait_Co());
        Debug.Log("CurrentStage : " + stage);
    }

    private IEnumerator Wait_Co()
    {
        yield return activeTime;

        //playerController.InitRespawn(); //poolController 초기화

        stageUp.SetActive(false);
        poolController.isPoolMove = false;
    }
}
