using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public float scrollSpeed = 1.0f;

    void Update()
    {
        if (transform.position.y > -1)
        {
            transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }
        else transform.position = new Vector3(0, 1, 0);
    }
}
