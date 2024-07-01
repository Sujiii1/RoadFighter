
using System.Collections;
using UnityEngine;


public enum CarType
{
    None = -1,
    Yellow,     // ������ �ִ� ��
    Green,      // ������ �����̴� ��
    Mint,       // ������ �����̴� ��
    Bus,       // ������ �ִµ� �ε�ġ�� ������ �״� ��
    ScoreUpItem,
    GotModeItem
}


public class CarObject : MonoBehaviour
{
    //=================�ܺ� ����====================


    /// <summary>
    /// Enemy CarType Setting
    /// </summary>
    /// 
    /// <param name="carType"> �ڵ��� ���� </param>
    public void SetCarType(CarType carType)
    {
        this.carType = carType;
    }

    public void Accident_Co()
    {
        StartCoroutine(Accident_Timer());
    }

    public bool pIsBus { get { return isBus; } }


    //=================���� ����====================
    private RoadLoop roadLoop;

    [Header("Car Type")]
    public CarType carType;
    public CarType CarType => carType;  // public getter, but private setter


    [SerializeField] private bool isBus = false;

    [Header("Player")]
    [SerializeField] private GameObject player;

    #region [�ڵ��� �ӵ�]
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

            case CarType.ScoreUpItem:
                carSpeed_x = 0.0f;
                break;

            case CarType.GotModeItem:

                carSpeed_x = 0.0f;

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
        else if (collision.gameObject.CompareTag("Player"))     //�ӵ� ������
        {
            #region [�浹 ���� ȸ��]
            Vector3 collisionPoint = collision.contacts[0].point;                   // �浹 ������ ��� ��ġ
            Vector3 direction = collisionPoint - transform.position;                // �÷��̾� ��ġ�� �浹 ���� ������ ���͸� ���
            direction.Normalize();                                                   // ���� ���͸� ����ȭ

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
                roadLoop.ZeroSpeed();  //�ε���� ����
                enemyRB.AddForce(20f * new Vector3(1, -1, 0), ForceMode.Impulse);   //�밢������ �з���
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



    #region [�ʱ�ȭ]
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

    public void EnQueueObject()
    {
        //gameObject.SetActive(false);
        ResetCarState();
        ObjectPool.Instance.EnqueueObject(gameObject);

        /* switch (carType)
         {
             case CarType.Yellow:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnYellowCar(this);
                     ObjectPool.Instance.RemainYellow.Dequeue();
                 }
                 break;

             case CarType.Green:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnGreenCar(this);
                     ObjectPool.Instance.RemainGreen.Dequeue();
                 }
                 break;

             case CarType.Mint:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnMintCar(this);
                     ObjectPool.Instance.RemainMint.Dequeue();
                 }
                 break;

             case CarType.Bus:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnBusCar(this);
                     ObjectPool.Instance.RemainBus.Dequeue();
                 }
                 break;

             case CarType.ScoreUpItem:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnScoreItem(this);
                     ObjectPool.Instance.RemainScoreUpItem.Dequeue();
                 }
                 break;

             case CarType.GotModeItem:
                 if (ObjectPool.Instance.RemainYellow.Count > 0)
                 {
                     ObjectPool.Instance.ReturnGotItem(this);
                     ObjectPool.Instance.RemainScoreUpItem.Dequeue();
                 }
                 break;
         }*/
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

    //�� ������� �ڷ�ƾ
    IEnumerator DestroyCar_Co()
    {
        //yield return new WaitForSeconds(dieFX.main.duration);
        yield return CollisionTime;
        gameObject.SetActive(false);
        ObjectPool.Instance.EnqueueObject(gameObject);
    }

    private IEnumerator Accident_Timer()
    {
        isAccident = true;
        yield return waitTime;
        isAccident = false;
    }

    #endregion

}
