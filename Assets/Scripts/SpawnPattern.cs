using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Enemy
{
    public Vector2 point;
    public string type;
    public bool isMovable;

    public Enemy(Vector2 point, string type, bool isMovable = true)
    {
        this.point = point;
        this.type = type;
        this.isMovable = isMovable;
    }
}

public struct Pattern
{
    public Enemy[] enemies;
}

public class SpawnPattern : MonoBehaviour
{
    //#1. Easy Patterns
    public Pattern b_s2_b_M;
    public Pattern s2_m1_b;
    public Pattern b_m1s2_b;
    public Pattern b_m1_s2;
    public Pattern b_m2_b;
    public Pattern b_m2_b_M;
    public Pattern b_m1_m2;

    //#2. Normal Patterns
    public Pattern b_m2_l1;
    public Pattern m1_m2_l1;
    public Pattern m1_s2_l1;
    public Pattern b_m1_l2;
    public Pattern m2_s4_l2;

    public Pattern boss;
    private void Awake()
    {
        //#1. Easy Patterns
        b_s2_b_M.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 2), "S", false),
            new Enemy(new Vector2(1, 2), "S", false)
        };
        s2_m1_b.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 1), "S"),
            new Enemy(new Vector2(1, 1), "S"),
            new Enemy(new Vector2(0, 2), "M")
        };
        b_m1s2_b.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 2), "S"),
            new Enemy(new Vector2(1, 2), "S"),
            new Enemy(new Vector2(0, 2), "M")
        };
        b_m1_s2.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 3), "S"),
            new Enemy(new Vector2(1, 3), "S"),
            new Enemy(new Vector2(0, 2), "M")
        };
        b_m2_b.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1 ,2), "M"),
            new Enemy(new Vector2(1 ,2), "M")
        };
        b_m2_b_M.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1 ,2), "M", false),
            new Enemy(new Vector2(1 ,2), "M", false)
        };
        b_m1_m2.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1 ,3), "M"),
            new Enemy(new Vector2(1 ,3), "M"),
            new Enemy(new Vector2(0 ,2), "M")
        };

        //#2. Hard Patterns
        b_m2_l1.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 2), "M"),
            new Enemy(new Vector2(1, 2), "M"),
            new Enemy(new Vector2(0, 3), "L", false)
        };
        m1_m2_l1.enemies = new Enemy[]
        {
            new Enemy(new Vector2(0, 1), "M", false),
            new Enemy(new Vector2(-2, 2), "M", false),
            new Enemy(new Vector2(2, 2), "M", false),
            new Enemy(new Vector2(0, 3), "L", false)
        };
        m1_s2_l1.enemies = new Enemy[]
        {
            new Enemy(new Vector2(0, 1), "M"),
            new Enemy(new Vector2(-1, 2), "S"),
            new Enemy(new Vector2(1, 2), "S"),
            new Enemy(new Vector2(0, 3), "L", false)
        };
        b_m1_l2.enemies = new Enemy[]
        {
            new Enemy(new Vector2(0, 2), "M"),
            new Enemy(new Vector2(-1.5f, 3), "L"),
            new Enemy(new Vector2(1.5f, 3), "L")
        };
        m2_s4_l2.enemies = new Enemy[]
        {
            new Enemy(new Vector2(-1, 1), "M", false),
            new Enemy(new Vector2(1, 1), "M", false),
            new Enemy(new Vector2(-1.5f, 2), "S"),
            new Enemy(new Vector2(-0.5f, 2), "S"),
            new Enemy(new Vector2(1.5f, 2), "S"),
            new Enemy(new Vector2(2, 2), "S"),
            new Enemy(new Vector2(-2, 3), "L", false),
            new Enemy(new Vector2(2, 3), "L", false)
        };

        boss.enemies = new Enemy[]
        {
            new Enemy(new Vector2(0, 2.5f), "Boss", false)
        };
    }
}