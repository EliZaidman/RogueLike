using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    [SerializeField] int _healAmount;
    HPSystem player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HPSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Heal(_healAmount);
            gameObject.SetActive(false);
            SoundManager.PlaySound(SoundManager.Sound.HealOrb, transform.position);
        }
    }
}
