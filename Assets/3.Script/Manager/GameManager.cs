using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

/*    //public RoadLoop roadLoop;

*//*    private void Awake()
    {
        roadLoop = GameObject.FindGameObjectWithTag("RoadLoop").GetComponent<RoadLoop>();
    }*//*

    private void Start()
    {
        AgainBtn();
    }

    // ReStart / Start


    public void StartGame()
    {
        ScoreManager.Instance.isStartGame = true;
        ScoreManager.Instance.startPopUp.gameObject.SetActive(false);

        ScoreManager.Instance.StartTimer();  // Timer 시작
    }

    public void AgainBtn()      // 완전 처음 게임 시작 - 다 초기화
    {
        //Score
        ScoreManager.Instance.isStartGame = true;
        ScoreManager.Instance.SavePreScore();
        ScoreManager.Instance.endPopUp.gameObject.SetActive(false);

        //Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        //Road
       // roadLoop.ZeroSpeed(0f);  //Null

        //Spawn
    }*/
}
