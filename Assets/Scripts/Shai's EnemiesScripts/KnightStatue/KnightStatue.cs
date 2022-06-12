using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStatue : MonoBehaviour
{
    #region Properties
    [Header("General Settings")]
    [SerializeField]private states _currentState = states.Idle;
    public BoxCollider shield;
    [SerializeField] float speed = 6;
    [SerializeField] float agroRange = 5;
    [SerializeField] float attackRange = 2;
    [SerializeField] bool drawAgroRange = false;
    [SerializeField] bool drawAttackRange = false;
    [Header("Combat Stats")]
    public int health = 50;
    public int damage = 5;
    [Header("Ram Settings")]
    [SerializeField] float ramSpeed = 5;
    [SerializeField] float ramDistance = 5;
    public float knockBackStrength = 8;
    [SerializeField] float ramCooldown = 4;
    [SerializeField] float recoveryTime = 3;
    [Tooltip("Height differential required for attacking")]
    [SerializeField] float heightDiff;
    [SerializeField] bool drawHeightDiff = false;
    [Header("Platform Check")]
    [SerializeField] Transform platCheck;
    [SerializeField] bool drawPlatCheck = false;
    private float platCheckRange = 3;
    [HideInInspector]public bool isRamming;


    [HideInInspector]public enum states { Idle, Follow, Attack, Recover }
    private GameObject target;
    private float _distanceFromTarget;
    private float _heightDiff;
    [HideInInspector]public float _ramTimer;
    private float _ramCooldownTimer = 0;
    private bool _isChargingRam;
    private bool _canRam;
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
        if (!isRamming)
        {
            _ramCooldownTimer -= Time.deltaTime;
        }
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

            case states.Recover:
                Recover();
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
            FacePlayer();
        }
        if (_ramCooldownTimer <= 0 && !isRamming && _heightDiff < heightDiff)
        {
            _ramCooldownTimer = ramCooldown;
            if (!isRamming || !_isChargingRam)
            {
               RamCharge();
            }
        }
        if (isRamming)
        {
            _animator.SetBool("Charge", false);
        }
        if (!IsTargetInAttackRange() && !_isChargingRam && !isRamming)
        {
            ChangeState(states.Follow);
        }
    }

    void FollowTarget()
    {
        if (!isRamming)
        {
            FacePlayer();
        }
        if (IsTargetInAttackRange())
        {
            ChangeState(states.Attack);
        }
        if (Check4EndOfPlatform() && !_isChargingRam)
        {
           _rb.velocity = transform.right * speed * EnemyTimeController.Instance.currentTimeScale;
        }
    }

    void Idle()
    {
        if (IsTargetDetected())
        {
            ChangeState(states.Follow);
        }
    }

    void Recover()
    {
        //Recover anim
        StartCoroutine(Recover2Follow());
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
        if (!isRamming && !_isChargingRam && Check4EndOfPlatform())
        {
            _animator.SetBool("Charge", true);
        }
    }

    void Ram()
    {
        if (isRamming)
        {
            _ramTimer -= Time.deltaTime * EnemyTimeController.Instance.currentTimeScale;
            _rb.velocity = transform.right * ramSpeed * EnemyTimeController.Instance.currentTimeScale;
            if (_ramTimer <= 0)
            {
                isRamming = false;
                _isChargingRam = false;
                _ramTimer = ramDistance;
                _rb.velocity = Vector3.zero;
                _currentState = states.Recover;
            }
        }
    }

    IEnumerator Recover2Follow()
    {
        yield return new WaitForSeconds(recoveryTime);
        ChangeState(states.Follow);
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



    /// <summary>
    /// returns false if near an end of a platform
    /// </summary>
    /// <returns></returns>
    bool Check4EndOfPlatform()
    {
        Collider[] colliders = Physics.OverlapSphere(platCheck.position, platCheckRange);
        foreach (var item in colliders)
        {
            if (item.tag == "Platform")
            {
                return true;
            }
        }
        return false;
    }

    void CheckDistance()
    {
        _distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
        _heightDiff = Mathf.Abs(transform.position.y - target.transform.position.y);
    }

    public void ChangeState(states state)
    {
        _currentState = state;
    }

    #endregion

    #region AnimationFuncs
    public void SetRamTrue()
    {
        isRamming = true;
    }

    public void SetChargeTrue()
    {
        _isChargingRam = true;
    }

    public void SetChargeFalse()
    {
        _isChargingRam = false;
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
        if (drawPlatCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(platCheck.position, platCheckRange);
        }
    }

#endregion

}
