using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string sceneNameToLoad;
    public string sceneTransitionName;
    public AreaEntrance theEntrance;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.sceneTransitionName = sceneTransitionName;
    }

    public void LoadNextLevel()
    {
        GameManager.instance.crossFadeIsActive = true;
        DialogueManager.instance.StoryMode();
        StartCoroutine(LoadLevel(sceneNameToLoad));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        PlayerController.instance.indicatorSpace.SetActive(false);

        SceneManager.LoadScene(sceneName);

        PlayerController.instance.sceneTransitionName = sceneTransitionName;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().indicatorSpace.SetActive(true);
            if (Input.GetKey(KeyCode.Space))
            {
                LoadNextLevel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().indicatorSpace.SetActive(false);
        }
    }
}
