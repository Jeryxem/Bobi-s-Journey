using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    public bool canTrade = false;
    public GameObject[] applesTraded;
    public int applesTradedCount;

    // Start is called before the first frame update
    void Start()
    {
        if(DialogueManager.instance.phaseCount <= 9)
        {
            gameObject.SetActive(false);
        }

        applesTradedCount = GameManager.instance.applesGivenToTrader;
        HideApple();
        ShowApple();

        // To progress story - phase count at 10 after tutorial match
        if(DialogueManager.instance.phaseCount == 10)
        {
            DialogueManager.instance.phaseCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U) && !DialogueManager.instance.dialogBox.activeInHierarchy && PlayerController.instance.playerCanMove && canTrade)
        {
            switch (applesTradedCount)
            {
                case 0:
                    break;
                case 1:
                    // Get Item 1
                    GameManager.instance.AddItem("Mushroom");
                    break;
                case 2:
                    // Get Item 2 
                    if(DialogueManager.instance.phaseCount <= 24)
                    {
                        GameManager.instance.AddItem("Turtle Dice");
                    }
                    else
                    {
                        GameManager.instance.AddItem("Power Dice");
                    }
                    break;
                default:
                    // Get Item 3
                    GameManager.instance.AddItem("Shield");
                    break;

            }

            GameManager.instance.applesGivenToTrader = 0;
            applesTradedCount = 0;
            HideApple();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canTrade = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canTrade = false;
        }
    }

    public void ShowApple()
    {
        for(int i = 0; i < applesTradedCount; i++)
        {
            applesTraded[i].SetActive(true);
        }
    }

    public void HideApple()
    {
        for (int i = 0; i < applesTraded.Length; i++)
        {
            applesTraded[i].SetActive(false);
        }
    }
}
