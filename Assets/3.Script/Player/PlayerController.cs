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
    [SerializeField] private float pushForce = 10f;     //밀려나는 힘 
    [SerializeField] private float rotationAngle = 45f;     //밀려나는 힘 


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

        // 가속도 제한
        float tar = horizontalInput;
        float smoothedVelocity = Mathf.Lerp(playerRB.velocity.x, tar, Time.deltaTime * smooth);
        playerRB.velocity = new Vector3(smoothedVelocity, playerRB.velocity.y, playerRB.velocity.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))     //만약 100초가 안지났다면
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

            //부딪혔을 때 Road Speed 낮추기
            if(roadLoop != null) 
            {
                float currentSpeed = roadLoop.GetSpeed();
                float reducedSpeed = currentSpeed * 0.5f; // 속도를 절반으로 줄임
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
