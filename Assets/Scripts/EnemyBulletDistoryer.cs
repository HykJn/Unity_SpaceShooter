using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDistoryer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if (collision.gameObject.tag == "BulletBorder" || collision.gameObject.tag == "Bomb")
        {
            rbody.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerLogic = collision.GetComponent<PlayerController>();
            if(!playerLogic.onHit)
            {
                rbody.velocity = Vector3.zero;
                this.gameObject.SetActive(false);
            }
        }
    }
}
