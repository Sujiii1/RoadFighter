using System.Collections;
using System;
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
    private bool isAgainBtn = false;



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

        // Score ����
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
        time = 10.0f;
        timeText = "10";
    }

    private void UpdateTimer()
    {
        if (!isStartTime) return;
        if (time > 0.0f)

        {
            time -= Time.deltaTime;
            timeText = Mathf.CeilToInt(time).ToString();   // Mathf.CeilToInt -> float�� int �������� ��ȯ
        }
        else if (time <= 0.0f)   // ���� ����
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

    private void EndTimer()     // Timer�� �� ������ ��
    {
        isStartGame = false;
        isGameOver = true;

        if (uiManager != null)
        {
            uiManager.endPopUp.gameObject.SetActive(true);
        }
        SavePreScore();

        if (isAgainBtn)
        {
            isAgainBtn = false; // �� ���� ���� ����
            StartGameAgain();   // ������ �ٽ� �����ϴ� ������ ������ �и�
        }
    }

    private void StartGameAgain()
    {
        isStartGame = true;
        isGameOver = false;
        ResetTime();
        StartTimer();
    }

    #endregion


    #region  [Score]
    private void ResetScore()   //�ʱ�ȭ
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

    // ���� ���� �Ͻ� ���� �޼��� �߰�
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
        StartTimer();  // Timer ����
    }

    public void AgainBtn()      // ���� ó�� ���� ���� - �� �ʱ�ȭ
    {
        isAgainBtn = true;
        isGameOver = false;
        isStartGame = true;

        SavePreScore();
        ResetScore();
        ResetTime();

        if (uiManager != null)
        {
            uiManager.endPopUp.gameObject.SetActive(false);
            uiManager.startPopUp.gameObject.SetActive(true);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(StartSpawnAfterReload());
    }

    private IEnumerator StartSpawnAfterReload()
    {
        yield return new WaitForSeconds(0.1f); // ���� �ε�� �� �ణ�� ��� �ð�
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null)
        {
            spawnManager.SpawnStart();
        }
    }
}
