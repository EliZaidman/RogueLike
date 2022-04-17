using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "HP")
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.instance.playerHp += 0.25f;
                Destroy(gameObject);
            }
        }
    }
}
