using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrb : MonoBehaviour
{
    [SerializeField] int _healAmount;
    [SerializeField] List<GameObject> gameObjectsToDestroy;

    bool used;
    HPSystem player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HPSystem>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !used)
        {
            foreach (GameObject go in gameObjectsToDestroy)
            {
                go.SetActive(false);
            }
            used = true;
            player.Heal(_healAmount);
            SoundManager.PlaySound(SoundManager.Sound.HealOrb, transform.position);
        }
    }
}
