using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuckooBullets : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody rb;
    public float speed, destroyTime, rotateSpeed;
    public int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        Invoke("gameObject.SetActive(false)",destroyTime);
    }
    private void FixedUpdate()
    {
        Bullet();
    }

    private void Bullet()
    {
        Vector3 direction = (Vector3)_player.transform.position - rb.position;
        direction = direction.normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = new Vector3(0,0,rotateAmount * rotateSpeed * EnemyTimeController.Instance.currentTimeScale);
        rb.velocity = - transform.up * speed * EnemyTimeController.Instance.currentTimeScale;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameObject.SetActive(false);
            collision.collider.GetComponent<HPSystem>().TakeDamage(damage);
            Debug.Log("HIT");
        }else if (collision.transform.tag == "Enemy")
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.collider);
        }
        else
            gameObject.SetActive(false);
            Debug.Log("HIL");
    }
}
