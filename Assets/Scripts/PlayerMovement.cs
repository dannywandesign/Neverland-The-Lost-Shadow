using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float jumpForce = 12f;
    public float attackDuration = 0.5f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject attackHitbox; 
    public AttackHitbox hitboxScript; 

    private float horizontal;
    private bool isGrounded;
    private bool isAttacking;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. Ground and Input Checks
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");

        // 2. DASH OVERRIDE: Stop regular movement logic if dashing
        if (AbilityManager.instance != null && AbilityManager.instance.isDashing)
        {
            return; // Skip the rest of Update while the dash burst is active
        }

        // 3. Jump Logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("isJumping");
        }

        // 4. Attack Logic
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }

        // 5. Movement Animations and Flipping
        bool isMoving = Mathf.Abs(horizontal) > 0.1f;
        anim.SetBool("isRunning", isMoving);

        if (horizontal > 0) 
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (horizontal < 0) 
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        // 6. DASH OVERRIDE: Prevent physics overwrite during dash
        if (AbilityManager.instance != null && AbilityManager.instance.isDashing)
        {
            return; // Allow the dash velocity from AbilityManager to persist
        }

        // Regular horizontal movement
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.3f); 
        
        if (hitboxScript != null) hitboxScript.CheckForKills();

        yield return new WaitForSeconds(attackDuration - 0.3f);

        attackHitbox.SetActive(false);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}