using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{


    private Rigidbody _rb;
    private KnightStatue knight;
    
    void Start()
    {
        knight = transform.parent.GetComponent<KnightStatue>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _rb = collision.collider.GetComponent<Rigidbody>();
        if (collision.transform.tag == "Player" && knight.isRamming)
        {
            Vector3 direction = transform.position - collision.transform.position;
            direction.y = direction.y * knight.knockBackStrength + 20;
            _rb.AddForce(direction.normalized * knight.knockBackStrength, ForceMode.Impulse);
            knight._ramTimer = 0;
            knight.ChangeState(KnightStatue.states.Recover);
            //collision.collider.GetComponent<HPSystem>().TakeDamage(knight.damage);
            Debug.Log("Hit");
        }
    }

}
