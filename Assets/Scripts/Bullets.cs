using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private GameObject _player;
    public float speed;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        Bullet();
        Invoke("gameObject.SetActive(false)", destroyTime);
    }
    private void Bullet()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameObject.SetActive(false);
            Debug.Log("HIT");
        }
        else
            gameObject.SetActive(false);
            Debug.Log("HIL");
    }
}
