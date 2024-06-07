using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour   //Timer, Score
{
    //Timer
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject endPopUp;

    private float time = 100f;
    private bool startTime;

    //Score
    [SerializeField] private TMP_Text scoreText;
    private float scoreTime = 0;
    private int preBsetScore = 0;

    private void Start()
    {
        StartTimer();
        StartCoroutine(IncreaseScore_Co());
    }

    private void Update()
    {
        UpdateTimer();
        UpdateScore();
    }

    //Timer
    private void UpdateTimer()
    {
        if (!startTime) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(time).ToString();   // Mathf.CeilToInt -> float을 int 형식으로 변환
        }
        else
        {
            endPopUp.gameObject.SetActive(true);    //Timer가 다 끝났을 때
        }
    }

    private void StartTimer()
    {
        timeText.text = time.ToString();
        startTime = true;
    }


    //Score
    private void UpdateScore()
    {
        scoreText.text = Mathf.CeilToInt(scoreTime).ToString("000000");
    }

    IEnumerator IncreaseScore_Co()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            scoreTime += 50;
            UpdateScore();
        }
    }

    public void LoadPreScore()
    {
        preBsetScore = PlayerPrefs.GetInt("preBsetScore", 0);
    }

    public void SavePreScore()
    {
        PlayerPrefs.SetInt("preBsetScore", preBsetScore);
        PlayerPrefs.Save();
    }

/*    public TMP_Text GetScoreTexts()
    {
        return scoreText;
    }*/
}
