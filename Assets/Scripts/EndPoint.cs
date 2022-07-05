using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameObject endText;
    [SerializeField] GameObject[] uiTohide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            endText.SetActive(true);
            pauseMenu.Pause();
            foreach (GameObject go in uiTohide)
            {
                go.SetActive(false);
            }
        }
    }
}
