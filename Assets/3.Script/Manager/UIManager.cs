
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //ReStart 없어짐
{
    [SerializeField] private Button againBtn;
    [SerializeField] private SpawnManager spawnManager;

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
        //againBtn.onClick.AddListener(ScoreManager.Instance.AgainBtn);
    }


    private void Update()
    {
        UIupdate();
    }


    private void UIupdate()
    {
        // 후에 최적화를 한다면..?
        // 60초에 한 번 호출
        timerTextUI.text = ScoreManager.Instance.GetTimerTexts();
        scoreTextUI.text = ScoreManager.Instance.GetScoreTexts();

        // 끝날 때 한 번 호출
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
        //isAgainBtn = true;
        ScoreManager.Instance.isGameOver = false;
        ScoreManager.Instance.isStartGame = true;

        ScoreManager.Instance.SavePreScore();
        ScoreManager.Instance.ResetScore();
        ScoreManager.Instance.ResetTime();

        endPopUp.gameObject.SetActive(false);
        startPopUp.gameObject.SetActive(true);
        // spawnManager.SpawnStart();
        if (spawnManager != null)
        {
            spawnManager.ResetCarObject();          //코루틴 메서드가 실행 안되는 이유 : ㄴCarObject가 비활성화 되어있음 . spawnManager가 싱글톤이 아님.
        }


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
