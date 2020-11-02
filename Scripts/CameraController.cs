using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CameraController : MonoBehaviour
{
    public Transform cameraTarget;
    public Tilemap theMap;
    private Vector3 mapBottomLeftLimit, mapTopRightLimit;
    private float halfHeight, halfWidth;

    public int musicToPlay;
    private bool musicStarted;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraTarget == null)
            cameraTarget = PlayerController.instance.transform;

        // Keeps the camera inside the bounds of aspect ratio
        if(theMap != null)
        {
            halfHeight = Camera.main.orthographicSize;
            halfWidth = halfHeight * Camera.main.aspect;
            mapBottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
            mapTopRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

            PlayerController.instance.SetMapBounds(theMap.localBounds.min, theMap.localBounds.max);
        }
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, transform.position.z);

        // Keep the camera inside the bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapBottomLeftLimit.x, mapTopRightLimit.x),
                                         Mathf.Clamp(transform.position.y, mapBottomLeftLimit.y, mapTopRightLimit.y),
                                         transform.position.z);

        // Music change when switching scene
        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}
