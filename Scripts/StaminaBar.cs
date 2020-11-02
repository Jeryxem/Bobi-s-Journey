using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private int maxStamina;
    private int currentStamina;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxStamina = GameManager.instance.playerStats[0].maxStamina;
        currentStamina = GameManager.instance.playerStats[0].currentStamina;

        //currentStamina = maxStamina;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }
}
