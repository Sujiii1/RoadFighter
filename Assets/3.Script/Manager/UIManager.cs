
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //ReStart 없어짐
{
    [SerializeField] private Button againBtn;


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
        ScoreManager.Instance.SetUIManager(this);

        againBtn.onClick.AddListener(ScoreManager.Instance.AgainBtn);
    }

    private void Update()
    {

        // 후에 최적화를 한다면..?
        // 60초에 한 번 호출
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();

        // 끝날 때 한 번 호출
        endScoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        preBsetScoreTextUI.text = ScoreManager.Instance.GetPreviousScoreTexts();
    }
}
