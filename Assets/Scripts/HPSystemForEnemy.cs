using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystemForEnemy : MonoBehaviour
{
    public int hp;
    [SerializeField] int maxHP = 100;
    [SerializeField] HPBar hpBar;

    void Start()
    {
        hp = maxHP;
    }

    void Update()
    {
        if (hp > maxHP)
        {
            hp = maxHP;
        }
        Dead();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public void Heal(int heal)
    {
        hp += heal;
    }

    void Dead()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
