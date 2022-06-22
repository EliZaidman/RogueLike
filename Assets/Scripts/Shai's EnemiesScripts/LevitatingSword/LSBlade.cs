using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSBlade : MonoBehaviour
{
    [SerializeField]LevitatingSword sword;
    [SerializeField] HPSystemForEnemy hpSystem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && sword.isAttacking) 
        {
            Debug.Log("Hit");
            collision.collider.GetComponent<HPSystem>().TakeDamage(sword.damage);
            //fill BulletTime charge
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && sword.isAttacking)
        {
            Debug.Log("SwordHit");
            other.GetComponent<Collider>().GetComponent<HPSystem>().TakeDamage(sword.damage);
            //fill BulletTime charge
        }
        else if (other.tag == "Hat")
        {
            hpSystem.TakeDamage(other.GetComponent<PlayerBullet>().damage);
            Debug.Log("SwordTookDamage");
        }
    }


}
