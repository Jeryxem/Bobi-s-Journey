using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleCharControlPlayer : MonoBehaviour
{
    public GameObject itemToGet;
    public GameObject house;
    private int score;
    private GameObject target;
    private GameObject[] moreThanOneInScene;
    public GameObject heldItemApple, heldItemChest, heldItemMushroom, stunText, tiredText;
    public Text scoreText, enduranceTextAbove, staminaTextAbove, spaceText;
    public TextMesh enduranceText, strText, defText;
    public GameObject explosionEffect;
    public float explosionForce, restTime;
    public float battleStr, battleDef, moveSpeed;
    public GameObject victoryWindow;

    // Dropped item
    private GameObject dropObj;
    private Vector2 newPos;

    // Movement
    Vector2 movement;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(itemToGet == null)
        {
            score = 0;
        }

        battleStr = GameManager.instance.playerStats[0].strength;
        battleDef = GameManager.instance.playerStats[0].defence;
        GameManager.instance.playerStats[0].currentEndurance = GameManager.instance.playerStats[0].maxEndurance;

        // Control speed before match start
        moveSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Player win battle if score = 5
        if(score == 5)
        {
            StartCoroutine(Victory());
        }

        // Movement of player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if((movement.x != 0 || movement.y != 0) && moveSpeed != 0)
        {
            GameManager.instance.playerStats[0].currentStamina -= 1;
            if (GameManager.instance.playerStats[0].currentStamina <= 0)
            {
                StartCoroutine(StunForFewSecond());
                GameManager.instance.playerStats[0].currentStamina = 0;
            }
        }
        else
        {
            if (!stunText.activeInHierarchy)
            {
                GameManager.instance.playerStats[0].currentStamina += 3;
                if (GameManager.instance.playerStats[0].currentStamina > GameManager.instance.playerStats[0].maxStamina)
                {
                    GameManager.instance.playerStats[0].currentStamina = GameManager.instance.playerStats[0].maxStamina;
                }
            }
        }

        // Kick Bomb
        if (Input.GetKey(KeyCode.Space) && moveSpeed != 0)
        {
            spaceText.color = Color.yellow;
            FindBomb();
        }
        else
        {
            spaceText.color = Color.white;
            target = null;
        }

        // Update player's various battle stats
        scoreText.text = score.ToString();
        enduranceTextAbove.text = GameManager.instance.playerStats[0].currentEndurance.ToString();
        //staminaTextAbove.text = GameManager.instance.playerStats[0].currentStamina.ToString();
        enduranceText.text = GameManager.instance.playerStats[0].currentEndurance.ToString();
        strText.text = battleStr.ToString();
        defText.text = battleDef.ToString();

        //Drop item
        if(dropObj != null)
            dropObj.transform.position = Vector2.MoveTowards(dropObj.transform.position, newPos, Time.deltaTime * 2.5f);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.playerStats[0].currentStamina > 0)
            rb.MovePosition(rb.position + movement * (GameManager.instance.playerStats[0].agility / 10 * moveSpeed) * Time.fixedDeltaTime);
    }

    public void FindHouse()
    {
        if (GameObject.Find("House") != null)
        {
            target = GameObject.Find("House");
            GameManager.instance.playerStats[0].currentStamina -= 2;
        }
    }
    public void FindApple()
    {
        if (GameObject.FindGameObjectWithTag("Apple") != null && GameManager.instance.playerStats[0].currentStamina >= 2)
        {
            moreThanOneInScene = null;
            moreThanOneInScene = GameObject.FindGameObjectsWithTag("Apple");
            target = moreThanOneInScene[Random.Range(0, moreThanOneInScene.Length)];
            GameManager.instance.playerStats[0].currentStamina -= 2;
        }
        else
        {
            target = null;
        }
    }
    public void FindChest()
    {
        if (GameObject.FindGameObjectWithTag("Chest") != null && GameManager.instance.playerStats[0].currentStamina >= 2)
        {
            moreThanOneInScene = null;
            moreThanOneInScene = GameObject.FindGameObjectsWithTag("Chest");
            target = moreThanOneInScene[Random.Range(0, moreThanOneInScene.Length)];
            GameManager.instance.playerStats[0].currentStamina -= 2;
        }
        else
        {
            target = null;
        }
    }
    public void FindMushroom()
    {
        if (GameObject.FindGameObjectWithTag("Mushroom") != null && GameManager.instance.playerStats[0].currentStamina >= 2)
        {
            moreThanOneInScene = null;
            moreThanOneInScene = GameObject.FindGameObjectsWithTag("Mushroom");
            target = moreThanOneInScene[Random.Range(0, moreThanOneInScene.Length)];
            GameManager.instance.playerStats[0].currentStamina -= 2;
        }
        else
        {
            target = null;
        }
    }
    public void FindBomb()
    {
        if (GameObject.FindGameObjectWithTag("Bomb") != null && GameManager.instance.playerStats[0].currentStamina >= 0)
        {
            moreThanOneInScene = null;
            moreThanOneInScene = GameObject.FindGameObjectsWithTag("Bomb");
            float temp = Vector2.Distance(moreThanOneInScene[0].transform.position, transform.position);
            //TODO: Find closest bomb to player
            for (int i = 0; i < moreThanOneInScene.Length; i++)
            {
                float dist = Vector2.Distance(moreThanOneInScene[i].transform.position, transform.position);
                if (dist <= temp)
                {
                    temp = dist;
                    target = moreThanOneInScene[i];
                }
            }
            //target = moreThanOneInScene[Random.Range(0, moreThanOneInScene.Length)];
            GameManager.instance.playerStats[0].currentStamina -= 4;
        }
        else
        {
            target = null;
        }
    }
    public void FindRival()
    {
        if (GameObject.FindGameObjectWithTag("BattleCharRival") != null && GameManager.instance.playerStats[0].currentStamina >= 2)
        {
            target = GameObject.Find("BattleCharRival");
            GameManager.instance.playerStats[0].currentStamina -= 2;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This Add Score when items delivered
        if (collision.gameObject == house)
        {
            if (heldItemApple.activeInHierarchy && itemToGet.name == "Apple")
            {
                score++;
                heldItemApple.SetActive(false);
            }
            if (heldItemChest.activeInHierarchy && itemToGet.name == "Chest")
            {
                score++;
                heldItemChest.SetActive(false);
            }
            if (heldItemMushroom.activeInHierarchy && itemToGet.name == "Mushroom")
            {
                score++;
                heldItemMushroom.SetActive(false);
            }
        }

        // Hold item on contact
        if (!stunText.activeInHierarchy)
        {
            if (collision.gameObject.name == "Apple(Clone)" && !stunText.activeInHierarchy)
            {
                // If holding other item
                ItemDropped();

                Destroy(collision.gameObject);
                heldItemApple.SetActive(true);
            }
            else if (collision.gameObject.name == "Chest(Clone)" && !stunText.activeInHierarchy)
            {
                // If holding other item
                ItemDropped();

                Destroy(collision.gameObject);
                heldItemChest.SetActive(true);
            }
            else if (collision.gameObject.name == "Mushroom(Clone)" && !stunText.activeInHierarchy)
            {
                // If holding other item
                ItemDropped();

                Destroy(collision.gameObject);
                heldItemMushroom.SetActive(true);
            }
        }

        // Bomb functionality on contact
        if (collision.gameObject.name == "Bomb(Clone)")
        {
            if(target == collision.gameObject) 
            {
                // TODO: Kick bomb to rival if target is bomb
                float newPosXBomb, newPosYBomb;
                if (GameObject.Find("BattleCharRival").transform.position.x > transform.position.x)
                {
                    newPosXBomb = 1f;
                }
                else
                {
                    newPosXBomb = -1f;
                }
                if (GameObject.Find("BattleCharRival").transform.position.y > transform.position.y)
                {
                    newPosYBomb = 1f;
                }
                else
                {
                    newPosYBomb = -1f;
                }
                collision.gameObject.transform.position = new Vector2(transform.position.x + newPosXBomb, transform.position.y + newPosYBomb);
                collision.gameObject.GetComponent<Bomb>().target = GameObject.Find("BattleCharRival");
                target = null;
            }
            else
            {
                // This Add destroy effect and decrease endurance/stamina
                Instantiate(explosionEffect, collision.transform.position, collision.transform.rotation);
                GameManager.instance.playerStats[0].currentEndurance -= 50;
                StartCoroutine(StunForFewSecond());

                // The Pushback explosion force of bombs to player
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, -1f * Time.deltaTime * explosionForce);
                target = null;
                Destroy(collision.gameObject);
            }       
        }

        // Rival functionality on contact
        if (collision.gameObject.name == "BattleCharRival")
        {
            //TODO: Add collision effect
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Instantiate(explosionEffect, collision.transform.position, collision.transform.rotation);
            transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, -1f * Time.deltaTime * explosionForce);
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, transform.position, -1f * Time.deltaTime * explosionForce);
            target = null;

            //TODO: Decrease player def (Rival str)
            battleDef -= collision.gameObject.GetComponent<BattleCharControlRival>().str;
            //TODO: Drop player item if def <= 0 and stunned, then reset def
            StartCoroutine(StunForFewSecond());
            collision.gameObject.GetComponent<BattleCharControlRival>().target = null;

            //TODO: Decrease rival def (Player str)
            collision.gameObject.GetComponent<BattleCharControlRival>().battleDef -= GameManager.instance.playerStats[0].strength;

            //TODO: Drop rival item if def <= 0 and stunned, then reset def
            StartCoroutine(collision.gameObject.GetComponent<BattleCharControlRival>().StunForFewSecond());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //This fixed continuous movement after colliding with target and stop movement
        if (collision.gameObject == target)
        {
            target = null;
        }
    }

    IEnumerator StunForFewSecond()
    {
        // Defence is 0
        if (battleDef <= 0)
        {
            battleDef = 0;
            moveSpeed = 0f;
            stunText.SetActive(true);
            GetComponent<Collider2D>().enabled = false;

            // Drop item
            ItemDropped();
            
            if(SceneManager.GetActiveScene().name == "BattleScene4")
            {
                yield return new WaitForSeconds(4f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
            
            stunText.SetActive(false);
            GetComponent<Collider2D>().enabled = true;

            // Reset after stun
            GameManager.instance.playerStats[0].currentStamina += 200;
            moveSpeed = 2f;
            battleDef = GameManager.instance.playerStats[0].defence;
            dropObj = null;
        }

        // Stamina is 0
        if (GameManager.instance.playerStats[0].currentStamina <= 0)
        {
            moveSpeed = 0f;
            stunText.SetActive(true);
            GetComponent<Collider2D>().enabled = false;

            // Drop item
            ItemDropped();

            if (SceneManager.GetActiveScene().name == "BattleScene4")
            {
                yield return new WaitForSeconds(4f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
                
            stunText.SetActive(false);
            GetComponent<Collider2D>().enabled = true;

            // Reset after stun
            GameManager.instance.playerStats[0].currentStamina += 200;
            moveSpeed = 2f;
            dropObj = null;
        }

        // Endurance is 0
        if (GameManager.instance.playerStats[0].currentEndurance <= 0)
        {
            GameManager.instance.playerStats[0].currentEndurance = 0;
            moveSpeed = 0f;
            stunText.SetActive(true);
            GetComponent<Collider2D>().enabled = false;

            // Drop item
            ItemDropped();

            if (SceneManager.GetActiveScene().name == "BattleScene3" || SceneManager.GetActiveScene().name == "BattleScene4")
            {
                yield return new WaitForSeconds(4f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }

            stunText.SetActive(false);
            GetComponent<Collider2D>().enabled = true;

            // Reset after stun
            GameManager.instance.playerStats[0].currentStamina += 200;
            moveSpeed = 2f;
            GameManager.instance.playerStats[0].currentEndurance = GameManager.instance.playerStats[0].maxEndurance;
            dropObj = null;
        }
    }

    private void ItemDropped()
    {
        dropObj = null;
        float randomPosX = Random.Range(-6.5f, 8.5f);
        float randomPosY = Random.Range(-2.5f, 3.5f);
        newPos = new Vector2(randomPosX, randomPosY);

        // This decide drop item spawn position
        float xPlus, yPlus;
        if(newPos.x > transform.position.x)
        {
            xPlus = 1f;
        }
        else
        {
            xPlus = -1f;
        }
        if(newPos.y > transform.position.y)
        {
            yPlus = 1f;
        }
        else
        {
            yPlus = -1f;
        }

        if (heldItemApple.activeInHierarchy)
        {
            heldItemApple.SetActive(false);
            dropObj = Instantiate(house.GetComponent<House>().apple) as GameObject;
            dropObj.transform.position = new Vector2(transform.position.x + xPlus, transform.position.y + yPlus);
        }
        if (heldItemChest.activeInHierarchy)
        {
            heldItemChest.SetActive(false);
            dropObj = Instantiate(house.GetComponent<House>().chest, transform.position, transform.rotation) as GameObject;
            dropObj.transform.position = new Vector2(transform.position.x + xPlus, transform.position.y + yPlus);
        }
        if (heldItemMushroom.activeInHierarchy)
        {
            heldItemMushroom.SetActive(false);
            dropObj = Instantiate(house.GetComponent<House>().mushroom, transform.position, transform.rotation) as GameObject;
            dropObj.transform.position = new Vector2(transform.position.x + xPlus, transform.position.y + yPlus);
        }
    }

    IEnumerator Victory()
    {
        moveSpeed = 0f;
        FindObjectOfType<BattleCharControlRival>().moveSpeed = 0f;
        victoryWindow.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameManager.instance.crossFadeIsActive = false;
        GameManager.instance.battleIsActive = false;
        DialogueManager.instance.phaseCount++;

        // TODO: Ending 4
        if (SceneManager.GetActiveScene().name == "BattleScene4")
        {
            SceneManager.LoadScene("Ending4");
        }
        else
        {
            SceneManager.LoadScene("A2");
        }
    }
}
