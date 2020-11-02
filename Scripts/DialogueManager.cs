using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogText;
    public GameObject dialogBox, portraitImg, portraitBorder, dialogChoicesBox;
    public DialogueActivator dialogueActivator;
    public int phaseCount = 0;
    public GameObject[] storyBox;

    [TextArea(3,10)]
    public string[] dialogLines;
    public int currentLine;
    public bool justStarted;

    public static DialogueManager instance;

    // BattleNPC - subject to change
    private GameObject temp;

    // Start is called before the first frame update
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

        //Story phase
        StoryMode();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: to be restruture - This manage StoryBox UI
        StoryMode();
        if(phaseCount < storyBox.Length)
        {
            if (storyBox[phaseCount] != null && storyBox[phaseCount].activeInHierarchy)
            {
                GameManager.instance.dialogIsActive = true;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    NextPhase();
                }
            }
        }

        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Store temp string of last lines (for choice box usage)
                string temp = dialogLines[currentLine];

                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        GameManager.instance.dialogIsActive = false;

                        // Check Choice Box
                        if (temp.Contains("Ready for the tourney?"))
                        {
                            ChoiceDialogToogle();
                        }

                        // Check if you get item
                        if (temp.Contains("(Alright! I've got an apple!)"))
                        {
                            GameManager.instance.AddItem("Hero's Apple");
                            phaseCount++;
                            StoryMode();
                        }
                    }
                    else
                    {
                        CheckIfPortrait(dialogueActivator);

                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
                AudioManager.instance.PlaySFX(3);
            }
        }

        //Yes/No Choice box is active
        if (dialogChoicesBox.activeInHierarchy)
        {
            PlayerController.instance.playerCanMove = false;

            if (Input.GetKeyUp(KeyCode.Y))
            {
                // TODO: Move battleNPC away
                dialogChoicesBox.SetActive(false);
                StartCoroutine(MoveNPCAway());
                switch (phaseCount)
                {
                    case 24:
                        GameObject.Find("AreaExit BattleArena").SetActive(false);
                        GameObject.Find("AreaExit BattleArena2").SetActive(true);
                        GameObject.Find("AreaExit BattleArena3").SetActive(false);
                        GameObject.Find("AreaExit BattleArena4").SetActive(false);
                        break;
                    case 27:
                        GameObject.Find("AreaExit BattleArena").SetActive(false);
                        GameObject.Find("AreaExit BattleArena2").SetActive(false);
                        GameObject.Find("AreaExit BattleArena3").SetActive(true);
                        GameObject.Find("AreaExit BattleArena4").SetActive(false);
                        break;
                    case 31:
                        GameObject.Find("AreaExit BattleArena").SetActive(false);
                        GameObject.Find("AreaExit BattleArena2").SetActive(false);
                        GameObject.Find("AreaExit BattleArena3").SetActive(false);
                        GameObject.Find("AreaExit BattleArena4").SetActive(true);
                        break;
                    default:
                        GameObject.Find("AreaExit BattleArena").SetActive(true);
                        GameObject.Find("AreaExit BattleArena2").SetActive(false);
                        GameObject.Find("AreaExit BattleArena3").SetActive(false);
                        GameObject.Find("AreaExit BattleArena4").SetActive(false);
                        break;
                }
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                PlayerController.instance.playerCanMove = true;
                dialogChoicesBox.SetActive(false);
            }
        }

        // This makes BattleNPC rotate/teleport away from scene
        if(temp != null)
        {
            temp.transform.position = Vector2.MoveTowards(temp.transform.position, new Vector2(temp.transform.position.x, temp.transform.position.y + 10f), 0.02f);
            temp.transform.Rotate(0, 10, 0);
        }
    }

    public void ShowDialogue(string[] newLines, bool isNPC)
    {
        dialogLines = newLines;
        currentLine = 0;

        CheckIfPortrait(dialogueActivator);

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        justStarted = true;
        portraitBorder.SetActive(isNPC);
        portraitImg.SetActive(isNPC);

        GameManager.instance.dialogIsActive = true;
    }

    // This function changes the portrait according to who is speaking in the dialogue
    public void CheckIfPortrait(DialogueActivator dialogueActivator)
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            if (dialogLines[currentLine].Contains("Player"))
                portraitImg.GetComponent<Image>().sprite = dialogueActivator.GetPortraitImg()[0];
            else if (dialogLines[currentLine].Contains("NPC"))
                portraitImg.GetComponent<Image>().sprite = dialogueActivator.GetPortraitImg()[1];

            currentLine++;
        }
    }

    public void ChoiceDialogToogle()
    {
        dialogChoicesBox.SetActive(true);
    }

    public void StoryMode()
    {
        // phase count at 10 after tutorial match
        switch (phaseCount)
        {
            case 0:
                if (SceneManager.GetActiveScene().name == "A1")
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                if (SceneManager.GetActiveScene().name == "BattleScene" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 12:
                if (SceneManager.GetActiveScene().name == "A3" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 14:
                if (SceneManager.GetActiveScene().name == "A1" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 24:
                if (SceneManager.GetActiveScene().name == "BattleScene2" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 27:
                if (SceneManager.GetActiveScene().name == "BattleScene3" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            case 31:
                if (SceneManager.GetActiveScene().name == "BattleScene4" && storyBox[phaseCount] != null)
                {
                    storyBox[phaseCount].SetActive(true);
                    PlayerController.instance.playerCanMove = false;
                }
                break;
            default:
                break;
        }
    }

    public void NextPhase()
    {
        for (int i = 0; i < storyBox.Length; i++)
        {
            if(storyBox[i] != null)
                storyBox[i].SetActive(false);
        }

        if (phaseCount < storyBox.Length)
            phaseCount++;
        StoryMode();

        //TODO: Give movement only when dialog box not active
        if(phaseCount == 9 || phaseCount == 25 || phaseCount == 28 || phaseCount == 32)
        {
            FindObjectOfType<BattleCharControlPlayer>().moveSpeed = 2f;
            FindObjectOfType<BattleCharControlRival>().moveSpeed = 2f;
        }

        if (phaseCount == 13)
        {
            phaseCount++;
            SceneManager.LoadScene("A1");
            PlayerController.instance.transform.position = Vector3.zero;
            GameManager.instance.playerStats[0].emotion = "Feeling anxious...";
            FindObjectOfType<MenuManager>().emotionTxt.text = GameManager.instance.playerStats[0].emotion.ToString();
        }

        GameManager.instance.dialogIsActive = false;
    }

    IEnumerator MoveNPCAway()
    {
        GameManager.instance.dialogIsActive = true;
        temp = GameObject.Find("BattleNPC");
        yield return new WaitForSeconds(2f);
        Destroy(temp);
        GameManager.instance.dialogIsActive = false;
    }
}
