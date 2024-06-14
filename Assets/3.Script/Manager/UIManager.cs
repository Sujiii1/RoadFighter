using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour //ReStart ¾ø¾îÁü
{

    public TMP_Text timerTextUI;
    public TMP_Text scoreTextUI;
    public TMP_Text endScoreTextUI;
    public TMP_Text preBsetScoreTextUI;

    public GameObject startPopUp;
    public GameObject endPopUp;

    private PlayerInput inputActions;

    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
        //inputActions.Touch.TouchPress.started += (context) => Debug.Log("123");
    }

    private void Update()
    {
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        endScoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        preBsetScoreTextUI.text = ScoreManager.Instance.GetPreviousScoreTexts();
    }
}
