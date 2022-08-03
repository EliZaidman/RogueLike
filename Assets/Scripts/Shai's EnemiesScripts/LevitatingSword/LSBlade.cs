using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSBlade : MonoBehaviour
{
    [SerializeField]LevitatingSword sword;
    [SerializeField] HPSystemForEnemy hpSystem;

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && sword.isStriking)
        {
            Debug.Log("SwordHit");
            other.GetComponent<Collider>().GetComponent<HPSystem>().TakeDamage(sword.damage);
            PlayerControllerV3.Instance.GetComponent<HPSystem>().TakeDamage(sword.damage);
            SoundManager.PlaySound(SoundManager.Sound.SwordAttack);
            Debug.Log("SwordHitPlayer");
            //sword.isStriking = false;
            sword.attackTimer = 0;
        }

        if (other.tag == "Hat")
        {
            hpSystem.TakeDamage(other.GetComponent<PlayerBullet>().damage);
            PlayerControllerV3.Instance.AddMgCharge(1);
            other.gameObject.SetActive(false);
            Debug.Log("SwordTookDamage");
        }
    }


}
