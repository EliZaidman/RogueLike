using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildMagicOrb : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectsToDestroy;
    bool used;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !used)
        {
            foreach (GameObject obj in gameObjectsToDestroy)
            {
                obj.SetActive(false);
            }
            PlayerControllerV3.Instance.ActivateWildMagic();
            used = true;
        }
    }
}
