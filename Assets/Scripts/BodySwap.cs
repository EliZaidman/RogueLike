using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySwap : MonoBehaviour
{
    public GameObject player;
    RougeController rougeController;
    GameObject _camera;

    private void OnEnable()
    {
        //gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        //gameObject.transform.position = Vector3.zero;
    }
    private void Start()
    {
        player = GameObject.Find("Player 2D");
        _camera = Camera.main.gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.GetComponent<RougeController>().enabled = false;
            collision.gameObject.GetComponent<RougeController>().enabled = true;
            _camera.transform.SetParent(collision.transform, false);
            Destroy(gameObject);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            Destroy(gameObject);

            //Destroy(gameObject);
            gameObject.SetActive(false);

            GameManager.instance.currentPlayer.Add(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.currentPlayer[0].GetComponent<RougeController>().enabled = false;
            player.GetComponent<RougeController>().enabled = true;
            _camera.transform.SetParent(collision.transform, false);

            gameObject.SetActive(false);


            Destroy(gameObject);

            //Destroy(gameObject);
            gameObject.SetActive(false);

        }
    }
}