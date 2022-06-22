using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildMagicOrb : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerControllerV3.Instance.ActivateWildMagic();
            gameObject.SetActive(false);
        }
    }
}
