using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rbody;
    public int damage;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BulletBorder") this.gameObject.SetActive(false);
    }
}
