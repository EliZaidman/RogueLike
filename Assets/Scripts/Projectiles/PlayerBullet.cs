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
                PlayerControllerV3.Instance.AddMgCharge(1);
                gameObject.SetActive(false);
                break;
            case "KnightsShield":
                PlayerControllerV3.Instance.AddMgCharge(1);
                gameObject.SetActive(false);
                break;
            case "Player":
                Physics.IgnoreCollision(GetComponent<Collider>(), coll.collider);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }
}
