using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalPickable : MonoBehaviour
{
    [Tooltip("Add scene name of item location")]
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.globalPickableItems.Contains(itemName))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //if(!GameManager.instance.globalPickableItems.Contains(itemName))
        //    GameManager.instance.globalPickableItems.Add(itemName);
    }
}
