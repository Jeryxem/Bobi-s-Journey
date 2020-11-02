using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialLoader : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public GameObject audioManager;
    public GameObject dialogManager;

    void Awake()
    {
        if (PlayerController.instance == null)
        {
            Instantiate(player);
        }

        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }

        if (DialogueManager.instance == null)
        {
            Instantiate(dialogManager);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
