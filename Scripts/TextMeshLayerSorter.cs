using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextMeshLayerSorter : MonoBehaviour
{
    public string layerToPush;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = layerToPush;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
