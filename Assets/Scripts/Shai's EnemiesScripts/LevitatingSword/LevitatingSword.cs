using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class LevitatingSword : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, attack;
    public string currentAnimation;

    [Header("General")]
    [SerializeField] float agroRange = 10; 
    [SerializeField] float attackRange = 3; 
    [SerializeField] float speed = 10;
    [SerializeField] Transform platChecker;
    [SerializeField] Collider blade;
    [SerializeField] int[] layersToIgnore;


    private GameObject target;
    private Rigidbody _rb;
    private float _distanceFromTarget;

    void Start()
    {
        foreach (var layer in layersToIgnore)
        {
            Physics.IgnoreLayerCollision(14, layer);
        }
        //Physics.IgnoreCollision(SwordStand, PlayerControllerV3.Instance.GetComponent<Collider>()); 
        target = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        attackTimer = timeBetweenAttacks;
    }

    void Update()
    {
        CheckDistanceFromTargert();
        HandleStateMachine();
        StayOnPlatform();
        AttackTimer();
    }

    


    #region StateMachine
    [HideInInspector] public enum states { Idle, Follow, Attack }
    [Header("StateMachine")]
    public states _currentState;

    void HandleStateMachine()
    {
        switch (_currentState)
        {
            case states.Idle:
                Idle();
                SetAnimation(idle, true, 1f);
                break;

            case states.Follow:
                FollowTarget();
                SetAnimation(walking, true, 1f);
                break;

            case states.Attack:
                Attack();
                break;

            default:
                break;
        }
    }

    #region State: Attack

    [Header("Attack")]
    [SerializeField] float timeBetweenAttacks = 2;
    [SerializeField] float damageWindowStart = 1;
    [SerializeField] float damageWindowLength = 0.3f;
    public int damage;
    public bool isAttacking;

    public float attackTimer;
    
    void Attack()
    {
        if (!isAttacking && IsTargetInAttackRange())
        {
            Strike();
        }
        if (!IsTargetInAttackRange() && !isAttacking)
        {
            ChangeState(states.Follow);
        }
    }

    void Strike()
    {
        if (attackTimer >= timeBetweenAttacks)
        {
            SetAnimation(attack, true, 1f * EnemyTimeController.Instance.currentTimeScale);
            Invoke("StopAttackAnimation", attack.Animation.Duration);
            Debug.Log("Attacked");
            if (EnemyTimeController.Instance.currentTimeScale == 1)
            {
                Invoke("StartDamageWindow", damageWindowStart);
                Invoke("CloseDamageWindow", damageWindowStart + damageWindowLength);
            }
            else
            {
                Invoke("StartDamageWindow", damageWindowStart + 3);
                Invoke("CloseDamageWindow", damageWindowStart + 3 + damageWindowLength);
            }
            
            attackTimer = 0;
        }
    }

    void StartDamageWindow()
    {
        isAttacking = true;
    }

    void CloseDamageWindow()
    {
        isAttacking = false;
    }

    private void StopAttackAnimation()
    {
        SetAnimation(attack, false, 1f * EnemyTimeController.Instance.currentTimeScale);
    }

    void AttackTimer()
    {
        if (attackTimer < timeBetweenAttacks)
        {
            attackTimer += Time.deltaTime;
        }
    }

    #endregion


    [SerializeField] float distanceToFacePlayer;
    void FollowTarget()
    {
        if (Mathf.Abs((transform.position - PlayerControllerV3.Instance.transform.position).x) > distanceToFacePlayer)
        {
            FacePlayer();
            if (!IsNearEndOfPlatform()) //moves towards target
            {
                _rb.velocity = -transform.right * speed * EnemyTimeController.Instance.currentTimeScale;
            }
        }
        if (IsTargetInAttackRange()) //switches to attack state
        {
            ChangeState(states.Attack);
        }
        if (!IsTargetDetected())
        {
            ChangeState(states.Idle);
        }
    }

    void Idle()
    {
        if (IsTargetDetected())// if target in agro range, switch to follow state
        {
            ChangeState(states.Follow);
        }
    }

    public void ChangeState(states state)
    {
        _currentState = state;
    }

    #endregion


    #region DistanceCheckers
    /// <summary>
    /// Returns true if targte is in attack range
    /// </summary>
    /// <returns></returns>
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

    void CheckDistanceFromTargert()
    {
        _distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
    }
    #endregion


    #region MiscMethods
    void FacePlayer() // Rotates the enemy 180 degrees to face the player
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    void StayOnPlatform() // Stops moving if near the end of a platform
    {
        if (IsNearEndOfPlatform())
        {
            _rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// returns true if near an end of a platform
    /// </summary>
    /// <returns></returns>
    bool IsNearEndOfPlatform()
    {
        Collider[] colliders = Physics.OverlapSphere(platChecker.position, 0.5f);
        foreach (var item in colliders)
        {
            if (item.tag == "Platform")
            {
                return false;
            }
        }
        return true;
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    #endregion


    #region Gizmos
    [Header("Gizmos")]
    [SerializeField] bool drawAgroRange;
    [SerializeField] bool drawAttackRange;
    [SerializeField] bool drawPlatCheck;

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

        if (drawPlatCheck)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(platChecker.position, 0.5f);
        }

    }
    #endregion
}
