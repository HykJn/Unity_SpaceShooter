using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    Animator anim;
    BulletController bullet;
    SpriteRenderer spriteRenderer;

    public GameManager gameManager;
    public ObjectManager objectManager;
    GameObject bombEffect;
    public GameObject[] playerBullets;
    public float speed = 6.0f;
    float axisH;
    float axisV;
    public bool onHit = false;
    public float hitTime = 0f;
    public static int life = 3;

    public int power = 1;

    public static int bomb = 0;
    int maxBomb = 3;

    public static int hasFollower = 2;

    public float delay = 0.2f;
    public float delayTick = 0f;

    public bool leftBorder, rightBorder, upperBorder, lowerBorder;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerBullets = new GameObject[8];
    }

    void OnEnable()
    {
        life = 3;
        power = 1;
        bomb = 0;
        hasFollower = 0;
    }

    void Update()
    {
        Move();
        Fire();
        Bomb();
        if(onHit)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 1f)
            {
                spriteRenderer.color = Color.white;
                hitTime = 0f;
                onHit = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "LeftBorder":
                    leftBorder = true;
                    break;
                case "RightBorder":
                    rightBorder = true;
                    break;
                case "UpperBorder":
                    upperBorder = true;
                    break;
                case "LowerBorder":
                    lowerBorder = true;
                    break;
            }
        }

        if (collision.gameObject.tag == "Item")
        {
            switch (collision.gameObject.name)
            {
                case "PowerItem(Clone)":
                    power++;
                    collision.gameObject.SetActive(false);
                    if(power > 5 && power % 2 != 0)
                    {
                        life = life < 3 ? ++life : life;
                    }
                    break;
                case "BombItem(Clone)":
                    bomb = bomb < maxBomb ? ++bomb : bomb;
                    collision.gameObject.SetActive(false);
                    break;
                case "FollowerItem(Clone)":
                    hasFollower++;
                    if (hasFollower == 1)
                    {
                        GameObject follower = objectManager.Activate("Follower");
                        FollowerController followerLogic = follower.GetComponent<FollowerController>();
                        followerLogic.gameManager = this.gameManager;
                        followerLogic.objectManager = this.objectManager;
                        followerLogic.player = this.gameObject;
                        followerLogic.number = 1;
                    }
                    else if (hasFollower == 2)
                    {
                        GameObject follower = objectManager.Activate("Follower");
                        FollowerController followerLogic = follower.GetComponent<FollowerController>();
                        followerLogic.gameManager = this.gameManager;
                        followerLogic.objectManager = this.objectManager;
                        followerLogic.player = this.gameObject;
                        followerLogic.number = 2;
                    }
                    else life = life < 3 ? ++life : life;
                    collision.gameObject.SetActive(false);
                    break;
            }
        }

        if (collision.gameObject.tag == "EnemyBullet")
        {
            if(!onHit)
            {
                onHit = true;
                life--;
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                if(life <= 0)
                {
                    gameManager.gameState = "GameOver";
                    this.gameObject.SetActive(false);
                    GameObject explosion = objectManager.Activate("ExplosionEffect");
                    explosion.transform.position = this.transform.position;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "LeftBorder":
                    leftBorder = false;
                    break;
                case "RightBorder":
                    rightBorder = false;
                    break;
                case "UpperBorder":
                    upperBorder = false;
                    break;
                case "LowerBorder":
                    lowerBorder = false;
                    break;
            }
        }
    }

    void Move()
    {
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
        if ((axisH == -1 && leftBorder) || (axisH == 1 && rightBorder)) axisH = 0;
        if ((axisV == -1 && lowerBorder) || (axisV == 1 && upperBorder)) axisV = 0;

        anim.SetInteger("AxisH", (int)axisH);

        transform.Translate(Vector2.right * axisH * speed * Time.deltaTime);
        transform.Translate(Vector2.up * axisV * speed * Time.deltaTime);
    }

    void Fire()
    {
        delayTick += Time.deltaTime;
        if(Input.GetKey(KeyCode.Space) && delayTick > delay)
        {
            switch(power)
            {
                case 1:
                    playerBullets[0] = objectManager.Activate("PlayerBulletS");
                    playerBullets[0].transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
                    delayTick = 0;
                    break;
                case 2:
                    playerBullets[1] = objectManager.Activate("PlayerBulletS");
                    playerBullets[1].transform.position = this.transform.position + new Vector3(-0.1f, 0.5f, 0);
                    playerBullets[2] = objectManager.Activate("PlayerBulletS");
                    playerBullets[2].transform.position = this.transform.position + new Vector3(0.1f, 0.5f, 0);
                    delayTick = 0;
                    break;
                case 3:
                    playerBullets[3] = objectManager.Activate("PlayerBulletL");
                    playerBullets[3].transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
                    delayTick = 0;
                    break;
                case 4:
                    playerBullets[4] = objectManager.Activate("PlayerBulletL");
                    bullet = playerBullets[4].GetComponent<BulletController>();
                    playerBullets[4].transform.localScale = new Vector2(1.1f, 1.1f);
                    bullet.damage = 4;
                    playerBullets[4].transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
                    delayTick = 0;
                    break;
                default:
                    playerBullets[5] = objectManager.Activate("PlayerBulletL");
                    bullet = playerBullets[5].GetComponent<BulletController>();
                    bullet.damage = 3;
                    playerBullets[5].transform.position = this.transform.position + new Vector3(0, 0.5f, 0);
                    playerBullets[5].transform.localScale = new Vector2(1.1f, 1.1f);
                    playerBullets[6] = objectManager.Activate("PlayerBulletS");
                    playerBullets[6].transform.position = this.transform.position + new Vector3(-0.2f, 0.5f, 0);
                    playerBullets[7] = objectManager.Activate("PlayerBulletS");
                    playerBullets[7].transform.position = this.transform.position + new Vector3(0.2f, 0.5f, 0);
                    delayTick = 0;
                    break;
            }
        }
    }

    void Bomb()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && bomb > 0)
        {
            bomb--;
            bombEffect = objectManager.Activate("BombEffect");
            bombEffect.transform.position = this.transform.position;
        }
    }
}
