using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement, previousMovement;
    public string sceneTransitionName;
    public GameObject indicatorSpace;

    // This allows only one instance of player that exist
    public static PlayerController instance;

    private Vector3 mapBottomLeftLimit, mapTopRightLimit;

    public bool playerCanMove = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCanMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); // Left-Right input
            movement.y = Input.GetAxisRaw("Vertical"); // Up-Down input

            // This controls the Diagonal movement speed
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
            {
                moveSpeed = 4f;
                previousMovement.x = movement.x;
                previousMovement.y = movement.y;
            } 
            else
            {
                moveSpeed = 5f;

                if (Mathf.Abs(movement.x) > 0.01f)
                {
                    previousMovement.x = movement.x;
                    previousMovement.y = movement.y;
                }
                if (Mathf.Abs(movement.y) > 0.01f)
                {
                    previousMovement.y = movement.y;
                }
            }

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
            animator.SetFloat("Previous Horizontal", previousMovement.x);
            animator.SetFloat("Previous Vertical", previousMovement.y);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapBottomLeftLimit.x, mapTopRightLimit.x),
                                             Mathf.Clamp(transform.position.y, mapBottomLeftLimit.y, mapTopRightLimit.y),
                                             transform.position.z);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void FixedUpdate()
    {
        if (playerCanMove)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        else
            rb.velocity = Vector2.zero;
    }

    // This prevents player to walk outside the bounds of the map
    public void SetMapBounds(Vector3 bottomLeft, Vector3 topRight)
    {
        mapBottomLeftLimit = bottomLeft + new Vector3(0.5f, 1f, 0f);
        mapTopRightLimit = topRight + new Vector3(-0.5f, -1f, 0f);
    }
}
