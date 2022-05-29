using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerControllerV3: MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private FrameInputs _inputs;

   
    private void Update()
    {

        GatherInputs();

        HandleGrounding();

        HandleWalking();

        HandleJumping();

        HandleDashing();

        HandleAiming();

        HandleShooting();

        HandleRotation();

        HandleGoUpDownPlatforms();

        HandleFaceDir();
    }

    #region Inputs

    [SerializeField]private bool _facingLeft;

    private void GatherInputs()
    {
        _inputs.RawX = (int)Input.GetAxisRaw("Horizontal");
        _inputs.RawY = (int)Input.GetAxisRaw("Vertical");
        _inputs.X = Input.GetAxis("Horizontal");
        _inputs.Y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.A))
        {
            _facingLeft = true;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            _facingLeft= false;
        }
    }



    #endregion

    #region Detection

    [Header("Detection")] [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _grounderOffset = -1, _grounderRadius = 0.2f;
    public bool IsGrounded;
    public static event Action OnTouchedGround;

    private readonly Collider[] _ground = new Collider[1];

    private void HandleGrounding()
    {
        // Grounder
        var grounded = Physics.OverlapSphereNonAlloc(transform.position + new Vector3(0, _grounderOffset), _grounderRadius, _ground, _groundMask) > 0;

        if (!IsGrounded && grounded)
        {
            IsGrounded = true;
            _hasDashed = false;
            _hasJumped = false;
            _currentMovementLerpSpeed = 100;
            OnTouchedGround?.Invoke();
            transform.SetParent(_ground[0].transform);
        }
        else if (IsGrounded && !grounded)
        {
            IsGrounded = false;
            _timeLeftGrounded = Time.time;
            transform.SetParent(null);
        }
    }

    private void DrawGrounderGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, _grounderOffset), _grounderRadius);
    }

    private void OnDrawGizmos()
    {
        DrawGrounderGizmos();
    }

    #endregion

    #region Walking

    [Header("Walking")] [SerializeField] private float _walkSpeed = 4;
    [SerializeField] private float _acceleration = 2;
    [SerializeField] private float _currentMovementLerpSpeed = 100;

    private void HandleWalking()
    {
        if (_dashing) return;
        // This can be done using just X & Y input as they lerp to max values, but this gives greater control over velocity acceleration
        var acceleration = IsGrounded ? _acceleration : _acceleration * 0.5f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (_rb.velocity.x > 0) _inputs.X = 0; // Immediate stop and turn. Just feels better
            _inputs.X = Mathf.MoveTowards(_inputs.X, -1, acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (_rb.velocity.x < 0) _inputs.X = 0;
            _inputs.X = Mathf.MoveTowards(_inputs.X, 1, acceleration * Time.deltaTime);
        }
        else
        {
            _inputs.X = Mathf.MoveTowards(_inputs.X, 0, acceleration * 2 * Time.deltaTime);
        }

        var idealVel = new Vector3(_inputs.X * _walkSpeed, _rb.velocity.y);
        // _currentMovementLerpSpeed should be set to something crazy high to be effectively instant. But slowed down after a wall jump and slowly released
        _rb.velocity = Vector3.MoveTowards(_rb.velocity, idealVel, _currentMovementLerpSpeed * Time.deltaTime);

    }

    #endregion

    #region Jumping

    [Header("Jumping")] [SerializeField] private float _jumpForce = 15;
    [SerializeField] private float _fallMultiplier = 7;
    [SerializeField] private float _jumpVelocityFalloff = 8;
   // [SerializeField] private ParticleSystem _jumpParticles;
    //[SerializeField] private Transform _jumpLaunchPoof;
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private bool _enableDoubleJump = true;
    private float _timeLeftGrounded = -10;
    private bool _hasJumped;
    private bool _hasDoubleJumped;

    private void HandleJumping()
    {
        if (_dashing) return;
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
             if (IsGrounded || Time.time < _timeLeftGrounded + _coyoteTime || _enableDoubleJump && !_hasDoubleJumped)
            {
                if (!_hasJumped || _hasJumped && !_hasDoubleJumped) ExecuteJump(new Vector2(_rb.velocity.x, _jumpForce), _hasJumped); // Ground jump
            }
        }

        void ExecuteJump(Vector3 dir, bool doubleJump = false)
        {
            _rb.velocity = dir;
            //_jumpLaunchPoof.up = _rb.velocity;
            //_jumpParticles.Play();
            _hasDoubleJumped = doubleJump;
            _hasJumped = true;
        }

        // Fall faster and allow small jumps. _jumpVelocityFalloff is the point at which we start adding extra gravity. Using 0 causes floating
        if (_rb.velocity.y < _jumpVelocityFalloff || _rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rb.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
    }

    #endregion

    #region Dash

    [Header("Dash")] [SerializeField] private float _dashSpeed = 15;
    [SerializeField] private float _dashLength = 1;
   // [SerializeField] private ParticleSystem _dashParticles;
    //[SerializeField] private Transform _dashRing;
    [SerializeField] private float wallDetectorRange = 3;
    //[SerializeField] private ParticleSystem _dashVisual;

    public static event Action OnStartDashing, OnStopDashing;

    private bool _hasDashed;
    private bool _dashing;
    private float _timeStartedDash;
    private Vector3 _dashDir;

    private void HandleDashing()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1) && !_hasDashed)
        {
            _dashDir = new Vector3(_inputs.RawX, _inputs.RawY).normalized;
            if (_dashDir == Vector3.zero) _dashDir = !_spriteRenderer.flipX ? Vector3.left : Vector3.right;
            //_dashRing.up = _dashDir;
            //_dashParticles.Play();
            _dashing = true;
            _hasDashed = true;
            _timeStartedDash = Time.time;
            _rb.useGravity = false;
            // _dashVisual.Play();
            OnStartDashing?.Invoke();
        }

        if (_dashing)
        {
            _rb.velocity = _dashDir * _dashSpeed;
            if (!isWallDetected())
            {
                _collider.enabled = false;
            }
            else
            {
                _collider.enabled = true;
            }

            if (Time.time >= _timeStartedDash + _dashLength || isWallDetected())
            {
               // _dashParticles.Stop();
                _dashing = false;
                // Clamp the velocity so they don't keep shooting off
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y > 3 ? 3 : _rb.velocity.y);
                _rb.useGravity = true;
                if (IsGrounded) _hasDashed = false;
                _collider.enabled = true;
                // _dashVisual.Stop();
                OnStopDashing?.Invoke();
            }
        }
    }

    private bool isWallDetected()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, wallDetectorRange);
        foreach (Collider collider in colls)
        {
            if (collider.tag == "Platform") return true;
        }
        return false;
    }
    
    #endregion

    #region Impacts

    //[Header("Collisions")]
    //[SerializeField]private ParticleSystem _impactParticles;

    //[SerializeField] private GameObject _deathExplosion;
    //[SerializeField] private float _minImpactForce = 2;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.relativeVelocity.magnitude > _minImpactForce && IsGrounded) _impactParticles.Play();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Death"))
    //    {
    //        Instantiate(_deathExplosion, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //    }

    //    _hasDashed = false;
    //}

    #endregion

    #region Aiming+Shooting

    [Header("Aiming and Shooting")]
    public float bulletForce = 5;
    public Transform bulletPos;
    public Transform bulletPosAnchor;
    [SerializeField]Pooler pooler;
    GameObject currentObj;
    Vector3 lookPos;
    [SerializeField]Vector3 shotDir;


    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentObj = pooler.SpawnFromPool("Hat", bulletPos.position, bulletPos.rotation);
            currentObj.GetComponent<Rigidbody>().velocity = shotDir * -bulletForce;
        }
    }
    void HandleAiming()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 _lookPos = hit.point;
            _lookPos.z = transform.position.z;
            lookPos = _lookPos;
        }
        shotDir = (transform.position - lookPos).normalized;
    }

    void HandleRotation()
    {
        bulletPosAnchor.transform.LookAt(lookPos);
    }

    #endregion

    #region GoUpDownPlatforms

    Collider[] _downPlats;
    Collider[] _upPlats;

    void HandleGoUpDownPlatforms()
    {
        DropThroughPlat();

        JumpThroughPlat();
    }

    void DropThroughPlat()
    {
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            //Checks for platfroms below player and ignores collision with it if the conditions are right
            _downPlats = Physics.OverlapSphere(new Vector3(transform.position.x, _collider.bounds.min.y), 0.3f);
            foreach (var plat in _downPlats)
            {
                if (plat.tag == "Platform" && plat.GetComponent<TwoWayPlatform>() != null)
                {
                    if (plat.GetComponent<TwoWayPlatform>()._dropThrough)
                    {
                        Physics.IgnoreCollision(_collider, plat);
                    }
                }
            }
        }
        if (_downPlats != null)
        {
            //disables ignoreCollision with previous platforms
            foreach (var plat in _downPlats)
            {
                if (plat.bounds.min.y > _collider.bounds.max.y)
                {
                    Physics.IgnoreCollision(_collider, plat, false);
                }
            }    
        }
    }

    void JumpThroughPlat()
    {
        if (_rb.velocity.y > 0)//Checks for platfroms above player while jumping and ignores collision with it if the conditions are right
        {
            _upPlats = Physics.OverlapSphere(new Vector3(transform.position.x, _collider.bounds.max.y), 0.3f);
            foreach (var plat in _upPlats)
            {
                if (plat.tag == "Platform" && plat.GetComponent<TwoWayPlatform>() != null)
                {
                    if (plat.GetComponent<TwoWayPlatform>()._jumpThrough)
                    {
                        Physics.IgnoreCollision(_collider, plat);
                    }
                }
            }
            _downPlats = Physics.OverlapSphere(new Vector3(transform.position.x, _collider.bounds.min.y), 0.3f);
            if (_downPlats != null)
            {
                foreach (var plat in _downPlats)
                {
                    if (plat.bounds.max.y < _collider.bounds.min.y)
                    {
                        Physics.IgnoreCollision(_collider, plat, false);
                    }
                }

            }
        }
        if (_upPlats != null)
        {
            //disables ignoreCollision with previous platforms
            foreach (var plat in _upPlats)
            {
                if (plat.bounds.min.y > _collider.bounds.max.y)
                {
                    Physics.IgnoreCollision(_collider, plat, false);
                }
            }
        }
    }

    #endregion

    #region FaceDirection
    void HandleFaceDir()
    {
        if (bulletPos.transform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }
    #endregion

    #region Gizmos

    [Header("Gizmos")]
    [SerializeField] bool _drawGizmos;
    private void OnDrawGizmosSelected()
    {
        if (_drawGizmos)
        {
          DrawMinPos();
        }
    }

    void DrawMinPos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, _collider.bounds.min.y), 0.3f);
    }
    #endregion


    private struct FrameInputs
    {
        public float X, Y;
        public int RawX, RawY;
    }

}