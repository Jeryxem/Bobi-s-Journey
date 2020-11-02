using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject target;
    private float moveSpeed = 4f;
    public GameObject explosionEffect;
    public float explosionForce;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == target.transform.position)
            {
                Instantiate(explosionEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name != "BattleCharRival" && collision.gameObject.name != "BattleCharPlayer")
        {
            // This Add destroy effect
            Instantiate(explosionEffect, transform.position, transform.rotation);

            // The Pushback explosion force of bombs to player
            transform.position = Vector2.MoveTowards(collision.transform.position, transform.position, -1f * Time.deltaTime * explosionForce);
            Destroy(gameObject);
        }
    }
}
