using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameManager gameManager;
    public GameObject player;
    GameObject bullet;
    float fireTick;
    public int number;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - 0.75f, player.transform.position.y + 0.5f);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + 0.75f, player.transform.position.y + 0.5f);
        }

        if (gameManager.gameState == "InGame")
        {
            fireTick += Time.deltaTime;
            if (fireTick >= 0.5f)
            {
                bullet = objectManager.Activate("PlayerBulletS");
                BulletController bulletLogic = bullet.GetComponent<BulletController>();
                bulletLogic.damage = 2;
                bullet.transform.position = this.transform.position;
                fireTick = 0;
            }
        }

        if(PlayerController.life <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
