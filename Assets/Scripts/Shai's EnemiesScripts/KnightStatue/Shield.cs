using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{


    private Rigidbody _playerRb;
    private HPSystem _playerHpSystem;
    private KnightStatue knight;
    
    void Start()
    {
        knight = transform.parent.GetComponent<KnightStatue>();
        _playerRb = PlayerControllerV3.Instance.GetComponent<Rigidbody>();
        _playerHpSystem = PlayerControllerV3.Instance.GetComponent<HPSystem>();
    }

    //private void OnTriggerEnter(Collision collision)
    //{
    //    _rb = collision.collider.GetComponent<Rigidbody>();
    //    if (collision.transform.tag == "Player" && knight.isRamming)
    //    {
    //        Vector3 direction = transform.position - collision.transform.position;
    //        direction.y = direction.y * knight.knockBackStrength + 20;
    //        _rb.AddForce(direction.normalized * knight.knockBackStrength, ForceMode.Impulse);
    //        knight._ramTimer = 0;
    //        knight.ChangeState(KnightStatue.states.Recover);
    //        //collision.collider.GetComponent<HPSystem>().TakeDamage(knight.damage);
    //        Debug.Log("Hit");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player" && knight.isRamming)
        {
            //Vector3 direction = transform.position - other.transform.position;
            //direction.y = direction.y * knight.knockBackStrength + 20;
            //_playerRb.AddForce(direction.normalized * knight.knockBackStrength, ForceMode.Impulse);
            knight._ramTimer = 0;
            //knight.ChangeState(KnightStatue.states.Recover);
            _playerHpSystem.TakeDamage(knight.damage);
            Debug.Log("Hit");
        }
        else if(other.tag == "Hat")
        {
            Debug.Log("BulletHitShield");
            other.gameObject.SetActive(false);
        }
    }
}
