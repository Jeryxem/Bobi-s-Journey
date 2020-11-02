using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleCharControlRival : MonoBehaviour
{
    public GameObject itemToGet;
    public GameObject house;
    private int score;
    [SerializeField] public GameObject target;
    private GameObject[] moreThanOneInScene;
    public GameObject heldItemApple, heldItemChest, heldItemMushroom, stunText, tiredText;
    public Text scoreText;
    public TextMesh enduranceText, strText, defText;
    public GameObject explosionEffect;
    public float explosionForce;
    public GameObject defeatWindow;

    //Rival's Stats
    public RivalLevel rivalLevel;
    public float endurance, str, def, agility;
    public float battleStr, battleDef, moveSpeed;
    public bool canKick = false;

    // Dropped item
    private GameObject dropObj;
    private Vector2 newPos;

    // Start is called before the first frame update
    void Start()
    {
        if (itemToGet == null)
        {
            score = 0;
        }

        battleStr = str;
        battleDef = def;

        // Control speed before match start
        moveSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Player win battle if score = 5
        if (score == 5)
        {
            StartCoroutine(Defeat());
        }

        switch (rivalLevel)
        {
            case RivalLevel.Level1:
                TargetItem();
                break;
            case RivalLevel.Level2:
                TargetPlayer();
                // Final battle
                if (SceneManager.GetActiveScene().name == "BattleScene4")
                {
                    if (target != house)
                        canKick = true;
                }
                break;
            case RivalLevel.Level3:
                RunFromPlayer();
                break;
            default:
                break;
        }

        // Movement of rival to target
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * (agility/10 * moveSpeed));
        }
        // Find new target if no target
        else
        {
            // TODO: find target logic base on rival level - AKA rival AI
            switch (rivalLevel)
            {
                case RivalLevel.Level1:
                    TargetItem();
                    break;
                case RivalLevel.Level2:
                    TargetPlayer();
                    // Final battle
                    if (SceneManager.GetActiveScene().name == "BattleScene4")
                    {
                        if(target != house)
                            canKick = true;
                    }
                    break;
                case RivalLevel.Level3:
                    RunFromPlayer();
                    break;
                default:
                    break;
            }
        }

        // Update rival's various battle stats
        scoreText.text = score.ToString();
        enduranceText.text = endurance.ToString();
        strText.text = battleStr.ToString();
        defText.text = battleDef.ToString();

        //Drop item
        if (dropObj != null)
            dropObj.transform.position = Vector2.MoveTowards(dropObj.transform.position, newPos, Time.deltaTime * 2.5f);
    }

    private void TargetItem()
    {
        if (house.GetComponent<House>().objToGet == house.GetComponent<House>().apple)
        {
            if (GameObject.FindGameObjectWithTag("Apple") == null)
            {
                // if rival is holding item
                if (heldItemApple.activeInHierarchy)
                {
                    target = house;
                }
                // Player is holding item
                else
                {
                    // Check if player is stunned
                    CheckPlayerStun();
                }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Apple");
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().mushroom)
        {
            if (GameObject.FindGameObjectWithTag("Mushroom") == null)
            {
                // if rival is holding item
                if (heldItemMushroom.activeInHierarchy)
                {
                    target = house;
                }
                else
                {
                    // Check if player is stunned
                    CheckPlayerStun();
                }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Mushroom");
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().chest)
        {
            if (GameObject.FindGameObjectWithTag("Chest") == null)
            {
                // if rival is holding item
                if (heldItemChest.activeInHierarchy)
                {
                    target = house;
                }
                else
                {
                    // Check if player is stunned
                    CheckPlayerStun();
                }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Chest");
            }
        }
    }
    public void TargetPlayer()
    {
        if (house.GetComponent<House>().objToGet == house.GetComponent<House>().apple)
        {
            if (GameObject.FindGameObjectWithTag("Apple") == null)
            {
                // if rival is holding item
                if (heldItemApple.activeInHierarchy)
                {
                    target = house;
                }
                // Player is holding item
                else
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
            }
            else
            {
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Apple").transform.position, transform.position) 
                    >= Vector3.Distance(GameObject.FindGameObjectWithTag("BattleCharPlayer").transform.position, transform.position) 
                    && !GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                    && target != GameObject.FindGameObjectWithTag("Apple"))
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
                else
                {
                    target = GameObject.FindGameObjectWithTag("Apple");
                }
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().mushroom)
        {
            if (GameObject.FindGameObjectWithTag("Mushroom") == null)
            {
                // if rival is holding item
                if (heldItemMushroom.activeInHierarchy)
                {
                    target = house;
                }
                // Player is holding item
                else
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
            }
            else
            {
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Mushroom").transform.position, transform.position) 
                    >= Vector3.Distance(GameObject.FindGameObjectWithTag("BattleCharPlayer").transform.position, transform.position) 
                    && !GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                    && target != GameObject.FindGameObjectWithTag("Mushroom"))
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
                else
                {
                    target = GameObject.FindGameObjectWithTag("Mushroom");
                }
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().chest)
        {
            if (GameObject.FindGameObjectWithTag("Chest") == null)
            {
                // if rival is holding item
                if (heldItemChest.activeInHierarchy)
                {
                    target = house;
                }
                // Player is holding item
                else
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
            }
            else
            {
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Chest").transform.position, transform.position) 
                    >= Vector3.Distance(GameObject.FindGameObjectWithTag("BattleCharPlayer").transform.position, transform.position) 
                    && !GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                    && target != GameObject.FindGameObjectWithTag("Chest"))
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
                else
                {
                    target = GameObject.FindGameObjectWithTag("Chest");
                }
            }
        }
    }

    public void RunFromPlayer()
    {
        if (house.GetComponent<House>().objToGet == house.GetComponent<House>().apple)
        {
            if (GameObject.FindGameObjectWithTag("Apple") == null)
            {
                // if rival is holding item
                if (heldItemApple.activeInHierarchy)
                {
                    if (GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                        || Vector3.Distance(transform.position, house.transform.position) <= 2.8f)
                    {
                        canKick = false;
                        target = house;
                    }
                    else
                    {
                        FindBomb();
                    }
                }
                // Player is holding item
                else
                {
                    FindBomb();
                }
            }
            else
            {
                canKick = false;
                target = GameObject.FindGameObjectWithTag("Apple");
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().mushroom)
        {
            if (GameObject.FindGameObjectWithTag("Mushroom") == null)
            {
                // if rival is holding item
                if (heldItemMushroom.activeInHierarchy)
                {
                    if (GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                        || Vector3.Distance(transform.position, house.transform.position) <= 2.8f)
                    {
                        canKick = false;
                        target = house;
                    }
                    else
                    {
                        FindBomb();
                    }
                }
                // Player is holding item
                else
                {
                    FindBomb();
                }
            }
            else
            {
                canKick = true;
                target = GameObject.FindGameObjectWithTag("Mushroom");
            }
        }
        else if (house.GetComponent<House>().objToGet == house.GetComponent<House>().chest)
        {
            if (GameObject.FindGameObjectWithTag("Chest") == null)
            {
                // if rival is holding item
                if (heldItemChest.activeInHierarchy)
                {
                    if (GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy
                        || Vector3.Distance(transform.position, house.transform.position) <= 2.8f)
                    {
                        canKick = false;
                        target = house;
                    }
                    else
                    {
                        FindBomb();
                    }
                }
                // Player is holding item
                else
                {
                    FindBomb();
                }
            }
            else
            {
                canKick = true;
                target = GameObject.FindGameObjectWithTag("Chest");
            }
        }
    }

    private void CheckPlayerStun()
    {
        if (GameObject.FindGameObjectWithTag("BattleCharPlayer").GetComponent<BattleCharControlPlayer>().stunText.activeInHierarchy)
        {
            target = null;
        }
        else
        {
            if (target != GameObject.FindGameObjectWithTag("BattleCharPlayer") && target != GameObject.FindGameObjectWithTag("Bomb"))
            {
                float rand = Random.Range(0f, 100f);
                if (rand <= 50)
                {
                    target = GameObject.FindGameObjectWithTag("BattleCharPlayer");
                }
                else
                {
                    FindBomb();
                }
            }
        }
    }

    public void FindBomb()
    {
        moreThanOneInScene = null;
        moreThanOneInScene = GameObject.FindGameObjectsWithTag("Bomb");
        float temp = Vector2.Distance(moreThanOneInScene[0].transform.position, transform.position);
        //TODO: Find closest bomb to rival
        for (int i = 0; i < moreThanOneInScene.Length; i++)
        {
            float dist = Vector2.Distance(moreThanOneInScene[i].transform.position, transform.position);
            if (dist <= temp)
            {
                temp = dist;
                target = moreThanOneInScene[i];
            }
        }
        canKick = true;
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
            //if (target == collision.gameObject)
            if (canKick == true)
            {
                // TODO: Kick bomb to rival if target is bomb
                canKick = false;
                float newPosXBomb, newPosYBomb;
                if (GameObject.Find("BattleCharPlayer").transform.position.x > transform.position.x)
                {
                    newPosXBomb = 1f;
                }
                else
                {
                    newPosXBomb = -1f;
                }
                if (GameObject.Find("BattleCharPlayer").transform.position.y > transform.position.y)
                {
                    newPosYBomb = 1f;
                }
                else
                {
                    newPosYBomb = -1f;
                }
                collision.gameObject.transform.position = new Vector2(transform.position.x + newPosXBomb, transform.position.y + newPosYBomb);
                collision.gameObject.GetComponent<Bomb>().target = GameObject.Find("BattleCharPlayer");
                target = null;
            }
            else
            {
                // This Add destroy effect and decrease endurance/stamina
                Instantiate(explosionEffect, collision.transform.position, collision.transform.rotation);
                endurance -= 50;
                StartCoroutine(StunForFewSecond());

                // The Pushback explosion force of bombs to player
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, -1f * Time.deltaTime * explosionForce);
                target = null;
                Destroy(collision.gameObject);
            }
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

    public IEnumerator StunForFewSecond()
    {
        if (battleDef <= 0)
        {
            battleDef = 0;
            moveSpeed = 0f;
            stunText.SetActive(true);
            GetComponent<Collider2D>().enabled = false;

            // Drop item
            ItemDropped();

            yield return new WaitForSeconds(2f);
            stunText.SetActive(false);
            GetComponent<Collider2D>().enabled = true;

            // Reset after stun
            moveSpeed = 2f;
            battleDef = def;
            dropObj = null;
        }

        if(endurance <= 0)
        {
            canKick = true;
            endurance = 0;
            moveSpeed = 0f;
            stunText.SetActive(true);
            GetComponent<Collider2D>().enabled = false;

            // Drop item
            ItemDropped();

            yield return new WaitForSeconds(2f);
            stunText.SetActive(false);
            GetComponent<Collider2D>().enabled = true;

            // Reset after stun
            moveSpeed = 2f;
            endurance = 100;
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
        if (newPos.x > transform.position.x)
        {
            xPlus = 1f;
        }
        else
        {
            xPlus = -1f;
        }
        if (newPos.y > transform.position.y)
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

    IEnumerator Defeat()
    {
        moveSpeed = 0f;
        FindObjectOfType<BattleCharControlPlayer>().moveSpeed = 0f;
        defeatWindow.SetActive(true);
        yield return new WaitForSeconds(3f);
        GameManager.instance.crossFadeIsActive = false;
        GameManager.instance.battleIsActive = false;

        //Reset Tutorial when lose
        if(SceneManager.GetActiveScene().name == "BattleScene")
        {
            DialogueManager.instance.phaseCount = 1;
            SceneManager.LoadScene("A2");
        }

        // TODO: Ending 1
        if (SceneManager.GetActiveScene().name == "BattleScene2")
            SceneManager.LoadScene("Ending1");

        // TODO: Ending 2
        if (SceneManager.GetActiveScene().name == "BattleScene3")
            SceneManager.LoadScene("Ending2");

        // TODO: Ending 3
        if (SceneManager.GetActiveScene().name == "BattleScene4")
            SceneManager.LoadScene("Ending3");
    }
}

public enum RivalLevel
{
    Level1,
    Level2,
    Level3
}
