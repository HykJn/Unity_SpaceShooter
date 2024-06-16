using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInitiater : MonoBehaviour
{
    Rigidbody2D rbody;
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rbody.velocity = Vector2.down * 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            this.gameObject.SetActive(false);
        }
    }
}
