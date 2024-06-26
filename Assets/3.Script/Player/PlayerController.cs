using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    [SerializeField] private RoadLoop roadLoop;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PoolController poolController;

    private float horizontalInput;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxX = 4.7f;     //-3.74
    private Vector3 targetPosition;
    public Vector3 playerBasePosition;

    //Collision
    [Header("Collision")]
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float rotationAngle = 45f;

    [SerializeField] private bool isGotMode;
    [SerializeField] private bool isScoreUpItem;
    private WaitForSeconds ItemDuration = new WaitForSeconds(3f);


    //Effect
    [Header("Effect")]
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;
    [SerializeField] private ParticleSystem ItemOnFX;


    // [SerializeField] private bool isWall = false;
    [SerializeField] private bool isRotate = false;
    [SerializeField] private bool isItemOn = false;


    //Event
    public event EventHandler onCollision;
    public event EventHandler onWall;


    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        ObjectPoolingManager.Instance.poolController.playerController = this;

        if (poolController != null)
        {
            poolController = GameObject.FindGameObjectWithTag("ObjectPooling").GetComponent<PoolController>();
        }
    }


    #region [Player KeyBoard Move]
    /*    public void PlayerMove(InputAction.CallbackContext context)
        {
            Vector3 input = context.ReadValue<Vector3>();
            horizontalInput = input.x * speed;

            // ���ӵ� ����
            float tar = horizontalInput;
            float smoothedVelocity = Mathf.Lerp(playerRB.velocity.x, tar, Time.deltaTime);
            playerRB.velocity = new Vector3(smoothedVelocity, playerRB.velocity.y, playerRB.velocity.z);
        }*/
    #endregion


    private void Start()
    {
        playerBasePosition = transform.position;

        if (poolController == null)
        {
            Debug.LogError("poolController is not assigned in the inspector");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
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
                float targetX = Mathf.Clamp(point.x, -maxX, maxX); // x�� �̵� ����
                var direction = targetX > 0 ? Vector3.right : Vector3.left;
                targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isItemOn)
        {
            //isWall = true;
            isRotate = false;
            TimeEnd();

            //Event
            onCollision?.Invoke(this, EventArgs.Empty);
            onWall?.Invoke(this, EventArgs.Empty);

            ObjectPoolingManager.Instance.isPlayerOnWall = true;

            //Pool Position Move
            poolController.isPoolMove = true;

            //�ε��� �� �ʱ�ȭ
            StartCoroutine(WallReSpawn_Co());
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionPoint = collision.contacts[0].point;                   // �浹 ������ ��� ��ġ
        Vector3 direction = collisionPoint - transform.position;                // �÷��̾� ��ġ�� �浹 ���� ������ ���͸� ���
        direction.Normalize();


        if (collision.gameObject.CompareTag("Enemy") && !isItemOn)
        {
            // isWall = false;
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

        else if (collision.gameObject.CompareTag("Bus") && !isItemOn)
        {

            // isWall = true;
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
            isGotMode = other.gameObject.name.Contains("GotModeItem");
            isScoreUpItem = other.gameObject.name.Contains("Cake");

            other.gameObject.SetActive(false);
            other.GetComponent<CarObject>().EnQueueObject();

            if (isGotMode)
            {
                StartCoroutine(GotMode_Co());
            }

            if (isScoreUpItem)
            {
                ScoreManager.Instance.ItemIncreaseScore();
            }
        }
    }

    private void TimeEnd()
    {
        //Timer�� 0
        if (ScoreManager.Instance.time <= 0)
        {
            uiManager.endPopUp.SetActive(true);

            ScoreManager.Instance.SavePreScore();
            ScoreManager.Instance.GameOverScore();

        }

        //���� 100�ʰ� �������ٸ�
        else
        {
            if (!isItemOn)
            {
                dieFX.Play();
                roadLoop.ZeroSpeed(0f);     //�ε� ���� ����
                ScoreManager.Instance.PauseScoreForSeconds(3f);  // 3�� ���� ���� ���� ����
            }
        }
    }

    public void ReduceLoadSpeed()
    {
        //�ε����� �� Road Speed ���߱�

        float currentSpeed = roadLoop.GetSpeed();
        float reducedSpeed = currentSpeed * 0.5f; // �ӵ��� �������� ����
        roadLoop.SetSpeed(reducedSpeed);
    }

    public void InitRespawn()
    {
        ObjectPoolingManager.Instance.isPlayerOnWall = false;

        if (poolController.spawnManager != null)
        {
            lock (spawnManager) // Lock to prevent conflicts
            {
                spawnManager.currentSpawnPosZ = poolController.reSpawnPosition;
                spawnManager.ResetCarObject();
                poolController.carPoolsParent.position = poolController.startPosition;
            }
        }
        poolController.isPoolMove = false;
    }


    IEnumerator WallReSpawn_Co()
    {
        yield return new WaitForSeconds(2f);

        if (!isItemOn)
        {
            InitRespawn();
        }
    }


    IEnumerator collision_Co()
    {
        yield return new WaitForSeconds(1f);

        if (!isItemOn)
        {
            transform.rotation = Quaternion.identity;
            isRotate = false;
        }
    }


    private IEnumerator GotMode_Co()
    {
        isItemOn = true;
        ItemOnFX.Play();
        yield return ItemDuration;
        isItemOn = false;
        ItemOnFX.Stop();
    }
}
