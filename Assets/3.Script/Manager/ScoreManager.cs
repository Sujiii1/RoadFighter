using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour   //Timer, Score
{
    public static ScoreManager Instance = null;

    private RoadLoop roadLoop;

    public GameObject startPopUp;
    public GameObject endPopUp;

    //Timer
    [Header("Timer")]
    public float time = 100f;
    public string timeText;
    private bool isStartTime;

    //Score
    [Header ("Score")]
    public string scoreText;
    public string preBsetScoreText;
    public float scoreTime = 0;
    private int preBsetScore = 0;

    [Header(" ")]
    public bool isStartGame = false;
    public bool isGameOver = false;

    private void Awake()
    {
        #region [SingleTone]
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        roadLoop = GameObject.FindGameObjectWithTag("RoadLoop").GetComponent<RoadLoop>();
    }

    private void Start()
    {
        LoadPreScore();
        StartTimer();
        StartCoroutine(IncreaseScore_Co());
    }

    private void Update()
    {
        if(isStartGame)
        {
            UpdateTimer();
            UpdateScore();
        }
    }
    


    //Timer
    private void UpdateTimer()
    {
        if (!isStartTime) return;
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText = Mathf.CeilToInt(time).ToString();   // Mathf.CeilToInt -> float을 int 형식으로 변환
        }
        else    // 게임 종료
        {
            endPopUp.gameObject.SetActive(true);    //Timer가 다 끝났을 때
            SavePreScore();

            isStartGame = false;
            isGameOver = true;
        }
    }

    private void StartTimer()
    {
        timeText = time.ToString();
        isStartTime = true;
    }



    //Score
    public void UpdateScore()
    {
        isStartGame = true;
        scoreText = Mathf.CeilToInt(scoreTime).ToString("000000");
    }

    IEnumerator IncreaseScore_Co()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(isStartGame)
            {
                scoreTime += 50;
                UpdateScore();
            }
        }
    }


    //PreScore
    public void LoadPreScore()
    {
        preBsetScore = PlayerPrefs.GetInt("preBsetScore", 0);
        preBsetScoreText = Mathf.CeilToInt(preBsetScore).ToString("000000");
    }

    public void SavePreScore()
    {
        if(scoreTime > preBsetScore)
        {
            preBsetScore = Mathf.CeilToInt(scoreTime);
            PlayerPrefs.SetInt("preBsetScore", preBsetScore);
            PlayerPrefs.Save();
        }
    }



    //UI
    public string GetTimerTexts()
    {
        return timeText;
    }

    public string GetScoreTexts()
    {
        return scoreText;
    }
    public string GetPreviousScoreTexts()
    {
        return preBsetScoreText;
    }

    
    //ReStart / Start

    public void StartGame()
    {
        isStartGame = false;
        //speed = 0;
        startPopUp.gameObject.SetActive(false);
    }

    public void ReStart()      //완전 처음 게임 시작 - 다 초기화
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isStartGame = true;


        roadLoop.ZeroSpeed(0f);
        endPopUp.gameObject.SetActive(false);
    }

}
