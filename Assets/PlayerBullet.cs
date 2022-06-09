using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Enemy")
        coll.collider.GetComponent<HPSystem>().TakeDamage(damage);
        gameObject.SetActive(false);
    }
}
