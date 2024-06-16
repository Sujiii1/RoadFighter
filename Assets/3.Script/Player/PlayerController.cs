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
    [SerializeField] private float pushForce = 10f;     //�з����� �� 
    [SerializeField] private float rotationAngle = 45f;     //�з����� �� 


    //Effect
    [Header("Effect")]
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;

    public bool isWall = false;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        //�̷� ���� ���� �������ּ���.
        //roadLoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        //cameraController = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraController>();
    }


    /*    public void PlayerMove(InputAction.CallbackContext context)
        {
            Vector3 input = context.ReadValue<Vector3>();
            horizontalInput = input.x * speed;

            // ���ӵ� ����
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
                float targetX = Mathf.Clamp(point.x, -maxX, maxX); // x�� �̵� ����
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

            //�ε����� �� Road Speed ���߱�
            if (roadLoop != null)
            {
                float currentSpeed = roadLoop.GetSpeed();
                float reducedSpeed = currentSpeed * 0.5f; // �ӵ��� �������� ����
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
            //StartCoroutine(cameraController.RePos_Co());
            dieFX.Play();
            roadLoop.ZeroSpeed(0f);     //�ε� ���� ����
            ScoreManager.Instance.PauseScoreForSeconds(3f);  // 3�� ���� ���� ���� ����
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
        //�Ʒ� �޼���� �ش� *��ũ��Ʈ�� ��� �ڷ�ƾ�� Stop�Ѵ�.
        //���⼭ �߿��� ���� �ٸ� ��ũ��Ʈ�� �ڷ�ƾ�� Stop��Ű���� �ʴ´ٴ� ��
        //�̴� �ٸ� ����� �ڵ忡 ������ ���ٴ� ���̱� ������
        //������ �� �� ����
        StopAllCoroutines();
    }
}
