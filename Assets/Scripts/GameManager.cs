using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> bulletsAmount = new List<GameObject>();
    public GameObject bullet;
    public int curNumber = 0;
    public List<GameObject> bulletPictures = new List<GameObject>();
    public bool addedBullet;
    public static GameManager instance;

    public GameObject _player;
    public GameObject templatePos;

    public float playerHp;


    void Awake()
    {
        MakeInstance();
    }
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    private void Update()
    {
        AddToList();


    }
    public void looseCondition()
    {
        if (playerHp <= 0)
        {
            Debug.Log("You loose!");
        }
    }

    public void AddToList()
    {

        if (GameObject.Find("bullet(Clone)") && addedBullet == false)
        {
            bullet = GameObject.Find("bullet(Clone)");
            bulletsAmount.Add(bullet);
            Debug.Log(bulletsAmount.Count);
            addedBullet = true;
        }

        else if (!GameObject.Find("bullet(Clone)"))
        {
            bulletsAmount.Remove(bullet);
        }
    }

    public void FindPicture(GameObject gameObject)
    {
        int p = 0;

        if (p < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                p++;
               gameObject.transform.position = bulletPictures[i-1].transform.position;
            }
        }
    }
}
