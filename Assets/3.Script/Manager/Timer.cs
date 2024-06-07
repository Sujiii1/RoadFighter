using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour  //100 Second Limit
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject endPopUp;

    private float time = 100f;
    private bool startTime;


    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

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
}
