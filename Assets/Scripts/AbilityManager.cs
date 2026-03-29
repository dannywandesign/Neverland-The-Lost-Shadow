using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    [Header("UI & Canvas References")]
    public GameObject popupCanvas;      
    public TextMeshProUGUI notificationText; 
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI glideText;
    public TextMeshProUGUI doubleJumpText;

    [Header("Gamble Settings")]
    [Range(0f, 1f)] public float abilityChance = 0.5f;

    [Header("Temporary Debuffs")]
    public float tempDebuffDuration = 5f;
    public float tempSpeedMultiplier = 0.6f;
    public float tempJumpMultiplier = 0.7f;
    public float tempGravityIncrease = 1.5f;
    public Color debuffColor = new Color(0.4f, 0.4f, 0.4f);

    [Header("Permanent Buffs")]
    public float permSpeedBoost = 0.1f;        
    public float permJumpBoost = 0.25f;         
    public float permDashBoost = 1.0f;         
    public float permGravityDecrease = 0.05f;  

    [Header("Base Stats")]
    public float baseSpeed;
    public float baseJump;
    public float baseGravity;

    [Header("State")]
    public bool isDashing = false;
    public int jumpsRemaining;
    private int activeGrowthCycles = 0; 

    [Header("Abilities Status")]
    public bool canDash = false;
    public bool canGlide = false;
    public bool canDoubleJump = false;

    [Header("Physics")]
    public float dashForce = 20f;
    public float dashTime = 0.2f;
    public float glideDrag = 10f;
    private float originalDrag;

    private PlayerMovement movement;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    void Awake() { instance = this; }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        baseSpeed = movement.speed;
        baseJump = movement.jumpForce;
        baseGravity = rb.gravityScale;
        originalDrag = rb.linearDamping;

        if (popupCanvas != null) popupCanvas.SetActive(false);
    }

    void Update()
    {
        if (movement.isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumpsRemaining = canDoubleJump ? 2 : 1;
        }

        if (!isDashing)
        {
            HandleDash();
            HandleGlide();
            HandleJump();
        }
    }

    public void GrowPlayer()
    {
        // 1. Permanent stats increase immediately
        baseSpeed += permSpeedBoost;
        baseJump += permJumpBoost;
        dashForce += permDashBoost;
        baseGravity = Mathf.Max(0.5f, baseGravity - permGravityDecrease);

        // 2. Roll for ability
        string unlockedAbilityName = "";
        if (Random.value <= abilityChance)
        {
            unlockedAbilityName = TryUnlockRandomAbility();
        }

        // 3. Start the visual/debuff cycle
        StartCoroutine(EvolutionCycle(unlockedAbilityName));
    }

    private string TryUnlockRandomAbility()
    {
        List<string> locked = new List<string>();
        if (!canDash) locked.Add("Dash");
        if (!canGlide) locked.Add("Glide");
        if (!canDoubleJump) locked.Add("Double Jump");

        if (locked.Count > 0)
        {
            string picked = locked[Random.Range(0, locked.Count)];

            if (picked == "Dash") { canDash = true; if(dashText) dashText.color = Color.green; }
            else if (picked == "Glide") { canGlide = true; if(glideText) glideText.color = Color.green; }
            else if (picked == "Double Jump") { canDoubleJump = true; if(doubleJumpText) doubleJumpText.color = Color.green; }
            
            return picked;
        }
        return "";
    }

    IEnumerator EvolutionCycle(string unlockedAbility)
    {
        activeGrowthCycles++;
        
        if (popupCanvas != null) popupCanvas.SetActive(true);

        // --- PHASE 1: THE DEBUFF (RED TEXT) ---
        movement.speed = baseSpeed * tempSpeedMultiplier;
        movement.jumpForce = baseJump * tempJumpMultiplier;
        rb.gravityScale = baseGravity + tempGravityIncrease;
        sr.color = debuffColor;

        if (notificationText != null)
        {
            notificationText.color = Color.red;
            // We keep the classic debuff text here
            notificationText.text = "GROWING PAINS...\nYou feel heavy.";
        }

        // Wait for the growing pains to finish
        yield return new WaitForSeconds(tempDebuffDuration);

        activeGrowthCycles--;

        // --- PHASE 2: THE MATURITY (CYAN/GREEN TEXT) ---
        // Only restore movement stats if this is the last/only active growth timer
        if (activeGrowthCycles <= 0)
        {
            activeGrowthCycles = 0;
            movement.speed = baseSpeed;
            movement.jumpForce = baseJump;
            rb.gravityScale = baseGravity;
            sr.color = Color.white;
        }

        // Always show the results of THIS specific growth click
        if (notificationText != null)
        {
            notificationText.color = Color.cyan;
            string resultMsg = "MATURED!\nStats Permanently Up";

            // If THIS specific click gave us a skill, add it to the message
            if (!string.IsNullOrEmpty(unlockedAbility))
            {
                notificationText.color = Color.green; // Turn green for success
                resultMsg += "\nUNLOCKED: " + unlockedAbility.ToUpper() + "!";
            }

            notificationText.text = resultMsg;

            // Keep the success message visible for a moment
            yield return new WaitForSeconds(3f);
            
            // Only hide the canvas if there aren't new growth cycles happening
            if (activeGrowthCycles <= 0)
            {
                popupCanvas.SetActive(false);
            }
        }
    }

    // --- MOVEMENT LOGIC (Restored) ---
    void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Jump")) && jumpsRemaining > 0)
        {
            jumpsRemaining--;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, movement.jumpForce);
            if(anim) anim.SetTrigger("isJumping");
        }
    }

    void HandleDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.Q) && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        float dashDir = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDir * dashForce, 0);
        float currentGrav = rb.gravityScale;
        rb.gravityScale = 0; 
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = currentGrav; 
        isDashing = false;
    }

    void HandleGlide()
    {
        if (canGlide && Input.GetKey(KeyCode.Space) && rb.linearVelocity.y < 0)
        {
            rb.linearDamping = glideDrag;
        }
        else
        {
            rb.linearDamping = originalDrag;
        }
    }
}