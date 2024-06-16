using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rbody;
    SpriteRenderer render;
    Animator anim;
    public GameObject player;
    public Vector3 point;
    public GameManager gameManager;
    public ObjectManager objectManager;
    public Sprite enemy;
    public Sprite hit;
    public Canvas HpBarCanvas;
    public Slider HPbar;

    public int curHp;
    public int maxHp;
    public float speed;
    public float fireRate;
    float fireTick;
    public string type;
    public int score;
    public bool isMovable;
    float moveOffset;
    float leftPoint;
    float rightPoint;
    public bool onPoint = false;
    bool startMove = false;
    public bool isBoss;
    public int effectCount = 0;
    float effectTick = 0;

    //#BossFireTick
    float bossSpreadFireTick;
    float bossLargeTargetFireTick;
    float bossSmallTargetFireTick;
    float bossShotgunFireTick;
    int bossShotgunRate = 10;
    float innerTick;
    int shotCount;

    public string[] items;
    public float[] dropChance;
    float randF;
    // Start is called before the first frame update
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        curHp = maxHp;
        moveOffset = type == "L" ? 0.3f : 1f;
        if (isBoss) anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        curHp = maxHp;
        rbody.velocity = isBoss ? Vector2.down * 2f : Vector2.down * 4f;
        onPoint = false;
        startMove = false;
    }

    void Update()
    {
        if (this.transform.position.y <= point.y && !onPoint)
        {
            rbody.velocity = Vector2.zero;
            onPoint = true;
        }
        if (rbody.velocity.y == 0)
        {
            Fire();
            if (isMovable) Move();
            if (isBoss) HpBarCanvas.enabled = true;
        }

        if(curHp <= 0 && isBoss)
        {
            effectTick += Time.deltaTime;
            if(effectTick > 0.2f && effectCount <= 15)
            {
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                float scaleOffset = Random.Range(2f, 3f);
                GameObject explosion = objectManager.Activate("ExplosionEffect");
                explosion.transform.position = new Vector2(this.transform.position.x + x, this.transform.position.y + y);
                explosion.transform.localScale = new Vector2(scaleOffset, scaleOffset);
                effectTick = 0;
                effectCount++;
            }
            if (effectCount > 15) 
            {
                this.gameObject.SetActive(false);
                GameManager.score += this.score;
                GameManager.score += PlayerController.life * 10000;
            }
            gameManager.gameState = "Clear";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && onPoint && curHp >= 0)
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            Hit(bullet.damage);

            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.name == "EnemyBorder") this.gameObject.SetActive(false);
        if (collision.gameObject.tag == "Bomb" && curHp >= 0) Hit(15);
    }

    void Hit(int damage)
    {
        curHp -= damage;
        if(isBoss)
        {
            anim.SetTrigger("OnHit");
            HPbar.value = curHp;
        }
        else
        {
            render.sprite = hit;
            Invoke("ReturnSprite", 0.1f);
        }

        if (curHp <= 0 && !isBoss)
        {
            this.gameObject.SetActive(false);
            DropItem();
            gameManager.enemies.Remove(this.gameObject);
            GameManager.score += this.score;
            GameObject explosion = objectManager.Activate("ExplosionEffect");
            explosion.transform.position = this.transform.position;
        }
    }

    void ReturnSprite()
    {
        render.sprite = enemy;
    }

    void DropItem()
    {
        randF = Random.Range(0, 1.0f);
        if (dropChance.Length > 0 && items[0] != null && dropChance[0] >= randF)
        {
            GameObject Item = objectManager.Activate(items[0]);
            Item.transform.position = this.transform.position;
        }
        else if (dropChance.Length > 1 && items[1] != null && dropChance[0] + dropChance[1] >= randF)
        {
            GameObject Item = objectManager.Activate(items[1]);
            Item.transform.position = this.transform.position;
        }
        else if (dropChance.Length > 2 && items[2] != null && dropChance[0] + dropChance[1] + dropChance[2] >= randF)
        {
            GameObject Item = objectManager.Activate(items[2]);
            Item.transform.position = this.transform.position;
        }
    }

    void Fire()
    {
        fireTick += Time.deltaTime;
        if(fireTick > fireRate)
        {
            switch(type)
            {
                case "S":
                    GameObject bulletS = objectManager.Activate("EnemyBulletS");
                    Rigidbody2D rbulletS = bulletS.GetComponent<Rigidbody2D>();

                    bulletS.transform.position = this.transform.position;
                    Vector2 dir = player.transform.position - this.transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    bulletS.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                    rbulletS.AddForce(dir.normalized * 6, ForceMode2D.Impulse);
                    fireTick = 0;
                    break;
                case "M":
                    GameObject bulletM = objectManager.Activate("EnemyBulletM");
                    Rigidbody2D rbulletM = bulletM.GetComponent <Rigidbody2D>();

                    bulletM.transform.position = this.transform.position;
                    rbulletM.AddForce(Vector2.down * 6, ForceMode2D.Impulse);
                    fireTick = 0;
                    break;
                case "L":
                    for(int i = 0; i < 6; i++)
                    {
                        GameObject Lbullet = objectManager.Activate("EnemyBulletL");
                        Rigidbody2D rLbullet = Lbullet.GetComponent<Rigidbody2D>();
                        if (i < 3)
                        {
                            Lbullet.transform.position = new Vector2(this.transform.position.x - 0.35f, this.transform.position.y - 0.75f);
                        }
                        else if (i >= 3)
                        {
                            Lbullet.transform.position = new Vector2(this.transform.position.x + 0.35f, this.transform.position.y - 0.75f);
                        }
                        if (i % 3 == 0)
                        {
                            Lbullet.transform.rotation = Quaternion.Euler(0, 0, -20f);
                            rLbullet.AddForce(new Vector2(-Mathf.Cos(-20f), Mathf.Sin(-20f)).normalized * 5, ForceMode2D.Impulse);
                        }
                        else if (i % 3 == 1)
                        {
                            Lbullet.transform.rotation = Quaternion.Euler(Vector3.zero);
                            rLbullet.AddForce(Vector2.down * 5, ForceMode2D.Impulse);
                        }
                        else if ( i % 3 == 2)
                        {
                            Lbullet.transform.rotation = Quaternion.Euler(0, 0, 20f);
                            rLbullet.AddForce(new Vector2(Mathf.Cos(20f), -Mathf.Sin(20f)).normalized * 5, ForceMode2D.Impulse);
                        }
                    }
                    fireTick = 0;
                    break;
                case "Boss":
                    float bossX = this.transform.position.x;
                    float bossY = this.transform.position.y;
                    //#Patern1.
                    if (curHp > 1000)
                    {
                        bossLargeTargetFireTick += Time.deltaTime;
                        bossSpreadFireTick += Time.deltaTime;
                        //#LargeTargetFire
                        if (bossLargeTargetFireTick >= 0.3f)
                        {
                            GameObject LargeTargetBullet = objectManager.Activate("BossBullet");
                            Rigidbody2D rLargeTargetBullet = LargeTargetBullet.GetComponent<Rigidbody2D>();

                            LargeTargetBullet.transform.position = new Vector2(bossX, bossY - 1.25f);
                            Vector2 playerDir = player.transform.position - this.transform.position;
                            rLargeTargetBullet.AddForce(playerDir.normalized * 5, ForceMode2D.Impulse);
                            bossLargeTargetFireTick = 0;
                        }
                        //#SpreadFire
                        if (bossSpreadFireTick >= 1f)
                        {
                            for(int i = 0; i < 6; i++)
                            {
                                GameObject bossSpreadBullet = objectManager.Activate("EnemyBulletL");
                                Rigidbody2D rBossSpreadBullet = bossSpreadBullet.GetComponent<Rigidbody2D>();
                                if (i < 3)
                                {
                                    bossSpreadBullet.transform.position = new Vector2(bossX - 0.72f, bossY - 1.4f);
                                }
                                else if (i >= 3)
                                {
                                    bossSpreadBullet.transform.position = new Vector2(bossX + 0.72f, bossY - 1.4f);
                                }
                                if (i % 3 == 0)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, -20f);
                                    rBossSpreadBullet.AddForce(new Vector2(-Mathf.Cos(-20f), Mathf.Sin(-20f)).normalized * 6, ForceMode2D.Impulse);
                                }
                                else if (i % 3 == 1)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    rBossSpreadBullet.AddForce(Vector2.down * 6, ForceMode2D.Impulse);
                                }
                                else if (i % 3 == 2)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, +20f);
                                    rBossSpreadBullet.AddForce(new Vector2(Mathf.Cos(20f), -Mathf.Sin(20f)).normalized * 6, ForceMode2D.Impulse);
                                }
                            }
                            bossSpreadFireTick = 0;
                        }
                    }
                    //#Patern2.
                    else if (curHp > 0)
                    {
                        bossLargeTargetFireTick += Time.deltaTime;
                        bossSpreadFireTick += Time.deltaTime;
                        bossSmallTargetFireTick += Time.deltaTime;
                        bossShotgunFireTick += Time.deltaTime;
                        
                        //#LargeTargetFire
                        if (bossLargeTargetFireTick >= 0.3f && bossShotgunFireTick <= bossShotgunRate - 0.025f && bossShotgunFireTick >= 0.5f)
                        {
                            GameObject LargeTargetBullet = objectManager.Activate("BossBullet");
                            Rigidbody2D rLargeTargetBullet = LargeTargetBullet.GetComponent<Rigidbody2D>();

                            LargeTargetBullet.transform.position = new Vector2(bossX, bossY - 1.25f);
                            Vector2 playerDir = player.transform.position - this.transform.position;
                            rLargeTargetBullet.AddForce(playerDir.normalized * 5, ForceMode2D.Impulse);
                            bossLargeTargetFireTick = 0;
                        }
                        //#SpreadFire
                        if (bossSpreadFireTick >= 1f && bossShotgunFireTick <= bossShotgunRate - 0.025f && bossShotgunFireTick >= 0.5f)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                GameObject bossSpreadBullet = objectManager.Activate("EnemyBulletL");
                                Rigidbody2D rBossSpreadBullet = bossSpreadBullet.GetComponent<Rigidbody2D>();
                                if (i < 3)
                                {
                                    bossSpreadBullet.transform.position = new Vector2(bossX - 0.72f, bossY - 1.4f);
                                }
                                else if (i >= 3)
                                {
                                    bossSpreadBullet.transform.position = new Vector2(bossX + 0.72f, bossY - 1.4f);
                                }
                                if (i % 3 == 0)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, -20f);
                                    rBossSpreadBullet.AddForce(new Vector2(-Mathf.Cos(-20f), Mathf.Sin(-20f)).normalized * 6, ForceMode2D.Impulse);
                                }
                                else if (i % 3 == 1)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    rBossSpreadBullet.AddForce(Vector2.down * 6, ForceMode2D.Impulse);
                                }
                                else if (i % 3 == 2)
                                {
                                    bossSpreadBullet.transform.rotation = Quaternion.Euler(0, 0, +20f);
                                    rBossSpreadBullet.AddForce(new Vector2(Mathf.Cos(20f), -Mathf.Sin(20f)).normalized * 6, ForceMode2D.Impulse);
                                }
                            }
                            bossSpreadFireTick = 0;
                        }
                        //#SmallTargetFire
                        if (bossSmallTargetFireTick >= 0.9f && bossShotgunFireTick <= bossShotgunRate - 0.025f && bossShotgunFireTick >= 0.5f)
                        {
                            GameObject SmallTargetBulletL = objectManager.Activate("EnemyBulletS");
                            GameObject SmallTargetBulletR = objectManager.Activate("EnemyBulletS");

                            Rigidbody2D rSmallTargetBulletL = SmallTargetBulletL.GetComponent<Rigidbody2D>();
                            Rigidbody2D rSmallTargetBulletR = SmallTargetBulletR.GetComponent<Rigidbody2D>();

                            SmallTargetBulletL.transform.position = new Vector2(bossX - 1.125f, bossY - 1.15f);
                            SmallTargetBulletR.transform.position = new Vector2(bossX + 1.125f, bossY - 1.15f);

                            Vector2 LDir = player.transform.position - SmallTargetBulletL.transform.position;
                            Vector2 RDir = player.transform.position - SmallTargetBulletR.transform.position;
                            float LAngle = Mathf.Atan2(LDir.y, LDir.x) * Mathf.Rad2Deg;
                            float RAngle = Mathf.Atan2(RDir.y, RDir.x) * Mathf.Rad2Deg;

                            SmallTargetBulletL.transform.rotation = Quaternion.Euler(0, 0, LAngle - 90f);
                            SmallTargetBulletR.transform.rotation = Quaternion.Euler(0, 0, RAngle - 90f);

                            rSmallTargetBulletL.AddForce(LDir.normalized * 5.5f, ForceMode2D.Impulse);
                            rSmallTargetBulletR.AddForce(RDir.normalized * 5.5f, ForceMode2D.Impulse);
                            bossSmallTargetFireTick = 0;
                        }
                        //#ShotgunFire
                        if (bossShotgunFireTick >= bossShotgunRate)
                        {
                            innerTick += Time.deltaTime;
                            if (innerTick >= 0.3f && shotCount < 3)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    GameObject ShotgunBullet = objectManager.Activate("EnemyBulletM");
                                    Rigidbody2D rShotgunBullet = ShotgunBullet.GetComponent<Rigidbody2D>();
                                    ShotgunBullet.transform.position = new Vector2(bossX, bossY - 1.25f);
                                    Vector2 bulletDir = new Vector2(Mathf.Cos((45 + i * (45f/4)) * Mathf.Deg2Rad), -Mathf.Sin((45 + i * (45f / 4)) * Mathf.Deg2Rad));
                                    rShotgunBullet.AddForce(bulletDir.normalized * 5.5f, ForceMode2D.Impulse);
                                }
                                for (int i = 0; i < 5; i++)
                                {
                                    GameObject ShotgunBullet = objectManager.Activate("EnemyBulletM");
                                    Rigidbody2D rShotgunBullet = ShotgunBullet.GetComponent<Rigidbody2D>();
                                    ShotgunBullet.transform.position = new Vector2(bossX, bossY - 1.25f);
                                    Vector2 bulletDir = new Vector2(-Mathf.Cos((45 + i * (45f / 4)) * Mathf.Deg2Rad), -Mathf.Sin((45 + i * (45f / 4)) * Mathf.Deg2Rad));
                                    rShotgunBullet.AddForce(bulletDir.normalized * 5.5f, ForceMode2D.Impulse);
                                }
                                shotCount++;
                                innerTick = 0;
                            }
                            if (shotCount == 3)
                            {
                                bossShotgunFireTick = 0;
                                shotCount = 0;
                                bossShotgunRate = Random.Range(10, 21);
                            }
                        }
                    }
                    break;
            }
        }
    }

    void Move()
    {
        if(onPoint && !startMove)
        {
            leftPoint = this.transform.position.x - moveOffset;
            rightPoint = this.transform.position.x + moveOffset;
            rbody.velocity = Vector2.left * speed;
            startMove = true;
        }
        if (this.transform.position.x <= leftPoint) rbody.velocity = Vector2.right * speed;
        if (this.transform.position.x >= rightPoint) rbody.velocity = Vector2.left * speed;
    }
}
