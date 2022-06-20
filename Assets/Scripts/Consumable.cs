using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [SerializeField] int hpAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "HP")
        {
            if (other.gameObject.tag == "Player")
            {
                //GameManager.instance.playerHp += 0.25f;
                other.GetComponent<HPSystem>().Heal(hpAmount);
                Destroy(this.gameObject);
            }
        }
    }
}
