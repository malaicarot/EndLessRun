using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Mover : MonoBehaviour
{

    private Animator animator;
    [SerializeField] AudioSource coinsSound;
    [SerializeField] AudioSource powerUpSound;
    [SerializeField] AudioSource painSound;
    [SerializeField] AudioSource screamSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] GameObject StoneThrow;


    public float speed = 1.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float sescondsPerTurn = 0.5f;

    [SerializeField] private float speedBoots = 30f;
    [SerializeField] private float speedOrigin = 15f;


    [SerializeField] private float invincibilityTime = 5f;
    private bool isInvincible = false;


    private HealthManager takeDame;

    [SerializeField] private Rigidbody rb;
    private float lastTurnTime = 0f;

    private Quaternion targetRotation;

    [SerializeField] private CameraFollow RotateCamera;

    [SerializeField] private float jumpForce = 10f;


    private bool isTurning = false;

    private bool isCamUpdateFinish = true;

    private bool isCanTurning = false;

    private bool isGrounded = true;
    public bool isFalling = false;
    public bool isDone = false;
    public bool canMove = true;

    private KeyCode throwKey = KeyCode.W;

    void Start()
    {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        takeDame = FindObjectOfType<HealthManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (!takeDame.isDead && canMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        // if(!isDone){
        //     transform.Translate(Vector3.forward * 0 * Time.deltaTime);
        // }

        float jumpInput = Input.GetAxisRaw("Jump");
        bool isJump = jumpInput != 0;

        animator.SetBool("isJump", isJump);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isDead", takeDame.isDead);
        animator.SetBool("isDone", isDone);
        if(takeDame.isDead){
            deathSound.Play();
        }
        



        if (horizontalInput == 0)
        {
            isTurning = false;
        }

        if (jumpInput != 0 && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;

        }

        animator.SetBool("isThrowing", Input.GetKeyDown(throwKey));
        if (Input.GetKeyDown(throwKey))
        {
            GameObject stone = Instantiate(StoneThrow, transform.position + new Vector3(1f, 3.5f, 0f), Quaternion.identity);
            stone.transform.rotation = transform.rotation;
        }


        if (isCanTurning == false && isCamUpdateFinish)
        {
            Vector3 moveLeftRight = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(moveLeftRight);
        }

        if (Time.time - lastTurnTime < sescondsPerTurn)
        {
            float maxDegreeDelta = 90f / sescondsPerTurn * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreeDelta);
            return;
        }

        if (!isTurning && isCamUpdateFinish && isCanTurning)
        {
            switch (horizontalInput)
            {
                case 0:
                    isTurning = false;
                    break;
                case -1:
                    StartTurn(-90);
                    isTurning = true;
                    isCamUpdateFinish = false;
                    RotateCamera.RotateCamera(-1, () => { isCamUpdateFinish = true; });
                    isCanTurning = false;
                    break;
                case 1:
                    StartTurn(90);
                    isTurning = true;
                    isCamUpdateFinish = false;
                    RotateCamera.RotateCamera(1, () => { isCamUpdateFinish = true; });
                    isCanTurning = false;
                    break;
                default:
                    break;
            }
        }

    }



    private void StartTurn(float angle)
    {
        targetRotation *= Quaternion.Euler(0, angle, 0);

        lastTurnTime = Time.time;

    }

    public void Boost()
    {
        StartCoroutine(SpeedUp());
    }

    IEnumerator SpeedUp()
    {
        speed = speedBoots;
        yield return new WaitForSeconds(5f);
        speed = speedOrigin;
    }

    public void ActiveInvincible()
    {
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mark"))
        {
            if (gameObject.CompareTag("Player"))
            {
                isCanTurning = true;
            }
        }

        if (other.CompareTag("Coins"))
        {
            ScoreManagement.AddScore(1);
            coinsSound.Play();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("SpeedUp"))
        {
            Boost();
            powerUpSound.Play();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Shield"))
        {
            ActiveInvincible();
            powerUpSound.Play();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("DeadZone"))
        {
            screamSound.Play();
            takeDame.TakeDame(3);
            isFalling = true;
        }

        if(other.CompareTag("Obstacle")){
            takeDame.TakeDame(3);
        }

        if(other.CompareTag("House")){
            isDone = true;
            canMove = false;
            StartCoroutine(takeDame.GameOver());

        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

        }

        if (collision.gameObject.CompareTag("Lava") && !isInvincible)
        {
            painSound.Play();
            takeDame.TakeDame(1);
        }


    }
}
