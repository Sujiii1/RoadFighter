using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text timerTextUI;
    public TMP_Text scoreTextUI;
    public TMP_Text endScoreTextUI;
    public TMP_Text preBsetScoreTextUI;

    private void Update()
    {
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        endScoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        preBsetScoreTextUI.text = ScoreManager.Instance.GetPreviousScoreTexts();
    }
}
