using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleNPC : MonoBehaviour
{
    private int currentPhaseCount;
    public GameObject[] battleNPCList;

    // Start is called before the first frame update
    void Start()
    {
        currentPhaseCount = DialogueManager.instance.phaseCount;

        for(int i = 0; i < battleNPCList.Length; i++)
        {
            if(battleNPCList[i] != null)
                battleNPCList[i].SetActive(false);
        }

        // phase count at 10 after tutorial match
        switch (currentPhaseCount)
        {
            case 1:
                if(battleNPCList[0] != null)
                    battleNPCList[0].SetActive(true);
                break;
            case 10: // Won tutorial
                GameManager.instance.playerStats[0].emotion = "Feeling overwhelmingly happy!";
                FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();
                if (battleNPCList[11] != null)
                    if (battleNPCList[1] != null)
                    battleNPCList[1].SetActive(true);
                break;
            case 11:
                if (battleNPCList[2] != null)
                    battleNPCList[2].SetActive(true);
                break;
            case 15:
                if (battleNPCList[3] != null)
                    battleNPCList[3].SetActive(true);
                break;
            case 16:
            case 17:
                if (battleNPCList[4] != null)
                    battleNPCList[4].SetActive(true);
                break;
            case 18:
                if (battleNPCList[5] != null)
                    battleNPCList[5].SetActive(true);
                break;
            case 19:
                if (battleNPCList[6] != null)
                    battleNPCList[6].SetActive(true);
                break;
            case 20:
                if (battleNPCList[7] != null)
                    battleNPCList[7].SetActive(true);
                break;
            case 21:
                if (battleNPCList[8] != null)
                    battleNPCList[8].SetActive(true);
                break;
            case 23:
                if (battleNPCList[9] != null)
                    battleNPCList[9].SetActive(true);
                break;
            case 24: // 2nd battle
                if (battleNPCList[10] != null)
                    battleNPCList[10].SetActive(true);
                break;
            case 26: // 2nd battle won
                GameManager.instance.playerStats[0].emotion = "Feeling overwhelmingly happy!";
                FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();
                if (battleNPCList[11] != null)
                    battleNPCList[11].SetActive(true);
                break;
            case 27: // 3rd battle
                if (battleNPCList[12] != null)
                    battleNPCList[12].SetActive(true);
                break;
            case 29: // 3rd battle won
                if (battleNPCList[13] != null)
                    battleNPCList[13].SetActive(true);
                break;
            case 30: // Hero appears
                if (battleNPCList[14] != null)
                    battleNPCList[14].SetActive(true);
                break;
            case 31: // Hero appears
                if (battleNPCList[15] != null)
                    battleNPCList[15].SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (DialogueManager.instance.phaseCount == 15 
                && DialogueManager.instance.dialogBox.activeInHierarchy 
                && gameObject.name == "NPCs")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;
                GameManager.instance.playerStats[0].emotion = "Feeling dejected.";
                FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();
            }

            if (DialogueManager.instance.phaseCount == 16 
                && !DialogueManager.instance.dialogBox.activeInHierarchy)
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[4] != null)
                    battleNPCList[4].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 17 
                && DialogueManager.instance.dialogBox.activeInHierarchy 
                && gameObject.name == "NPC3")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;
                GameManager.instance.playerStats[0].emotion = "Feeling hopeful.";
                FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();
            }

            if (DialogueManager.instance.phaseCount == 18 
                && !DialogueManager.instance.dialogBox.activeInHierarchy)
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[5] != null)
                    battleNPCList[5].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 20 
                && DialogueManager.instance.dialogBox.activeInHierarchy 
                && gameObject.name == "NPC4")
            {
                for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                {
                    if (GameManager.instance.itemsHeld[i] == "Hero's Apple")
                    {
                        GameManager.instance.itemsHeld[i] = "";
                    }
                }

                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[7] != null)
                    battleNPCList[7].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 21 
                && DialogueManager.instance.dialogBox.activeInHierarchy 
                && gameObject.name == "NPCs")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[8] != null)
                    battleNPCList[8].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 22
                && !DialogueManager.instance.dialogBox.activeInHierarchy
                && gameObject.name == "NPCs")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                GameManager.instance.playerStats[0].emotion = "Feeling excited!";
                FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[9] != null)
                    battleNPCList[9].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 23
                && DialogueManager.instance.dialogBox.activeInHierarchy
                && gameObject.name == "NPC5")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[10] != null)
                    battleNPCList[10].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 26
                && DialogueManager.instance.dialogBox.activeInHierarchy
                && gameObject.name == "NPC3")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[12] != null)
                    battleNPCList[12].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 29
                && DialogueManager.instance.dialogBox.activeInHierarchy
                && gameObject.name == "NPC3")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[14] != null)
                    battleNPCList[14].SetActive(true);
            }

            if (DialogueManager.instance.phaseCount == 30
                && DialogueManager.instance.dialogBox.activeInHierarchy
                && gameObject.name == "Hero")
            {
                DialogueManager.instance.phaseCount++;
                currentPhaseCount = DialogueManager.instance.phaseCount;

                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[15] != null)
                    battleNPCList[15].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (DialogueManager.instance.phaseCount == 19)
            {
                for (int i = 0; i < battleNPCList.Length; i++)
                {
                    if (battleNPCList[i] != null)
                        battleNPCList[i].SetActive(false);
                }

                if (battleNPCList[6] != null)
                    battleNPCList[6].SetActive(true);

                if(gameObject.name == "NPC4")
                {
                    DialogueManager.instance.phaseCount++;
                    currentPhaseCount = DialogueManager.instance.phaseCount;
                }
            }
        }
    }
}
