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

    [HideInInspector] public float horizontal; 
    public bool isGrounded; 
    private bool isAttacking;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");

        if (AbilityManager.instance != null && AbilityManager.instance.isDashing) return;

        // --- UPDATED ATTACK LOGIC (Mouse or F key) ---
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F)) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }

        // Movement Animations and Flipping
        bool isMoving = Mathf.Abs(horizontal) > 0.1f;
        anim.SetBool("isRunning", isMoving);

        if (horizontal > 0) 
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (horizontal < 0) 
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        if (AbilityManager.instance != null && AbilityManager.instance.isDashing) return;
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
}