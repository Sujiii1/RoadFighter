
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //ReStart ������
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

        // �Ŀ� ����ȭ�� �Ѵٸ�..?
        // 60�ʿ� �� �� ȣ��
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();

        // ���� �� �� �� ȣ��
        endScoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        preBsetScoreTextUI.text = ScoreManager.Instance.GetPreviousScoreTexts();
    }
}
