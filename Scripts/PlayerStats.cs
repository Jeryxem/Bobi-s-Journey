using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public string charName;
    public string emotion;

    public int currentEndurance;
    public int maxEndurance;
    public int currentStamina;
    public int maxStamina;
    public float agility;
    public int strength;
    public int defence;
    public string equippedSlot1;
    public string equippedSlot2;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeEmotion(string newEmotion)
    {
        emotion = newEmotion;
    }
}
