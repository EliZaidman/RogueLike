using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> bulletsAmount = new List<GameObject>();
    [HideInInspector] public int curNumber = 0;
    [HideInInspector] public GameObject bullet;
    [HideInInspector] public bool addedBullet;
    [HideInInspector] public static GameManager instance;

    private TextMeshProUGUI hpNumber;

    public float playerHp;
    public List<GameObject> bulletPictures = new List<GameObject>();
    public GameObject _player;

    public Slider _PlayerHp;
    void Awake()
    {
        MakeInstance();
        SoundManager.Initialize();
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
        Death();

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


    public void Death()
    {
        if (playerHp <= 0)
        {
        //DO SHIT
        }
    }
}
