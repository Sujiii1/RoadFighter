using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    private RoadLoop roadLoop;

    public GameObject startPopUp;
    public GameObject endPopUp;

    // Timer
    [Header("Timer")]
    public float time = 100f;
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
        roadLoop = GameObject.FindGameObjectWithTag("RoadLoop").GetComponent<RoadLoop>();
    }

    private void Start()
    {
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

    // Timer
    private void UpdateTimer()
    {
        if (!isStartTime) return;
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText = Mathf.CeilToInt(time).ToString();   // Mathf.CeilToInt -> float�� int �������� ��ȯ
        }
        else    // ���� ����
        {
            endPopUp.gameObject.SetActive(true);    // Timer�� �� ������ ��

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

    // Score
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

    // UI
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


    // ReStart / Start


    public void StartGame()
    {
        isStartGame = true;
        startPopUp.gameObject.SetActive(false);

        StartTimer(); // Ÿ�̸� ����
    }

    public void AgainBtn() // ���� ó�� ���� ���� - �� �ʱ�ȭ
    {
        SavePreScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isStartGame = true;

        roadLoop.ZeroSpeed(0f); //Null
        endPopUp.gameObject.SetActive(false);
    }
}
