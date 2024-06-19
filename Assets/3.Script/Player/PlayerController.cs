using System;
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
    public Vector3 playerBasePosition;

    //Collision
    [Header("Collision")]
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float rotationAngle = 45f;


    //Effect
    [Header("Effect")]
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;

    public bool isWall = false;
    public bool isRotate = false;


    //Event
    public event EventHandler onCollision;
    public event EventHandler onWall;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        ObjectPoolingManager.Instance.poolController.playerController = this;

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

    private void Start()
    {
        playerBasePosition = transform.position;
    }


    private void FixedUpdate()
    {
        if (ScoreManager.Instance.isStartGame)
        {
            TouchMovePlayer();
        }
    }

    private void TouchMovePlayer()
    {
        if (Input.GetMouseButton(0) && !isRotate)
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWall = true;
            isRotate = false;
            TimeEnd();

            onCollision?.Invoke(this, EventArgs.Empty);
            onWall?.Invoke(this, EventArgs.Empty);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionPoint = collision.contacts[0].point;                   // 충돌 지점의 평균 위치
        Vector3 direction = collisionPoint - transform.position;                // 플레이어 위치와 충돌 지점 사이의 벡터를 계산
        direction.Normalize();


        if (collision.gameObject.CompareTag("Enemy"))
        {
            isWall = false;
            isRotate = true;
            onCollision?.Invoke(this, EventArgs.Empty);


            if (roadLoop != null)
            {
                ReduceLoadSpeed();
            }

            if (Vector3.Dot(transform.right, direction) > 0)
            {
                playerRB.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);    //right
                transform.rotation = Quaternion.Euler(0, -rotationAngle, 0);
                hitFX.Play();
                StartCoroutine(collision_Co());
            }
            else
            {
                playerRB.AddTorque(Vector3.up * -pushForce, ForceMode.Impulse);   //left
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
                hitFX.Play();
                StartCoroutine(collision_Co());
            }
        }
        else if (collision.gameObject.CompareTag("Bus"))
        {
            isWall = true;
            isRotate = true;
            TimeEnd();
            onCollision?.Invoke(this, EventArgs.Empty);


            playerRB.AddForce(pushForce * Vector3.right, ForceMode.Impulse);
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);


            if (Vector3.Dot(transform.right, direction) > 0)
            {
                playerRB.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);    //right
                transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
                StartCoroutine(collision_Co());
            }
            else
            {
                playerRB.AddTorque(Vector3.up * -pushForce, ForceMode.Impulse);   //left
                transform.rotation = Quaternion.Euler(0, -rotationAngle, 0);
                StartCoroutine(collision_Co());
            }
            StartCoroutine(collision_Co());
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

    public void ReduceLoadSpeed()
    {
        //부딪혔을 때 Road Speed 낮추기

        float currentSpeed = roadLoop.GetSpeed();
        float reducedSpeed = currentSpeed * 0.5f; // 속도를 절반으로 줄임
        roadLoop.SetSpeed(reducedSpeed);



    }

    /*    private void CollisionPlayer()
        {
            playerRB.AddForce(pushForce * Vector3.right, ForceMode.Impulse);
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            hitFX.Play();
            // StartCoroutine(collision_Co());
        }*/

    IEnumerator collision_Co()
    {
        yield return new WaitForSeconds(1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        isRotate = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
