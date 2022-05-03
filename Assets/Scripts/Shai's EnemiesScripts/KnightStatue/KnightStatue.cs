using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatue : MonoBehaviour
{
    #region Properties
    [SerializeField]private string currentState = "Idle";
    public float speed = 6;
    public float agroRange = 5;
    public float attackRange = 2;
    public bool drawAgroRange = false;
    public bool drawAttackRange = false;
    [Header("Combat Stats")]
    public int health = 50;
    public int damage = 5;
    [Header("Ram Settings")]
    public float ramSpeed = 5;
    public float ramDistance = 5;
    public float ramChargeTime = 1.5f;
    [Tooltip("Height differential required for attacking")]
    public float heightDiff;
    public bool drawHeightDiff = false;


    private GameObject target;
    private float _distanceFromTarget;
    private float _heightDiff;
    #endregion

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CheckDistance();
        HandleStates();
    }

    #region State Machine
    void HandleStates()
    {
        switch (currentState)
        {
            case "Idle":
                Idle();
                break;

            case "Follow":
                FollowTarget();
                break;

            case "Attack":
                Attack();
                break;

            default:
                Debug.Log(gameObject.name + "Entered a state that doesnt exists");
                break;
        }
    }

    #region States
    void Attack()
    {

    }

    void FollowTarget()
    {

    }

    void Idle()
    {

    }
    #endregion

    #endregion

    #region Methods
    void FacePlayer()
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
    }

    void RamSignal()
    {

    }

    bool IsTargetInAttackRange()
    {
        if (_distanceFromTarget <= attackRange)
        {
            return true;
        }
        return false;
    }

    bool IsTargetDetected()
    {
        if (_distanceFromTarget <= agroRange)
        {
            return true;
        }
        return false;
    }

    //RayCast to check for drop? ask game designers

    void CheckDistance()
    {
        _distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
        _heightDiff = Mathf.Abs(transform.position.y - target.transform.position.y);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (drawAgroRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, agroRange);
        }
        if (drawAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        if (drawHeightDiff)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y+heightDiff, 0));
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y-heightDiff, 0));
        }
    }
    #endregion
}
