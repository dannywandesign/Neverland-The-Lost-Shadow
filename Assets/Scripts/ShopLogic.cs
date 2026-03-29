using UnityEngine;

public class ShopLogic : MonoBehaviour
{
    public void PressGrowButton()
    {
        // 1. Tell Peter to start growing/gambling
        if (AbilityManager.instance != null)
        {
            AbilityManager.instance.GrowPlayer();
        }

        // 2. Close the shop immediately
        CloseShop();
    }

    private void CloseShop()
    {
        // This looks for the GlobalShop script on the parent object to turn the UI off
        if (GetComponentInParent<GlobalShop>() != null)
        {
            GetComponentInParent<GlobalShop>().ToggleShop();
        }
    }
}