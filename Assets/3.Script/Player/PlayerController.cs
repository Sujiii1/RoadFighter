using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;

    private float horizontalInput;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float smooth = 30f;

    //Effect
    [SerializeField] private ParticleSystem dieFX;
    [SerializeField] private ParticleSystem hitFX;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector3>();
        horizontalInput = input.x * speed;

        // 가속도 제한
        float tar = horizontalInput;
        float smoothedVelocity = Mathf.Lerp(playerRB.velocity.x, tar, Time.deltaTime * smooth);
        playerRB.velocity = new Vector3(smoothedVelocity, playerRB.velocity.y, playerRB.velocity.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            dieFX.Play();
           // Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            hitFX.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
    }
}
