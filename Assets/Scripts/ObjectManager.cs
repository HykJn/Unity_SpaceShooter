using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //Componenet
    public GameObject EnemySPrefab;
    public GameObject EnemyMPrefab;
    public GameObject EnemyLPrefab;

    public GameObject PlayerBulletSPrefab;
    public GameObject PlayerBulletLPrefab;
    public GameObject BossPrefab;

    public GameObject EnemyBulletSPrefab;
    public GameObject EnemyBulletMPrefab;
    public GameObject EnemyBulletLPrefab;
    public GameObject BossBulletPrefab;

    public GameObject PowerItemPrefab;
    public GameObject BombItemPrefab;
    public GameObject FollowerItemPrefab;
    public GameObject FollowerPrefab;

    public GameObject BombEffectPrefab;
    public GameObject ExplosionEffectPrefab;

    //Array
    GameObject[] EnemyS;
    GameObject[] EnemyM;
    GameObject[] EnemyL;
    GameObject[] Boss;

    GameObject[] PlayerBulletS;
    GameObject[] PlayerBulletL;

    GameObject[] EnemyBulletS;
    GameObject[] EnemyBulletM;
    GameObject[] EnemyBulletL;
    GameObject[] BossBullet;

    GameObject[] PowerItem;
    GameObject[] BombItem;
    GameObject[] FollowerItem;
    GameObject[] Follower;

    GameObject[] BombEffect;
    GameObject[] ExplosionEffect;
    GameObject[] TargetPool;

    void Awake()
    {
        EnemyS = new GameObject[10];
        EnemyM = new GameObject[10];
        EnemyL = new GameObject[10];
        Boss = new GameObject[1];

        PlayerBulletS = new GameObject[100];
        PlayerBulletL = new GameObject[100];

        EnemyBulletS = new GameObject[100];
        EnemyBulletM = new GameObject[100];
        EnemyBulletL = new GameObject[100];
        BossBullet = new GameObject[100];

        PowerItem = new GameObject[20];
        BombItem = new GameObject[20];
        FollowerItem = new GameObject[20];
        Follower = new GameObject[2];

        BombEffect = new GameObject[10];
        ExplosionEffect = new GameObject[30];
        Generate();
    }

    void Generate()
    {
        //#1. Enemy
        for(int i = 0; i < EnemyS.Length; i++)
        {
            EnemyS[i] = Instantiate(EnemySPrefab);
            EnemyS[i].SetActive(false);
        }
        for (int i = 0; i < EnemyM.Length; i++)
        {
            EnemyM[i] = Instantiate(EnemyMPrefab);
            EnemyM[i].SetActive(false);
        }
        for (int i = 0; i < EnemyL.Length; i++)
        {
            EnemyL[i] = Instantiate(EnemyLPrefab);
            EnemyL[i].SetActive(false);
        }
        for (int i = 0; i < Boss.Length; i++)
        {
            Boss[i] = Instantiate(BossPrefab);
            Boss[i].SetActive(false);
        }
        //#2. PlayerBullet
        for (int i = 0; i < PlayerBulletS.Length; i++)
        {
            PlayerBulletS[i] = Instantiate(PlayerBulletSPrefab);
            PlayerBulletS[i].SetActive(false);
        }
        for (int i = 0; i < PlayerBulletL.Length; i++)
        {
            PlayerBulletL[i] = Instantiate(PlayerBulletLPrefab);
            PlayerBulletL[i].SetActive(false);
        }
        //#3. EnemyBullet
        for (int i = 0; i < EnemyBulletS.Length; i++)
        {
            EnemyBulletS[i] = Instantiate(EnemyBulletSPrefab);
            EnemyBulletS[i].SetActive(false);
        }
        for (int i = 0; i < EnemyBulletM.Length; i++)
        {
            EnemyBulletM[i] = Instantiate(EnemyBulletMPrefab);
            EnemyBulletM[i].SetActive(false);
        }
        for (int i = 0; i < EnemyBulletL.Length; i++)
        {
            EnemyBulletL[i] = Instantiate(EnemyBulletLPrefab);
            EnemyBulletL[i].SetActive(false);
        }
        for (int i = 0; i < BossBullet.Length; i++)
        {
            BossBullet[i] = Instantiate(BossBulletPrefab);
            BossBullet[i].SetActive(false);
        }
        //#4. Items
        for (int i = 0; i < PowerItem.Length; i++)
        {
            PowerItem[i] = Instantiate(PowerItemPrefab);
            PowerItem[i].SetActive(false);
        }
        for (int i = 0; i < BombItem.Length; i++)
        {
            BombItem[i] = Instantiate(BombItemPrefab);
            BombItem[i].SetActive(false);
        }
        for (int i = 0; i < FollowerItem.Length; i++)
        {
            FollowerItem[i] = Instantiate(FollowerItemPrefab);
            FollowerItem[i].SetActive(false);
        }
        for (int i = 0; i < Follower.Length; i++)
        {
            Follower[i] = Instantiate(FollowerPrefab);
            Follower[i].SetActive(false);
        }
        //#5. Effect
        for (int i = 0; i < BombEffect.Length; i++)
        {
            BombEffect[i] = Instantiate(BombEffectPrefab);
            BombEffect[i].SetActive(false);
        }
        for (int i = 0; i < ExplosionEffect.Length; i++)
        {
            ExplosionEffect[i] = Instantiate(ExplosionEffectPrefab);
            ExplosionEffect[i].SetActive(false);
        }
    }

    public GameObject Activate(string prefab)
    {
        switch(prefab)
        {
            case "EnemyS":
                TargetPool = EnemyS;
                break;
            case "EnemyM":
                TargetPool = EnemyM;
                break;
            case "EnemyL":
                TargetPool = EnemyL;
                break;
            case "Boss":
                TargetPool = Boss;
                break;
            case "PlayerBulletS":
                TargetPool = PlayerBulletS;
                break;
            case "PlayerBulletL":
                TargetPool = PlayerBulletL;
                break;
            case "EnemyBulletS":
                TargetPool = EnemyBulletS;
                break;
            case "EnemyBulletM":
                TargetPool = EnemyBulletM;
                break;
            case "EnemyBulletL":
                TargetPool = EnemyBulletL;
                break;
            case "BossBullet":
                TargetPool = BossBullet;
                break;
            case "PowerItem":
                TargetPool = PowerItem;
                break;
            case "BombItem":
                TargetPool = BombItem;
                break;
            case "FollowerItem":
                TargetPool = FollowerItem;
                break;
            case "Follower":
                TargetPool = Follower;
                break;
            case "BombEffect":
                TargetPool = BombEffect;
                break;
            case "ExplosionEffect":
                TargetPool = ExplosionEffect;
                break;
        }

        for(int i = 0; i < TargetPool.Length; i++)
        {
            if (!TargetPool[i].activeSelf)
            {
                TargetPool[i].SetActive(true);
                return TargetPool[i];
            }
        }
        return null;
    }
}
