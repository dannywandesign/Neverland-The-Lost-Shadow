using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class LanternGlow : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D playerLight;
    public float normalRadius = 1.5f;
    public float glowRadius = 8f;
    public float transitionSpeed = 5f;

    [Header("Battery Settings")]
    public float maxFuel = 5f;
    public float rechargeRate = 1.2f; 
    
    [Header("UI References")]
    public GameObject warningPopup; 

    private float currentFuel;
    private bool isOverheated = false;

    void Start()
    {
        currentFuel = maxFuel;
        if (warningPopup != null) warningPopup.SetActive(false);
    }

    void Update()
    {
        if (isOverheated)
        {
            if (currentFuel < maxFuel)
            {
                currentFuel += Time.deltaTime * rechargeRate;
                // FORCE THE POPUP ON
                if (warningPopup != null && !warningPopup.activeSelf) 
                {
                    warningPopup.SetActive(true);
                    Debug.Log("Warning Popup Activated!"); 
                }
            }
            else
            {
                isOverheated = false;
                currentFuel = maxFuel;
                if (warningPopup != null) warningPopup.SetActive(false);
            }
        }

        bool isTryingToGlow = (Input.GetMouseButton(1) || Input.GetKey(KeyCode.G)) && !isOverheated;
        float targetRadius = isTryingToGlow ? glowRadius : normalRadius;

        if (isTryingToGlow)
        {
            currentFuel -= Time.deltaTime;
            if (currentFuel <= 0)
            {
                currentFuel = 0;
                isOverheated = true;
            }
        }
        else if (!isOverheated && currentFuel < maxFuel)
        {
            currentFuel += Time.deltaTime * rechargeRate;
        }

        if (playerLight != null)
        {
            playerLight.pointLightOuterRadius = Mathf.Lerp(playerLight.pointLightOuterRadius, targetRadius, transitionSpeed * Time.deltaTime);
            playerLight.color = isOverheated ? Color.red : (isTryingToGlow ? new Color(1f, 0.9f, 0.5f) : Color.white);
        }
    }
}