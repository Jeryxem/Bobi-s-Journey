using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isEquipment;
    public bool canPickUp;

    [Header("Item Details")]
    public Sprite sprite;
    public string itemName;
    public string description;
    public int affectValueEnd, affectValueSta, affectValueStr, affectValueDef, affectValueAgi;
    public bool affectEndurance, affectStamina, affectStr, affectDef, affectAgi;
    public int phaseCountToAppear;

    // Start is called before the first frame update
    void Start()
    {
        if(phaseCountToAppear != DialogueManager.instance.phaseCount)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Pickup Items
        if(canPickUp && Input.GetKeyDown(KeyCode.Space) && PlayerController.instance.playerCanMove)
        {
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            if (!GameManager.instance.globalPickableItems.Contains(GetComponent<GlobalPickable>().itemName))
                GameManager.instance.globalPickableItems.Add(GetComponent<GlobalPickable>().itemName);
            Destroy(gameObject);
        }
    }

    public void UseItem(int selectedItems)
    {
        GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItems]).AffectedStatsChange(true);
        GameManager.instance.RemoveItem(GameManager.instance.itemsHeld[selectedItems], selectedItems);
    }

    public void GiveItem(int selectedItems)
    {
        if(FindObjectOfType<Trader>() != null)
        {
            GameObject[] applesTraded = GameManager.instance.applesTraded;
            if (applesTraded == null || FindObjectOfType<Trader>().canTrade == false)
            {
                // No trader in scenes notification - Music/Pop-up text
                Debug.LogError("Cant Trade");
            }
            else
            {
                GameManager.instance.RemoveItem(GameManager.instance.itemsHeld[selectedItems], selectedItems);
                GameManager.instance.applesGivenToTrader++;
                FindObjectOfType<Trader>().applesTradedCount++;
                FindObjectOfType<Trader>().ShowApple();
            }
        }
    }

    public void EquipItemSlot1(int selectedItems)
    {
        if (GameManager.instance.playerStats[0].equippedSlot1 != "") 
        {
            GameManager.instance.AddItem(GameManager.instance.playerStats[0].equippedSlot1);
            // Remove previous equiped item stats
            GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot1).AffectedStatsChange(false);
        }

        GameManager.instance.playerStats[0].equippedSlot1 = itemName;
        GameManager.instance.itemsHeld[selectedItems] = "";
        // Add new equiped item stats
        GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot1).AffectedStatsChange(true);
    }

    public void EquipItemSlot2(int selectedItems)
    {
        if (GameManager.instance.playerStats[0].equippedSlot2 != "")
        {
            GameManager.instance.AddItem(GameManager.instance.playerStats[0].equippedSlot2);
            // Remove previous equiped item stats
            GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot2).AffectedStatsChange(false);
        }

        GameManager.instance.playerStats[0].equippedSlot2 = itemName;
        GameManager.instance.itemsHeld[selectedItems] = "";
        // Add new equiped item stats
        GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot2).AffectedStatsChange(true);
    }

    // Affect the players stat changes from items --- TRUE = Increase, FALSE = decrease
    public void AffectedStatsChange(bool addingEffect) 
    {
        if (addingEffect)
        {
            if (affectEndurance)
            {
                GameManager.instance.playerStats[0].maxEndurance += affectValueEnd;
                if(GameManager.instance.playerStats[0].maxEndurance <= 0)
                {
                    GameManager.instance.playerStats[0].maxEndurance = 10;
                }
                GameManager.instance.playerStats[0].currentEndurance = GameManager.instance.playerStats[0].maxEndurance;
            }

            if (affectStamina)
            {
                GameManager.instance.playerStats[0].maxStamina += affectValueSta;
                GameManager.instance.playerStats[0].currentStamina = GameManager.instance.playerStats[0].maxStamina;
            }

            if (affectStr)
            {
                GameManager.instance.playerStats[0].strength += affectValueStr;
            }

            if (affectDef)
            {
                GameManager.instance.playerStats[0].defence += affectValueDef;
            }

            if (affectAgi)
            {
                GameManager.instance.playerStats[0].agility += affectValueAgi;
            }
        }
        else
        {
            if (affectEndurance)
            {
                GameManager.instance.playerStats[0].maxEndurance -= affectValueEnd;
                GameManager.instance.playerStats[0].currentEndurance = GameManager.instance.playerStats[0].maxEndurance;
            }

            if (affectStamina)
            {
                GameManager.instance.playerStats[0].maxStamina -= affectValueSta;
                GameManager.instance.playerStats[0].currentStamina = GameManager.instance.playerStats[0].maxStamina;
            }

            if (affectStr)
            {
                GameManager.instance.playerStats[0].strength -= affectValueStr;
            }

            if (affectDef)
            {
                GameManager.instance.playerStats[0].defence -= affectValueDef;
            }

            if (affectAgi)
            {
                GameManager.instance.playerStats[0].agility -= affectValueAgi;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canPickUp = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canPickUp = false;
    }
}
