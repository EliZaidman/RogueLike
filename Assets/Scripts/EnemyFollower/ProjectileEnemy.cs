using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    Pooler objPooler;
    [HideInInspector] public GameObject currentBall;
    [HideInInspector] public List<GameObject> activeBalls = new List<GameObject>();
    public Transform bulletResPos;
    public float detectionRange;
    public LayerMask Layer;
    public float waitTime;
    void Start()
    {
        objPooler = Pooler.Instance;
        StartCoroutine("Shoot");
    }
    
    private IEnumerator Shoot()
    {
        while (true)
        {
        yield return new WaitForSeconds(waitTime);
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, Layer);

        if(colliders.Length > 0)
        currentBall = objPooler.SpawnFromPool("Clock",bulletResPos.position, bulletResPos.rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}