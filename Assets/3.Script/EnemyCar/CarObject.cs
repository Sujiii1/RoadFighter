using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CarType
{
    None = -1,
    Yellow,     // 가만히 있는 차
    Green,      // 느리게 움직이는 차
    Mint,       // 빠르게 움직이는 차
    Bus         // 가만히 있는데 부딪치면 무조건 죽는 차
}


public class CarObject : MonoBehaviour
{
    //=================외부 변수====================



    /// <summary>
    /// Enemy CarType Setting
    /// </summary>
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
    [Header("자동차 상태")]
    [SerializeField] private CarType carType;
    [SerializeField] private bool isBus = false;

    [Header("플레이어 오브젝트")]
    [SerializeField] private GameObject player;

    #region [자동차 속도]
    [Header("자동차 속도")]
    [SerializeField] private float carSpeed_x;
    #endregion

    private WaitForSeconds waitTime = new WaitForSeconds(3f);

    private float xLimit = 3.5f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float rotationAngle = 45f;





    private bool isFindPlayer = false;
    private bool isAccident = false;
    private bool isCheck = false;
    private bool isRight;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    private void Start()
    {
        switch (carType)
        {
            case CarType.Yellow:
                carSpeed_x = 0.0f;
                break;

            case CarType.Green:
                carSpeed_x = 5.0f;
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
            gameObject.SetActive(false);

            EnQueueObject();
        }
        else if (collision.gameObject.CompareTag("Player"))     //속도 느려짐
        {
            Rigidbody enemyRB = GetComponent<Rigidbody>();
            enemyRB.AddForce(pushForce * new Vector3(1, -1, 0), ForceMode.Impulse);   //대각선으로 밀려남

            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            StartCoroutine(collision_Co());


            //앞으로 가기 
        }
    }
    private void Update()
    {
        FindPlayer();
        CheckDirection();

        if (transform.position.z <= -7f)
        {
            EnQueueObject();
        }
    }

    private void FixedUpdate()
    {
        CarMove_x();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void EnQueueObject()
    {
        gameObject.SetActive(false);

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

    private IEnumerator Accident_Timer()
    {
        isAccident = true;
        yield return waitTime;
        isAccident = false;
    }

    private void FindPlayer()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= 5f)
        {
            isFindPlayer = true;
        }
    }


    private void CheckDirection()
    {
        if (isCheck || !isFindPlayer) return;

        // Vector3 cross = Vector3.Cross(transform.position.normalized, player.transform.position.normalized);
        Vector3 directionPlayer = player.transform.position - transform.position;

        if (Vector3.Dot(directionPlayer, transform.right) > 0f) //Right
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
            if (transform.position.x < -xLimit)
            {
                transform.position = new Vector3(-xLimit, transform.position.y, transform.position.z);
            }
            return;
        }
    }

    IEnumerator collision_Co()
    {
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}