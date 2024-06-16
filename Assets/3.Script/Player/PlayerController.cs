using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    [SerializeField] private RoadLoop roadLoop;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraController cameraController;

    private float horizontalInput;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxX = 4.7f;
    private Vector3 targetPosition;

    //Collision
    [Header("Collision")]
    [SerializeField] private float pushForce = 10f;     //밀려나는 힘 
    [SerializeField] private float rotationAngle = 45f;     //밀려나는 힘 


    //Effect
    [Header("Effect")]
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;

    public bool isWall = false;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        //이런 것은 직접 참조해주세요.
        //roadLoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        //cameraController = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraController>();
    }


    /*    public void PlayerMove(InputAction.CallbackContext context)
        {
            Vector3 input = context.ReadValue<Vector3>();
            horizontalInput = input.x * speed;

            // 가속도 제한
            float tar = horizontalInput;
            float smoothedVelocity = Mathf.Lerp(playerRB.velocity.x, tar, Time.deltaTime);
            playerRB.velocity = new Vector3(smoothedVelocity, playerRB.velocity.y, playerRB.velocity.z);
        }*/

    private void FixedUpdate()
    {
        if (ScoreManager.Instance.isStartGame)
        {
            TouchMovePlayer();
        }
    }

    private void TouchMovePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 point = hit.point;
                float targetX = Mathf.Clamp(point.x, -maxX, maxX); // x축 이동 제한
                var direction = targetX > 0 ? Vector3.right : Vector3.left;
                targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Bus"))
        {
            isWall = true;
            TimeEnd();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            isWall = false;
            CollisionPlayer();

            //부딪혔을 때 Road Speed 낮추기
            if (roadLoop != null)
            {
                float currentSpeed = roadLoop.GetSpeed();
                float reducedSpeed = currentSpeed * 0.5f; // 속도를 절반으로 줄임
                roadLoop.SetSpeed(reducedSpeed);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);

            ScoreManager.Instance.ItemIncreaseScore();
        }
    }

    private void TimeEnd()
    {
        //Timer가 0
        if (ScoreManager.Instance.time <= 0)
        {
            uiManager.endPopUp.SetActive(true);

            ScoreManager.Instance.SavePreScore();
            ScoreManager.Instance.GameOverScore();

        }

        //만약 100초가 안지났다면
        else
        {
            //StartCoroutine(cameraController.RePos_Co());
            dieFX.Play();
            roadLoop.ZeroSpeed(0f);     //로드 루프 멈춤
            ScoreManager.Instance.PauseScoreForSeconds(3f);  // 3초 동안 점수 증가 멈춤
        }
    }

    private void CollisionPlayer()
    {
        playerRB.AddForce(pushForce * Vector3.right, ForceMode.Impulse);
        transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        hitFX.Play();
        StartCoroutine(collision_Co());
    }

    IEnumerator collision_Co()
    {
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnDestroy()
    {
        //아래 메서드는 해당 *스크립트의 모든 코루틴을 Stop한다.
        //여기서 중요한 것은 다른 스크립트의 코루틴을 Stop시키지는 않는다는 것
        //이는 다른 사람의 코드에 영향이 없다는 것이기 때문에
        //협업할 때 걍 쓰자
        StopAllCoroutines();
    }
}
