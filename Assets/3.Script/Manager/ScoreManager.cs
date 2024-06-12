using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    private UIManager uiManager;


    // Timer
    [Header("Timer")]
    public float time = 10f;
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
    private bool isPauseScore = false;


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
        uiManager = GameObject.FindGameObjectWithTag("UI")?.GetComponent<UIManager>();

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


    #region  [Timer]

    public void ResetTime()
    {
        time = 100;
        timeText = "100";
    }

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
            if (uiManager != null)
            {
                uiManager.endPopUp.gameObject.SetActive(true);
            }
            // Timer가 다 끝났을 때

            SavePreScore();

            isStartGame = false;
            isGameOver = true;

        }
    }

    public void StartTimer()
    {
        timeText = time.ToString();
        isStartTime = true;
    }

    #endregion


    #region  [Score]
    private void ResetScore()   //초기화
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
        while (true)
        {
            yield return new WaitForSeconds(1f);
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
        StartCoroutine(ReInCrease_Co(seconds));
    }

    public void GameOverScore()
    {
        isPauseScore = true;
    }


    // PreScore
    public void LoadPreScore()
    {
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

    public void AgainBtn()      // 완전 처음 게임 시작 - 다 초기화
    {
        SavePreScore();
        ResetScore();
        ResetTime();
        isGameOver = false;
        isStartGame = true;

        if (uiManager != null)
        {
            uiManager.startPopUp.gameObject.SetActive(false);
            uiManager.endPopUp.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(StartSpawnAfterReload());
    }



    private IEnumerator StartSpawnAfterReload()
    {
        yield return new WaitForSeconds(0.1f); // 씬이 로드된 후 약간의 대기 시간
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.SpawnStart();
        }
    }
}
