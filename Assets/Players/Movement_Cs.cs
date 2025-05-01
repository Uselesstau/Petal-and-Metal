using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Movement_Cs : MonoBehaviour
{
    [SerializeField] private SFX_Cs audioSource;
    
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] float springForce;
    public Rigidbody2D rb;
    private Animator animator;

    private float horizontalMovement;
    
    public bool petal;
    
    public bool magnetized;
    
    private Coroutine currentShakeCoroutine;
    private Vector3 cameraStartingPos = new Vector3(0, 0, -10);

    public bool startingLevel;
    public bool isDead;

    [SerializeField] private Transform rayCastPosition1;
    [SerializeField] private Transform rayCastPosition2;
    [SerializeField] private Transform rayCastPosition3;

    bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (!petal)
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
            Physics2D.IgnoreLayerCollision(8, 4, true);
        }

        
        PlayerInput input = GetComponent<PlayerInput>();
        if (petal)
        {
            input.SwitchCurrentControlScheme(new[] { Joystick.all[0] });
        }
        else
        {
            input.SwitchCurrentControlScheme(new[] { Joystick.all[1] });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        AnimatePlayer();
        if (startingLevel)
        {
            horizontalMovement = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
            return;
        }
    }

    void FixedUpdate()
    {
        if (startingLevel) return;
        Movement();
    }

    void AnimatePlayer()
    {
        if (rb.linearVelocityX > 0.1f)
        {
            animator.SetBool("Walk", true);
            animator.SetFloat("Idle", 5);
            transform.localScale = new Vector3(1, 1, 1);
            return;
        }
        if (rb.linearVelocityX < -0.1f)
        {
            animator.SetBool("Walk", true);
            animator.SetFloat("Idle", 5);
            transform.localScale = new Vector3(-1, 1, 1);
            return;
        }
        animator.SetBool("Walk", false);
        animator.SetFloat("Idle", animator.GetFloat("Idle") - Time.deltaTime);
        if (animator.GetFloat("Idle") < -0.5f)
        {
            animator.SetFloat("Idle", 5);
        }
    }

    void Movement()
    {
        if ((rb.linearVelocity.x < maxSpeed && rb.linearVelocity.x >= 0) || (rb.linearVelocity.x > -maxSpeed && rb.linearVelocity.x < 0)) rb.linearVelocityX = horizontalMovement * maxSpeed;
        if (rb.linearVelocity.x >= maxSpeed || rb.linearVelocity.x <= -maxSpeed) rb.linearVelocityX += horizontalMovement * maxSpeed * Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();
        Vector2 dir = Vector2.zero;
        if (v.x < 0) dir.x = -1;
        else if (v.x > 0) dir.x = 1;
        if (v.y > 0) dir.y = 1;
        else if (v.y < 0) dir.y = -1;

        CheckMovement(dir);
        if (v.y > 0.5f && !isJumping)
        {
            Jump();
            isJumping = true;
        }
        else isJumping = false;
    }

    void CheckMovement(Vector2 dir)
    {
        horizontalMovement = dir.x;
        

        if (dir.y > 0 && !petal && magnetized)
        {
            rb.linearVelocityY = maxSpeed;
        }
        
        if (dir.y < 0 && !petal && magnetized)
        {
            rb.linearVelocityY = -maxSpeed;
        }
    }

    void Jump()
    {
        if (petal)
        {
            if (!IsGrounded()) return;

            audioSource.PlaySound(SFX_Cs.SoundType.Jumping, 1f, 0.1f);

            rb.linearVelocityY = jumpForce;
        }
        else
        {
            if (!IsGrounded()) return;

            audioSource.PlaySound(SFX_Cs.SoundType.Jumping, 0.6f, 0.1f);

            rb.linearVelocityY = jumpForce;
        }
    }

    bool IsGrounded(bool checkOneLayer = false, string layerToCheck = null)
    {
        if (checkOneLayer)
        {
            LayerMask layerMask = LayerMask.GetMask(layerToCheck);
            return Physics2D.Raycast(rayCastPosition1.position, Vector2.down, 0.1f, layerMask) 
                                  || Physics2D.Raycast(rayCastPosition2.position, Vector2.down, 0.1f, layerMask)
                                  || Physics2D.Raycast(rayCastPosition3.position, Vector2.down, 0.1f, layerMask);
        }
        
        LayerMask mask = LayerMask.GetMask("Ground") + LayerMask.GetMask("Box");
        if (petal) mask += LayerMask.GetMask("Cloud") + LayerMask.GetMask("Metal") + LayerMask.GetMask("Water");
        if (!petal) mask += LayerMask.GetMask("Petal");

        return Physics2D.Raycast(rayCastPosition1.position, Vector2.down, 0.1f, mask) 
               || Physics2D.Raycast(rayCastPosition2.position, Vector2.down, 0.1f, mask)
               || Physics2D.Raycast(rayCastPosition3.position, Vector2.down, 0.1f, mask);
    }
    
    IEnumerator SpringJump(GameObject springTop)
    {
        Animator animator = springTop.GetComponent<Animator>();

        audioSource.PlaySound(SFX_Cs.SoundType.Spring, 1f, 0);
        rb.linearVelocity = springTop.transform.up * springForce;
        animator.SetBool("Press", true);
        
        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Press", false);
        
        yield return null;
    }
    
    private IEnumerator Shake(float duration, float amount)
    {
        float elapsed = 0.0f;
        Vector3 startPos = cameraStartingPos; 

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * amount;
            
            Camera.main.transform.localPosition = startPos + randomOffset;
            
            elapsed += Time.deltaTime;

            yield return null;
        }
        
        Camera.main.transform.localPosition = cameraStartingPos;
        currentShakeCoroutine = null;
    }

    public IEnumerator Death()
    {
        gameObject.transform.localScale = Vector3.zero;
        startingLevel = true;
        audioSource.PlaySound(SFX_Cs.SoundType.Death, 1f, 0f);
        while (audioSource.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (IsGrounded() && petal && other.gameObject.layer == 6)
        {
            audioSource.PlaySound(SFX_Cs.SoundType.PetalLanding, 1, 0.1f);
        }
        if (IsGrounded() && petal && other.gameObject.layer == 4)
        {
            audioSource.PlaySound(SFX_Cs.SoundType.Splash, 1, 0.1f);
        }

        if (IsGrounded() && !petal && other.gameObject.layer == 6)
        {
            audioSource.PlaySound(SFX_Cs.SoundType.MetalLanding, 0.6f, 0.1f);
            
            if (currentShakeCoroutine != null)
            {
                StopCoroutine(currentShakeCoroutine);
                Camera.main.transform.localPosition = cameraStartingPos;
            }

            currentShakeCoroutine = StartCoroutine(Shake(0.1f, 0.2f));
        }

        if (IsGrounded(true, "Petal") && !petal && other.gameObject.layer == 7)
        {
            float xdiff = other.transform.position.x - transform.position.x;
            if (xdiff > 0)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(7, 0), ForceMode2D.Impulse);
            }
            if (xdiff < 0)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-7, 0), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spring") && !other.gameObject.GetComponent<Animator>().GetBool("Press"))
        {
            StartCoroutine(SpringJump(other.gameObject));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Void") && !isDead)
        {
            isDead = true;
            StartCoroutine(Death());
        }
    }
}
