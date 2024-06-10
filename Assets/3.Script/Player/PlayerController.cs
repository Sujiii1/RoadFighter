using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    [SerializeField]private RoadLoop roadLoop;

    private float horizontalInput;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float smooth = 30f;

    //Collision
    [SerializeField] private float pushForce = 10f;     //�з����� �� 
    [SerializeField] private float rotationAngle = 45f;     //�з����� �� 


    //Effect
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;

    public bool isDie = false;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        // player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        roadLoop = GameObject.FindGameObjectWithTag("Road").GetComponent<RoadLoop>();
    }



    public void PlayerMove(InputAction.CallbackContext context)
    {
        if (isDie) return;

        Vector3 input = context.ReadValue<Vector3>();
        horizontalInput = input.x * speed;

        // ���ӵ� ����
        float tar = horizontalInput;
        float smoothedVelocity = Mathf.Lerp(playerRB.velocity.x, tar, Time.deltaTime * smooth);
        playerRB.velocity = new Vector3(smoothedVelocity, playerRB.velocity.y, playerRB.velocity.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))     //���� 100�ʰ� �������ٸ�
        {
            isDie = true;
            dieFX.Play();
            //Destroy(gameObject);
            if(ScoreManager.Instance.time == 0)
            {
                ScoreManager.Instance.SavePreScore();
                ScoreManager.Instance.endPopUp.SetActive(true);
                
            }
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            if (isDie) return;

            playerRB.AddForce(pushForce * Vector3.right, ForceMode.Impulse);
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
            hitFX.Play();
            StartCoroutine(collision_Co());

            //�ε����� �� Road Speed ���߱�
            if(roadLoop != null) 
            {
                float currentSpeed = roadLoop.GetSpeed();
                float reducedSpeed = currentSpeed * 0.5f; // �ӵ��� �������� ����
                roadLoop.SetSpeed(reducedSpeed);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator collision_Co()
    {
        yield return new WaitForSeconds(2f);
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}
