using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public enum DebuffType { Slow, Heavy, Stiff }

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    [Header("Unlocks")]
    public bool canDash = false;
    public bool canGlide = false;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashCooldown = 1f;
    public bool isDashing = false; 
    private float lastDashTime;

    [Header("Glide Settings")]
    public float glideGravity = 0.5f;
    private float originalGravity;

    [Header("Permanent Debuffs")]
    public float popupDisplayTime = 1f; 
    public GameObject popupObject; 
    public TextMeshProUGUI popupText; 

    private int slowStackCount = 0;
    private int gravityStackCount = 0;
    private int jumpStackCount = 0;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim; 
    private PlayerMovement movement;
    private float baseSpeed;
    private float baseJump;

    void Awake() { instance = this; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
        movement = GetComponent<PlayerMovement>();
        originalGravity = rb.gravityScale;
        baseSpeed = movement.speed;
        baseJump = movement.jumpForce;

        if(popupObject != null) popupObject.SetActive(false);
    }

    void Update()
    {
        // --- RESET LOGIC ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetWorld();
        }

        if (!isDashing)
        {
            HandleDash();
            HandleGlide();
        }
    }

    void HandleDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.Q) && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        if (anim != null) anim.SetTrigger("Dash");
        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0); 
        yield return new WaitForSeconds(0.2f); 
        isDashing = false;
    }

    void HandleGlide()
    {
        if (canGlide && rb.linearVelocity.y < 0 && Input.GetKey(KeyCode.Space))
        {
            rb.gravityScale = glideGravity;
        }
        else
        {
            ApplyDebuffEffects();
        }
    }

    public void BecomeMature()
    {
        int randomChoice = Random.Range(0, 3); 
        if (randomChoice == 0) { slowStackCount++; ShowPopup("You act more mature. (Speed -20%)"); }
        else if (randomChoice == 1) { gravityStackCount++; ShowPopup("The world feels heavier. (Gravity +50%)"); }
        else { jumpStackCount++; ShowPopup("Your joints feel stiff. (Jump -15%)"); }
        
        ApplyDebuffEffects();
    }

    void ApplyDebuffEffects()
    {
        movement.speed = baseSpeed * Mathf.Max(0.1f, 1f - (slowStackCount * 0.2f));
        rb.gravityScale = originalGravity * (1f + (gravityStackCount * 0.5f));
        movement.jumpForce = baseJump * Mathf.Max(0.1f, 1f - (jumpStackCount * 0.15f));

        if (slowStackCount + gravityStackCount + jumpStackCount > 0)
            sr.color = new Color(0.75f, 0.75f, 0.75f);
    }

    void ShowPopup(string message)
    {
        if (popupObject != null)
        {
            popupObject.SetActive(true);
            popupText.text = message;
            StopCoroutine("HidePopupAfterDelay");
            StartCoroutine(HidePopupAfterDelay(popupDisplayTime)); 
        }
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popupObject != null) popupObject.SetActive(false);
    }

    public void ResetWorld()
    {
        // Reloads the current scene, clearing shards, enemies, and debuff stacks
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        // Safety: ensure time is moving (useful if you ever add a pause menu)
        Time.timeScale = 1f;
    }
}