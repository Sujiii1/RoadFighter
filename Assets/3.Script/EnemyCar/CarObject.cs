
using System.Collections;
using UnityEngine;


public enum CarType
{
    None = -1,
    Yellow,     // 가만히 있는 차
    Green,      // 느리게 움직이는 차
    Mint,       // 빠르게 움직이는 차
    Bus,       // 가만히 있는데 부딪치면 무조건 죽는 차
    Empty         // 가만히 있는데 부딪치면 무조건 죽는 차
}


public class CarObject : MonoBehaviour
{
    //=================외부 변수====================


    /// <summary>
    /// Enemy CarType Setting
    /// </summary>
    /// 
    /// <param name="carType"> 자동차 상태 </param>
    public void SetCarType(CarType carType)
    {
        this.carType = carType;
    }

    public void Accident_Co()
    {
        StartCoroutine(Accident_Timer());
    }

    public bool pIsBus { get { return isBus; } }


    //=================내부 변수====================
    private RoadLoop roadLoop;

    [Header("Car Type")]
    [SerializeField] private CarType carType;
    [SerializeField] private bool isBus = false;

    [Header("Player")]
    [SerializeField] private GameObject player;

    #region [자동차 속도]
    [Header("Car Speed")]
    [SerializeField] private float carSpeed_x;
    #endregion

    private float xLimit = 4.2f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float rotationAngle = 45f;


    [Header("Effect")]
    // [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private GameObject ren;

    private WaitForSeconds waitTime = new WaitForSeconds(3f);
    private WaitForSeconds CollisionTime = new WaitForSeconds(2f);

    private Rigidbody enemyRB;
    [SerializeField] private bool isFindPlayer = false;
    [SerializeField] private bool isAccident = false;
    [SerializeField] private bool isCheck = false;
    [SerializeField] private bool isRight;


    private void Awake()
    {
        roadLoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
        enemyRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        ren.SetActive(false);

        if (player == null)
        {
            Debug.LogError("Player not found.");
        }

        switch (carType)
        {
            case CarType.Yellow:
                carSpeed_x = 0.0f;
                break;

            case CarType.Green:
                carSpeed_x = 7.0f;
                break;

            case CarType.Mint:
                carSpeed_x = 10.0f;
                break;

            case CarType.Bus:
                carSpeed_x = 0.0f;
                isBus = true;
                break;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //dieFX.Play();
            ren.SetActive(true);
            StartCoroutine(DestroyCar_Co());
        }
        else if (collision.gameObject.CompareTag("Player"))     //속도 느려짐
        {
            #region [충돌 방향 회전]
            Vector3 collisionPoint = collision.contacts[0].point;                   // 충돌 지점의 평균 위치
            Vector3 direction = collisionPoint - transform.position;                // 플레이어 위치와 충돌 지점 사이의 벡터를 계산
            direction.Normalize();                                                   // 방향 벡터를 정규화

            if (!isBus)
            {

                if (Vector3.Dot(transform.right, direction) > 0)
                {
                    enemyRB.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);    //right
                    transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
                    StartCoroutine(Collision_Co());
                }
                else
                {
                    enemyRB.AddTorque(Vector3.up * -pushForce, ForceMode.Impulse);   //left
                    transform.rotation = Quaternion.Euler(0, -rotationAngle, 0);
                    StartCoroutine(Collision_Co());
                }
            }
            else
            {
                roadLoop.ZeroSpeed(0f);  //로드루프 멈춤
                enemyRB.AddForce(20f * new Vector3(1, -1, 0), ForceMode.Impulse);   //대각선으로 밀려남
            }
        }
        #endregion
    }


    private void FixedUpdate()
    {
        CarMove_x();
    }


    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerController>()?.gameObject;

            if (player != null)
            {
                FindPlayer();
                CheckDirection();
            }
        }
        else
        {
            FindPlayer();
            CheckDirection();
        }

        if (transform.position.z <= -7f)
        {
            EnQueueObject();
        }
    }



    #region [초기화]
    private void OnEnable()
    {
        StopAllCoroutines();

        InitializePlayer();
        ResetCarState();
    }

    private void InitializePlayer()
    {
        player = GameObject.FindObjectOfType<PlayerController>()?.gameObject;
        isFindPlayer = false;
        isCheck = false;
    }

    private void ResetCarState()
    {
        isFindPlayer = false;
        isCheck = false;
        isAccident = false;
        ren.SetActive(false);

        transform.rotation = Quaternion.identity;
    }
    #endregion



    private void OnDisable()
    {
        StopAllCoroutines();
        CarMove_x();
        isFindPlayer = false;
        isCheck = false;
    }

    private void EnQueueObject()
    {
        gameObject.SetActive(false);
        ResetCarState();

        switch (carType)
        {
            case CarType.Yellow:
                ObjectPoolingManager.Instance.YellowcarObjectPool.Enqueue(this);
                break;

            case CarType.Green:
                ObjectPoolingManager.Instance.GreencarObjectPool.Enqueue(this);
                break;

            case CarType.Mint:
                ObjectPoolingManager.Instance.MintcarObjectPool.Enqueue(this);
                break;

            case CarType.Bus:
                ObjectPoolingManager.Instance.BuscarObjectPool.Enqueue(this);
                break;
        }
    }



    #region   [Collision]

    private void FindPlayer()
    {

        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= 5f)
            {
                isFindPlayer = true;
            }
        }
    }

    private void CheckDirection()
    {
        if (isCheck || !isFindPlayer) return;

        Vector3 directionPlayer = player.transform.position - transform.position;

        if (Vector3.Dot(directionPlayer, transform.right) > 0f)     //Right
        {
            isRight = true;
            isCheck = true;
        }
        else    //Left
        {
            isRight = false;
            isCheck = true;
        }
    }


    private void CarMove_x()
    {
        if (isAccident) return;

        if (!isCheck || !isFindPlayer) return;

        if (isRight)
        {
            transform.Translate(Vector3.right * carSpeed_x * Time.deltaTime);

            if (transform.position.x > xLimit)
            {
                transform.position = new Vector3(xLimit, transform.position.y, transform.position.z);
            }

            return;
        }
        if (!isRight)
        {
            transform.Translate(Vector3.left * carSpeed_x * Time.deltaTime);
            if (transform.position.x < -3.7f)
            {
                transform.position = new Vector3(-3.7f, transform.position.y, transform.position.z);
            }
            return;
        }
    }


    IEnumerator Collision_Co()
    {
        yield return CollisionTime; ;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    //적 사라지는 코루틴
    IEnumerator DestroyCar_Co()
    {
        //yield return new WaitForSeconds(dieFX.main.duration);
        yield return CollisionTime;
        gameObject.SetActive(false);
        EnQueueObject();
    }

    private IEnumerator Accident_Timer()
    {
        isAccident = true;
        yield return waitTime;
        isAccident = false;
    }

    #endregion

}
