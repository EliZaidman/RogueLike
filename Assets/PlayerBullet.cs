using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision coll)
    {
        switch (coll.transform.tag)
        {
            case "Enemy":
                coll.collider.GetComponent<HPSystem>().TakeDamage(damage);
                gameObject.SetActive(false);
                PlayerControllerV3.Instance.AddMgCharge(1);
                break;
            case "KnightsShield":
                gameObject.SetActive(false);
                PlayerControllerV3.Instance.AddMgCharge(1);
                break;
            default:
                break;
        }
    }
}
