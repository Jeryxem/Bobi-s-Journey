using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaEntrance : MonoBehaviour
{
    public string sceneTransitionName;

    // Start is called before the first frame update
    void Start()
    {
        if(sceneTransitionName == PlayerController.instance.sceneTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;

            StartCoroutine(DelayMovement());
        }
    }

    IEnumerator DelayMovement()
    {
        yield return new WaitForSeconds(1f);

        GameManager.instance.crossFadeIsActive = false;
    }
}
