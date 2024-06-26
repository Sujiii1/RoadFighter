using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    // ==========================================
    // 외부 참조
    // ==========================================

    public void SetUIManager(UIManager uIManager)
    {
        this.uiManager = uIManager;
    }


    // ==========================================
    // 내부 변수
    // ==========================================



    [SerializeField] private UIManager uiManager;


    // Timer
    [Header("Timer")]
    public float time = 50f;
    public string timeText;
    private bool isStartTime;

    // Score
    [Header("Score")]
    public string scoreText;
    public string preBsetScoreText;
    public float scoreTime = 0;
    private int preBsetScore = 0;
    private float increaseScore = 50;

    [Header(" ")]
    public bool isStartGame = false;
    public bool isGameOver = false;
    public bool isPauseScore = false;



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
    }

    private void Start()
    {
        if (isGameOver.Equals(true))
        {
            return;
        }

        if (uiManager == null)
        {
            Debug.LogError("UIManager None");
        }
        uiManager.startPopUp.gameObject.SetActive(true);
        LoadPreScore();
        ResetScore();

        // Score 오름
        StartCoroutine(IncreaseScore_Co());
    }

    private void Update()
    {
        if (isStartGame)
        {
            UpdateTimer();
            UpdateScore();
        }
    }

    public void ItemIncreaseScore()     //Item Score Up
    {
        if (isStartGame && !isPauseScore)
        {
            scoreTime += 1000;
            UpdateScore();
        }
    }

    #region  [Timer]

    public void ResetTime()
    {
        time = 50.0f;
        timeText = "50";
    }

    private void UpdateTimer()
    {
        if (!isStartTime) return;

        if (time > 0.0f)
        {
            time -= Time.deltaTime;
            timeText = Mathf.CeilToInt(time).ToString();   // Mathf.CeilToInt -> float을 int 형식으로 변환
        }
        else if (time <= 0.0f)   // 게임 종료
        {
            time = 0.0f;
            timeText = "0";
            EndTimer();
        }
    }

    public void StartTimer()
    {
        timeText = time.ToString();
        isStartTime = true;
    }

    private void EndTimer()     // Timer가 다 끝났을 때
    {
        isStartGame = false;
        isGameOver = true;

        if (uiManager != null)
        {
            uiManager.endPopUp.gameObject.SetActive(true);
        }
        SavePreScore();
    }


    #endregion


    #region  [Score]


    public void ResetScore()   //초기화
    {
        scoreTime = 0;
        scoreText = "000000";
    }

    public void UpdateScore()
    {
        scoreText = Mathf.CeilToInt(scoreTime).ToString("000000");
    }


    IEnumerator IncreaseScore_Co()
    {
        WaitForSeconds waitTime = new WaitForSeconds(1f);

        while (true)
        {
            yield return waitTime;
            if (isStartGame && !isPauseScore)
            {
                scoreTime += increaseScore;
                UpdateScore();
            }
        }
    }

    // 점수 증가 일시 중지 메서드 추가
    public IEnumerator ReInCrease_Co(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPauseScore = false;
    }

    public void PauseScoreForSeconds(float seconds)
    {
        isPauseScore = true;
        StartCoroutine(ReInCrease_Co(delay: seconds));
    }

    public void GameOverScore()
    {
        isPauseScore = true;
    }


    // PreScore
    public void LoadPreScore()
    {
        //PlayerPrefs는 로컬 레지스트리에 저장

        //PlayerPrefs 초기화 코드
        //PlayerPrefs.DeleteAll();


        preBsetScore = PlayerPrefs.GetInt("preBsetScore", 0);
        preBsetScoreText = Mathf.CeilToInt(preBsetScore).ToString("000000");
    }

    public void SavePreScore()
    {
        if (scoreTime > preBsetScore)
        {
            preBsetScore = Mathf.CeilToInt(scoreTime);
            PlayerPrefs.SetInt("preBsetScore", preBsetScore);
            PlayerPrefs.Save();
        }
    }
    #endregion


    #region [Get UI]
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
    #endregion

    public void StartGame()
    {
        isGameOver = false;
        isStartGame = true;

        if (uiManager != null)
        {
            uiManager.startPopUp.gameObject.SetActive(false);
        }

        if (!isGameOver)
        {
            uiManager.startPopUp.gameObject.SetActive(false);
        }
        StartTimer();  // Timer 시작
    }
}
