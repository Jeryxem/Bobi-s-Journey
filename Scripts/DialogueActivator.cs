using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueActivator : MonoBehaviour
{
    [TextArea(3,10)]
    public string[] lines;
    public Sprite[] portraitImage;

    private bool activateDialogue;
    public bool isNPC;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activateDialogue && Input.GetKeyDown(KeyCode.Space) && !DialogueManager.instance.dialogBox.activeInHierarchy && PlayerController.instance.playerCanMove)
        {
            DialogueManager.instance.ShowDialogue(lines, isNPC);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            activateDialogue = true;
            DialogueManager.instance.dialogueActivator = this;
            collision.GetComponent<PlayerController>().indicatorSpace.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            activateDialogue = false;
            collision.GetComponent<PlayerController>().indicatorSpace.SetActive(false);
        }
    }

    public Sprite[] GetPortraitImg()
    {
        return portraitImage;
    }
}
