using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Windows and Pointers")]
    public int pointerIndex;
    public GameObject[] currentPointer;
    public GameObject theMenu, menuInstruction, itemsWindow, statsWindow, equipItemWindow, useItemWindow, saveWindow;
  
    private PlayerStats[] playerStats;

    [Header("Player Stats Window")]
    public Text nameTxt, emotionTxt, enduranceTxt, staminaTxt, agilityTxt, strTxt, defTxt, itemSlot1Txt, itemSlot2Txt;
    public bool secondaryWindowActive = false;

    [Header("Inventory Window and Selected Items")]
    public GameObject[] inventoryItems;
    public GameObject[] selectedItemBorders;
    public int selectedItem;
    private bool buttonInputConstraint = false;

    //public string selectedItem;
    //public Item activeItem;
    public Text itemName, itemDescription, equippedItemSlot1, equippedItemSlot2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (theMenu.activeInHierarchy)
            {
                currentPointer[pointerIndex].gameObject.SetActive(false);
                theMenu.SetActive(false);
                menuInstruction.SetActive(false);
                GameManager.instance.gameMenuIsOpen = false;

                // Close all secondary window
                selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white; // Reset Highlighted border
                useItemWindow.SetActive(false);
                equipItemWindow.SetActive(false);
                itemsWindow.SetActive(false);
                statsWindow.SetActive(false);
                secondaryWindowActive = false;
                buttonInputConstraint = false;
                saveWindow.SetActive(false);
            }
            else
            {
                if (!DialogueManager.instance.dialogBox.activeInHierarchy && PlayerController.instance.playerCanMove)
                {
                    theMenu.SetActive(true);
                    menuInstruction.SetActive(true);
                    pointerIndex = 0;
                    UpdateMenuUI(pointerIndex);
                    GameManager.instance.gameMenuIsOpen = true;
                }
            }
            AudioManager.instance.PlaySFX(1);
        }

        MenuControl();
        HighlightSelectedItem();
    }

    public void UpdateMenuUI(int menuPointer)
    {
        playerStats = GameManager.instance.playerStats;

        currentPointer[menuPointer].gameObject.SetActive(true);

        nameTxt.text = playerStats[0].charName.ToString();
        emotionTxt.text = playerStats[0].emotion.ToString();
        enduranceTxt.text = playerStats[0].maxEndurance.ToString();
        staminaTxt.text = (playerStats[0].maxStamina / 10).ToString();
        agilityTxt.text = playerStats[0].agility.ToString();
        strTxt.text = playerStats[0].strength.ToString();
        defTxt.text = playerStats[0].defence.ToString();
        
        if(playerStats[0].equippedSlot1.ToString() == "")
        {
            itemSlot1Txt.text = "None";
            equippedItemSlot1.text = "None";
        }
        else
        {
            itemSlot1Txt.text = playerStats[0].equippedSlot1.ToString();
            equippedItemSlot1.text = playerStats[0].equippedSlot1.ToString();
        }
        if (playerStats[0].equippedSlot2.ToString() == "")
        {
            itemSlot2Txt.text = "None";
            equippedItemSlot2.text = "None";
        }
        else
        {
            itemSlot2Txt.text = playerStats[0].equippedSlot2.ToString();
            equippedItemSlot2.text = playerStats[0].equippedSlot2.ToString();
        }
    }

    public void MenuControl()
    {
        if (theMenu.activeInHierarchy)
        {
            // Limit pointer in menu when opening secondary menu window
            if (!secondaryWindowActive)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentPointer[pointerIndex].gameObject.SetActive(false);
                    if (pointerIndex >= 4)
                    {
                        pointerIndex = 0;
                    }
                    else
                    {
                        pointerIndex++;
                    }
                    UpdateMenuUI(pointerIndex);
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentPointer[pointerIndex].gameObject.SetActive(false);
                    if (pointerIndex <= 0)
                    {
                        pointerIndex = 4;
                    }
                    else
                    {
                        pointerIndex--;
                    }
                    UpdateMenuUI(pointerIndex);
                    AudioManager.instance.PlaySFX(1);
                }
            }

            // Opens secondary menu window
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                // Enable secondary windows
                switch (pointerIndex)
                {
                    case 0: // Items
                        if (!itemsWindow.activeInHierarchy)
                        {
                            itemsWindow.SetActive(true);
                            ShowInverntoryItems();
                            selectedItem = 0;
                            selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.green;
                        }
                        break;
                    case 1: // Stats
                        statsWindow.SetActive(true);
                        break;
                    case 2: // Save
                        saveWindow.SetActive(true);
                        GameManager.instance.SaveGame();
                        break;
                    case 3: // Load
                        SceneManager.LoadScene("LoadingScene");
                        GameManager.instance.gameMenuIsOpen = false;
                        break;
                    case 4: // Quit Game
                        Application.Quit();
                        break;
                }
                secondaryWindowActive = true;
                AudioManager.instance.PlaySFX(1);
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // Close secondary menu window
            {
                // Disable secondary windows
                switch (pointerIndex)
                {
                    case 0: // Items
                        selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white;
                        useItemWindow.SetActive(false);
                        equipItemWindow.SetActive(false);
                        itemsWindow.SetActive(false);
                        break;
                    case 1: // Stats
                        statsWindow.SetActive(false);
                        break;
                    case 2: // Save
                        saveWindow.SetActive(false);
                        break;
                    case 3: // Load
                    case 4: // Quit Game
                        break;
                }
                secondaryWindowActive = false;
                buttonInputConstraint = false;
                AudioManager.instance.PlaySFX(1);
            }

            // Use item window control
            if (useItemWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    // Use Item
                    GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).UseItem(selectedItem);
                    ShowInverntoryItems();
                    useItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    // Give Item
                    GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).GiveItem(selectedItem);
                    ShowInverntoryItems();
                    useItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    // Do Nothing/Back
                    useItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
            }

            // Equip item window control
            if (equipItemWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    // Equip Item to Slot 1
                    GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).EquipItemSlot1(selectedItem);
                    equippedItemSlot1.text = GameManager.instance.playerStats[0].equippedSlot1.ToString();
                    ShowInverntoryItems();
                    equipItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    // Equip Item to Slot 2
                    GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).EquipItemSlot2(selectedItem);
                    equippedItemSlot2.text = GameManager.instance.playerStats[0].equippedSlot2.ToString();
                    ShowInverntoryItems();
                    equipItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    // Do Nothing/Back
                    equipItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
            }

            // Unequip all items
            if (itemsWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    // Remove previous equiped item stats
                    if (GameManager.instance.playerStats[0].equippedSlot1 != "")
                    {
                        GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot1).AffectedStatsChange(false);
                    }

                    if (GameManager.instance.playerStats[0].equippedSlot2 != "")
                    {
                        GameManager.instance.GetItemDetails(GameManager.instance.playerStats[0].equippedSlot2).AffectedStatsChange(false);
                    }

                    GameManager.instance.UnequipItem();
                    equippedItemSlot1.text = GameManager.instance.playerStats[0].equippedSlot1.ToString();
                    equippedItemSlot2.text = GameManager.instance.playerStats[0].equippedSlot2.ToString();
                    ShowInverntoryItems();
                    equipItemWindow.SetActive(false);
                    AudioManager.instance.PlaySFX(1);
                }
            }
        }
    }

    // Update inventory sprites
    public void ShowInverntoryItems()
    {
        GameManager.instance.SortItems();

        for(int i = 0; i < inventoryItems.Length; i++)
        {
            if(GameManager.instance.itemsHeld[i] != "")
            {
                inventoryItems[i].SetActive(true);
                inventoryItems[i].GetComponent<Image>().sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).sprite;
            }
            else
            {
                inventoryItems[i].SetActive(false);
            }
        }
    }

    public void HighlightSelectedItem()
    {
        if (itemsWindow.activeInHierarchy)
        {
            if (!useItemWindow.activeInHierarchy && !equipItemWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white;
                    if (selectedItem == selectedItemBorders.Length - 1)
                    {
                        selectedItem = 0;
                    }
                    else
                    {
                        selectedItem++;
                    }
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.green;
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white;
                    if (selectedItem == 0)
                    {
                        selectedItem = selectedItemBorders.Length - 1;
                    }
                    else
                    {
                        selectedItem--;
                    }
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.green;
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white;
                    if (selectedItem >= 12 && selectedItem <= 17)
                    {
                        selectedItem -= 12;
                    }
                    else
                    {
                        selectedItem += 6;
                    }
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.green;
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.white;
                    if (selectedItem >= 0 && selectedItem <= 5)
                    {
                        selectedItem += 12;
                    }
                    else
                    {
                        selectedItem -= 6;
                    }
                    selectedItemBorders[selectedItem].GetComponent<Image>().color = Color.green;
                    AudioManager.instance.PlaySFX(1);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    UseInventoryItem();
                    AudioManager.instance.PlaySFX(1);
                }
            }

            // Update text and description
            if(GameManager.instance.itemsHeld[selectedItem] != "")
            {
                itemName.text = GameManager.instance.itemsHeld[selectedItem];
                itemDescription.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).description;
            }
            else
            {
                itemName.text = "";
                itemDescription.text = "";
            }
                
        }
    }

    public void UseInventoryItem()
    {
        // Button constraint to prevent double input of Keycode Space
        if (buttonInputConstraint)
        {
            if (GameManager.instance.itemsHeld[selectedItem] != "")
            {
                if (GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).isItem)
                {
                    useItemWindow.SetActive(true);
                }
                if (GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[selectedItem]).isEquipment)
                {
                    equipItemWindow.SetActive(true);
                }
            }
        }
        buttonInputConstraint = true;
    }
}
