using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatue : MonoBehaviour
{
    #region Properties
    [SerializeField]private states _currentState = states.Idle;
    public BoxCollider shield;
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
    public float knockBackStrength = 8;
    public float ramCooldown = 3;
    [Tooltip("Height differential required for attacking")]
    public float heightDiff;
    public bool drawHeightDiff = false;
    [HideInInspector]public bool isRamming;


    private enum states { Idle, Follow, Attack }
    private GameObject target;
    private float _distanceFromTarget;
    private float _heightDiff;
    private bool _isFacingRight;
    private float _ramTimer;
    private float _ramCooldownTimer = 0;
    private Rigidbody _rb;
    private Animator _animator;
    #endregion

    private void Start()
    {
        
        _ramTimer = ramDistance;
        target = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Ram();
        CheckDistance();
        HandleStates();
    }

    #region State Machine
    void HandleStates()
    {
        switch (_currentState)
        {
            case states.Idle:
                Idle();
                break;

            case states.Follow:
                FollowTarget();
                break;

            case states.Attack:
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
        if (!isRamming)
        {
            _ramCooldownTimer -= Time.deltaTime;
            FacePlayer();
        }
        if (_ramCooldownTimer <= 0 && !isRamming)
        {
            _ramCooldownTimer = ramCooldown;
            RamCharge();
        }
        if (isRamming)
        {
            _animator.SetBool("Charge", false);
        }
        if (!IsTargetInAttackRange())
        {
            ChangeState(states.Follow);
        }
    }

    void FollowTarget()
    {
       // _animator.SetBool("Charge", false);
        FacePlayer();
        _rb.velocity = transform.right * speed;
        if (IsTargetInAttackRange())
        {
            ChangeState(states.Attack);
        }
    }

    void Idle()
    {
        if (IsTargetDetected())
        {
            ChangeState(states.Follow);
        }
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

    void RamCharge()
    {
        _animator.SetBool("Charge", true);
    }

    public void SetRamTrue()
    {
        isRamming = true;
    }

    void Ram()
    {
        if (isRamming)
        {
            _ramTimer -= Time.deltaTime;
            _rb.velocity = transform.right * ramSpeed;
            if (_ramTimer <= 0)
            {
                isRamming = false;
                _ramTimer = ramDistance;
                _currentState = states.Follow;
            }
        }
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

    void ChangeState(states state)
    {
        _currentState = state;
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
