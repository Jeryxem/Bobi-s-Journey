using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private float randomPosX, randomPosY;
    public GameObject[] spawnBox;
    public GameObject apple, mushroom, chest, bomb;
    public GameObject objToGet, objToAvoid, getApple, getMushroom, getChest;
    public BattleCharControlPlayer player;
    public BattleCharControlRival rival;
    public GameObject[] playerHeldItem, rivalHeldItem;
    private float timerToNextChange = 10f, rand, bombTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewItemToGet();
        RandomObjToGet();
        GameManager.instance.battleIsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        timerToNextChange -= Time.deltaTime;
        if(timerToNextChange <= 0 && player.moveSpeed != 0f && rival.moveSpeed != 0f)
        {
            timerToNextChange = 10f;
            RandomObjToGet();
            SpawnBomb();
        }

        bombTimer -= Time.deltaTime;
        if (bombTimer <= 0 && player.moveSpeed != 0f && rival.moveSpeed != 0f)
        {
            bombTimer = 5f;
            SpawnBomb();
        }

        SpawnNewItemToGet();

        // Update obj to get UI
        if (objToGet == apple)
        {
            getApple.SetActive(true);
            getMushroom.SetActive(false);
            getChest.SetActive(false);
        }
        if (objToGet == mushroom)
        {
            getApple.SetActive(false);
            getMushroom.SetActive(true);
            getChest.SetActive(false);
        }
        if (objToGet == chest)
        {
            getApple.SetActive(false);
            getMushroom.SetActive(false);
            getChest.SetActive(true);
        }

        // Spawn bomb if no bomb in battle scene
        if(GameObject.Find("Bomb(Clone)") == null)
        {
            SpawnBomb();
        }
    }

    public void RandomObjToGet()
    {
        rand = Random.Range(0f, 150f);
        if (rand <= 50f)
        {
            objToGet = apple;
        }
        else if (rand <= 100f)
        {
            objToGet = mushroom;
        }
        else
        {
            objToGet = chest;
        }

        player.itemToGet = objToGet;
        rival.itemToGet = objToGet;
    }

    private void SpawnNewItemToGet()
    {
        foreach (GameObject obj in spawnBox)
        {
            if (obj.name == "AppleSpawn")
            {
                for(int i = 0; i < playerHeldItem.Length; i++)
                {
                    if (!playerHeldItem[0].activeInHierarchy && !rivalHeldItem[0].activeInHierarchy && GameObject.FindGameObjectWithTag("Apple") == null)
                    {
                        Instantiate(apple, obj.transform.position, obj.transform.rotation);
                        SpawnBomb();
                    }
                }
            }
            if (obj.name == "MushroomSpawn")
            {
                for (int i = 0; i < playerHeldItem.Length; i++)
                {
                    if (!playerHeldItem[2].activeInHierarchy && !rivalHeldItem[2].activeInHierarchy && GameObject.FindGameObjectWithTag("Mushroom") == null)
                    {
                        Instantiate(mushroom, obj.transform.position, obj.transform.rotation);
                        SpawnBomb();
                    }
                }
            }
            if (obj.name == "ChestSpawn")
            {
                for (int i = 0; i < playerHeldItem.Length; i++)
                {
                    if (!playerHeldItem[1].activeInHierarchy && !rivalHeldItem[1].activeInHierarchy && GameObject.FindGameObjectWithTag("Chest") == null)
                    {
                        Instantiate(chest, obj.transform.position, obj.transform.rotation);
                        SpawnBomb();
                    }
                }
            }
        }
    }

    public void SpawnBomb()
    {
        // Spawn bombs randomly
        randomPosX = Random.Range(-6.5f, 8.5f);
        randomPosY = Random.Range(-2.5f, 3.5f);
        objToAvoid = Instantiate(bomb) as GameObject;
        objToAvoid.transform.position = new Vector2(randomPosX, randomPosY);
    }
}
