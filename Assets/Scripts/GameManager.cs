using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ObjectManager objectManager;
    public GameObject player;
    public SpawnPattern spawnPattern;
    public Canvas titleCanvas;
    public Canvas inGameCanvas;
    public Canvas gameOverCanvas;
    public Canvas HpBarCanvas;
    public Canvas clearCanvas;
    public Text title;
    public Text gameStartText;
    public Text gameOverScore;
    public Text restartText;
    public Text backToTitleText;
    public Text ClearScore;
    public Slider BossHpBar;
    float titleY;

    Pattern[] easyPatterns;
    Pattern[] hardPatterns;
    Pattern boss;

    public string gameState;
    public static int score = 0;
    public Text scoreBoard;
    public Image[] Lifes, Bombs;

    public List<GameObject> enemies;



    void Awake()
    {
        instance = this;
        gameState = "Title";
        titleY = title.transform.position.y;
        player = GameObject.FindWithTag("Player");
        PlayerController playerLogic = player.GetComponent<PlayerController>();
        playerLogic.objectManager = this.objectManager;
        player.SetActive(false);
        easyPatterns = new Pattern[]
        {
            spawnPattern.b_s2_b_M,
            spawnPattern.s2_m1_b,
            spawnPattern.b_m1s2_b,
            spawnPattern.b_m1_s2,
            spawnPattern.b_m2_b,
            spawnPattern.b_m2_b_M,
            spawnPattern.b_m1_m2
        };
        hardPatterns = new Pattern[]
        {
            spawnPattern.b_m2_l1,
            spawnPattern.m1_m2_l1,
            spawnPattern.m1_s2_l1,
            spawnPattern.b_m1_l2,
            spawnPattern.m2_s4_l2
        };
        boss = spawnPattern.boss;
    }

    void Update()
    {
        UIControll();
        if(Input.anyKeyDown && gameState ==  "Title")
        {
            gameState = "InGame";
            player.SetActive(true);
            player.transform.position = new Vector2(0, -4);
        }
        else if (gameState == "InGame")
        {
            if (enemies.Count == 0 && score <= 40000)
            {
                int idx = Random.Range(0, easyPatterns.Length);
                foreach (Enemy enemy in easyPatterns[idx].enemies)
                {
                    SpawnEnemy(enemy);
                }
            }
            else if (enemies.Count == 0 && score <= 100000)
            {
                int idx = Random.Range(0, hardPatterns.Length);
                foreach (Enemy enemy in hardPatterns[idx].enemies)
                {
                    SpawnEnemy(enemy);
                }
            }
            else if (enemies.Count == 0 && score > 100000)
            {
                SpawnEnemy(boss.enemies[0]);
            }
        }
        else if (gameState == "GameOver" || gameState == "Clear")
        {
            if (Input.anyKeyDown)
            {
                HpBarCanvas.enabled = false;
                score = 0;
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach(GameObject bullet in bullets)
                {
                    bullet.SetActive(false);
                }
                GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
                foreach(GameObject item in items)
                {
                    item.SetActive(false);
                }
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject enemy in enemies)
                {
                    enemy.SetActive(false);
                }
                this.enemies.Clear();
                gameState = "Title";
            }
        }
    }

    void SpawnEnemy(Enemy enemy)
    {
        GameObject enemyObj = null;
        switch (enemy.type) 
        {
            case "S":
                enemyObj = objectManager.Activate("EnemyS");
                enemies.Add(enemyObj);
                break;
            case "M":
                enemyObj = objectManager.Activate("EnemyM");
                enemies.Add(enemyObj);
                break;
            case "L":
                enemyObj = objectManager.Activate("EnemyL");
                enemies.Add(enemyObj);
                break;
            case "Boss":
                enemyObj = objectManager.Activate("Boss");
                enemies.Add(enemyObj);
                break;
        }
        if (enemyObj != null)
        {
            enemyObj.transform.position = enemy.point + Vector2.up * 5;
            EnemyController enemyLogic = enemyObj.GetComponent<EnemyController>();
            enemyLogic.gameManager = this;
            enemyLogic.objectManager = this.objectManager;
            enemyLogic.point = enemy.point;
            enemyLogic.isMovable = enemy.isMovable;
            enemyLogic.player = player;
            if(enemy.type == "Boss")
            {
                enemyLogic.HPbar = BossHpBar;
                enemyLogic.HpBarCanvas = HpBarCanvas;
            }
        }
    }

    void UIControll()
    {
        if (gameState == "Title")
        {
            titleCanvas.enabled = true;
            inGameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            clearCanvas.enabled = false;

            title.transform.position = new Vector2(title.transform.position.x, titleY + Mathf.Sin(Time.time * 3) * 10);
            gameStartText.color = new Color(1, 1, 1, Mathf.Sin(Time.time * 10) * 0.25f + 0.75f);
        }
        else if (gameState == "InGame")
        {
            titleCanvas.enabled = false;
            inGameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            clearCanvas.enabled = false;
            scoreBoard.text = $"{score}";
            switch (PlayerController.bomb)
            {
                case 0:
                    Bombs[0].gameObject.SetActive(false);
                    Bombs[1].gameObject.SetActive(false);
                    Bombs[2].gameObject.SetActive(false);
                    break;
                case 1:
                    Bombs[0].gameObject.SetActive(true);
                    Bombs[1].gameObject.SetActive(false);
                    Bombs[2].gameObject.SetActive(false);
                    break;
                case 2:
                    Bombs[0].gameObject.SetActive(true);
                    Bombs[1].gameObject.SetActive(true);
                    Bombs[2].gameObject.SetActive(false);
                    break;
                case 3:
                    Bombs[0].gameObject.SetActive(true);
                    Bombs[1].gameObject.SetActive(true);
                    Bombs[2].gameObject.SetActive(true);
                    break;
            }
            switch (PlayerController.life)
            {
                case 0:
                    Lifes[0].gameObject.SetActive(false);
                    Lifes[1].gameObject.SetActive(false);
                    Lifes[2].gameObject.SetActive(false);
                    break;
                case 1:
                    Lifes[0].gameObject.SetActive(true);
                    Lifes[1].gameObject.SetActive(false);
                    Lifes[2].gameObject.SetActive(false);
                    break;
                case 2:
                    Lifes[0].gameObject.SetActive(true);
                    Lifes[1].gameObject.SetActive(true);
                    Lifes[2].gameObject.SetActive(false);
                    break;
                case 3:
                    Lifes[0].gameObject.SetActive(true);
                    Lifes[1].gameObject.SetActive(true);
                    Lifes[2].gameObject.SetActive(true);
                    break;
            }
        }
        else if (gameState == "GameOver")
        {
            titleCanvas.enabled = false;
            inGameCanvas.enabled = false;
            gameOverCanvas.enabled = true;
            clearCanvas.enabled = false;
            gameOverScore.text = $"Score : {score}";
            restartText.color = new Color(1, 1, 1, Mathf.Sin(Time.time * 10) * 0.25f + 0.75f);
        }
        else if (gameState == "Clear")
        {
            titleCanvas.enabled = false;
            inGameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
            clearCanvas.enabled = true;
            ClearScore.text = $"Score : {score}";
            backToTitleText.color = new Color(1, 1, 1, Mathf.Sin(Time.time * 10) * 0.25f + 0.75f);
        }
    }
}
