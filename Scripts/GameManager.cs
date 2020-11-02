using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerStats[] playerStats;

    public bool gameMenuIsOpen, dialogIsActive, crossFadeIsActive, battleIsActive;

    public string[] itemsHeld;
    public Item[] referenceItem;
    public int applesGivenToTrader = 0;
    public GameObject[] applesTraded;
    public List<string> globalPickableItems = new List<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameMenuIsOpen || dialogIsActive || crossFadeIsActive || battleIsActive)
        {
            PlayerController.instance.playerCanMove = false;
        } else
        {
            PlayerController.instance.playerCanMove = true;
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for(int i = 0; i < referenceItem.Length; i++)
        {
            if(referenceItem[i].itemName == itemToGrab)
            {
                return referenceItem[i];
            }
        }

        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if(itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    if(itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == "")
            {
                newItemPosition = i;
                i = itemsHeld.Length; // Found empty space hence stop searching in loop
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            bool itemExists = false;
            for(int i = 0; i < referenceItem.Length; i++)
            {
                if(referenceItem[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItem.Length; // Item exist hence stop searching in loop
                }
            }

            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
            } 
            else
            {
                Debug.LogError(itemToAdd + " does not exist in the game!");
            }
        }
    }

    public void RemoveItem(string itemToRemove, int selectedItem)
    {
        bool foundItem = false;
        
        if(itemsHeld[selectedItem] == itemToRemove)
        {
            foundItem = true;
        }

        if (foundItem)
        {
            itemsHeld[selectedItem] = "";
        }
    }

    public void UnequipItem()
    {
        if (instance.playerStats[0].equippedSlot1 != "")
        {
            instance.AddItem(instance.playerStats[0].equippedSlot1);
        }

        if (instance.playerStats[0].equippedSlot2 != "")
        {
            instance.AddItem(instance.playerStats[0].equippedSlot2);
        }

        instance.playerStats[0].equippedSlot1 = "";
        instance.playerStats[0].equippedSlot2 = "";
    }

    public void SaveGame()
    {
        // Scene and player position
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        // Player info / stats
        for(int i  = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentEndurance", playerStats[i].currentEndurance);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxEndurance", playerStats[i].maxEndurance);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentStamina", playerStats[i].currentStamina);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxCurrentStamina", playerStats[i].maxStamina);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetFloat("Player_" + playerStats[i].charName + "_Agility", playerStats[i].agility);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedSlot1", playerStats[i].equippedSlot1);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedSlot2", playerStats[i].equippedSlot2);
        }

        // Inventory data
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory" + i, itemsHeld[i]);
        }

        // Global item
        for (int i = 0; i < globalPickableItems.Count; i++)
        {
            PlayerPrefs.SetString("GlobalPickable" + i, globalPickableItems[i]);
        }

        // Phase count
        PlayerPrefs.SetInt("PhaseCount", DialogueManager.instance.phaseCount);
    }

    public void LoadGame()
    {
        // Scene and player position
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        // Player info / stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].currentEndurance = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentEndurance");
            playerStats[i].maxEndurance = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxEndurance");
            playerStats[i].currentStamina = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentStamina");
            playerStats[i].maxStamina = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxCurrentStamina");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].agility = PlayerPrefs.GetFloat("Player_" + playerStats[i].charName + "_Agility");
            playerStats[i].equippedSlot1 = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedSlot1");
            playerStats[i].equippedSlot2 = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedSlot2");

        }

        // Inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory" + i);
        }

        // Global item
        for (int i = 0; i < globalPickableItems.Count; i++)
        {
            globalPickableItems[i] = PlayerPrefs.GetString("GlobalPickable" + i);
        }

        // Phase count
        DialogueManager.instance.phaseCount = PlayerPrefs.GetInt("PhaseCount");
    }
}
