using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public int hp;
    public int maxHP = 100;

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

    void Heal(int heal)
    {
        hp += heal;
    }

    void Dead()
    {
        if (gameObject.tag == "Enemy" && hp <= 0)
        {
            Destroy(this.gameObject);
        }
        if (gameObject.tag == "Player" && hp <= 0)
        {
            Debug.Log("Player dead");
        }
    }
}
