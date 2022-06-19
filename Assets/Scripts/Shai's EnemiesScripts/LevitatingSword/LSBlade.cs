using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSBlade : MonoBehaviour
{
    [SerializeField]LevitatingSword sword;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && sword.isAttacking) 
        {
            Debug.Log("Hit");
            collision.collider.GetComponent<HPSystem>().TakeDamage(sword.damage);
            //fill BulletTime charge
        }
    }


}
