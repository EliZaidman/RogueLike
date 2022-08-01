using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision coll)
    {
        switch (coll.collider.tag)
        {
            case "Enemy":
                gameObject.SetActive(false);
                Debug.Log("Hit Enemy");
                coll.collider.GetComponent<HPSystemForEnemy>().TakeDamage(damage);
                PlayerControllerV3.Instance.AddMgCharge(1);
                break;
            case "KnightsShield":
                Debug.Log("shield");
                PlayerControllerV3.Instance.AddMgCharge(1);
                gameObject.SetActive(false);
                break;
            case "Player":
                Physics.IgnoreCollision(GetComponent<Collider>(), coll.collider);
                break;
            default:
                Debug.Log("noting");
                gameObject.SetActive(false);
                break;
        }
    }
}
