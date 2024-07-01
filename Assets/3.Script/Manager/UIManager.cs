
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //ReStart ������
{
    [SerializeField] private SpawnManager spawnManager;

    [Header("Button")]
    [SerializeField] private Button againBtn;
    [SerializeField] private Button developerBtn;   //developer mode


    [Header("Text")]
    public TMP_Text timerTextUI;
    public TMP_Text scoreTextUI;
    public TMP_Text endScoreTextUI;
    public TMP_Text preBsetScoreTextUI;

    [Header("PopUp")]
    public GameObject startPopUp;
    public GameObject endPopUp;


    private PlayerInput inputActions;


    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
        //inputActions.Touch.TouchPress.started += (context) => Debug.Log("123");

        ScoreManager.Instance.SetUIManager(this);
        //againBtn.onClick.AddListener(ScoreManager.Instance.AgainBtn);
    }


    private void Update()
    {
        UIupdate();
    }


    private void UIupdate()
    {
        // �Ŀ� ����ȭ�� �Ѵٸ�..?
        // 60�ʿ� �� �� ȣ��
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();

        // ���� �� �� �� ȣ��
        endScoreTextUI.text = ScoreManager.Instance.GetScoreTexts();
        preBsetScoreTextUI.text = ScoreManager.Instance.GetPreviousScoreTexts();
    }

    public void ExitPopUp()
    {
        endPopUp.SetActive(false);
        startPopUp.SetActive(true);
    }

    public void ReRoadScene()
    {
        ScoreManager.Instance.isGameOver = false;
        ScoreManager.Instance.isStartGame = true;

        ScoreManager.Instance.SavePreScore();
        ScoreManager.Instance.LoadPreScore();

        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.ResetTime();

        endPopUp.gameObject.SetActive(false);
        startPopUp.gameObject.SetActive(true);
        // spawnManager.SpawnStart();
        if (spawnManager != null)
        {
            //spawnManager.ResetCarObject();          //�ڷ�ƾ �޼��尡 ���� �ȵǴ� ���� : ��CarObject�� ��Ȱ��ȭ �Ǿ����� . spawnManager�� �̱����� �ƴ�.
            //ObjectPool.Instance.ResetAllCarObject();

        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
