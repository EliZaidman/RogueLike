using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public int hp;
    [SerializeField] int maxHP = 12;
    [SerializeField] HPBar hpBar;
    [SerializeField] PauseMenu menu;
    [SerializeField] GameObject[] uiToDisable;
    [SerializeField] GameObject deadText;
    [SerializeField] int nearDeath = 2;

    void Start()
    {
        hp = maxHP;
        hpBar.SetMaxHealth(maxHP);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        if (hp <= nearDeath)
        {
            //SoundManager.PlaySound(SoundManager.Sound.PlayerAlmostDead, transform.position);
        }
        Dead();
    }

   public void TakeDamage(int damage)
    {
        hp -= damage;
        hpBar.SetHealth(hp);
    }

    public void Heal(int heal)
    {
        hp += heal;
        hpBar.SetHealth(hp);
    }

    void Dead()
    {
        if (hp <= 0)
        {
            menu.Pause();
            deadText.SetActive(true);
            SoundManager.PlaySound(SoundManager.Sound.PlayerDeath);
            foreach (GameObject go in uiToDisable)
            {
                go.SetActive(false);
            }
            Debug.Log("Player dead");
        }
    }
}
