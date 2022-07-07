using System;
using UnityEngine;
using System.Collections.Generic;
using Spine.Unity;
using Random = UnityEngine.Random;
using System.Collections;

public class PlayerControllerV3 : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] SkeletonAnimation animationSkeleton;
    [SerializeField] AnimationReferenceAsset idle;
    [SerializeField] AnimationReferenceAsset run;
    [SerializeField] AnimationReferenceAsset dead;
    [SerializeField] AnimationReferenceAsset hit;
    [SerializeField] AnimationReferenceAsset jump;
    [SerializeField] string currentAnimation;
    [Header("")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public static PlayerControllerV3 Instance { get; set; }
    private FrameInputs _inputs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        bulletRegenBar.SetMaxHealth(mbRegenTime);
        _tempRegenTime = mbRegenTime;
        _currentMgCharges = maxMagicalBullets;
        _allPlats = GameObject.FindGameObjectsWithTag("Platform");
    }
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

        HandleWildMagic();

    }

    #region Inputs

    [SerializeField] private bool _facingLeft;

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
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _facingLeft = false;
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
            //transform.SetParent(_ground[0].transform);
        }
        else if (IsGrounded && !grounded)
        {
            IsGrounded = false;
            _timeLeftGrounded = Time.time;
            //transform.SetParent(null);
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

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (IsGrounded)
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerFootstep, transform.position);
            }

            if (_rb.velocity.x > 0) _inputs.X = 0;// Immediate stop and turn. Just feels better
            {
                StartCoroutine(LerpRotation(0.5f,-180));
                _inputs.X = Mathf.MoveTowards(_inputs.X, -1, acceleration * Time.deltaTime);
                SetAnimation(run, true, 1f);
            }

        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (IsGrounded)
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerFootstep, transform.position);
            }
            if (_rb.velocity.x < 0) _inputs.X = 0;
            {
                StartCoroutine(LerpRotation(0.5f,0));
                _inputs.X = Mathf.MoveTowards(_inputs.X, 1, acceleration * Time.deltaTime);
                SetAnimation(run, true, 1f);
            }
        }
        else
        {
            _inputs.X = Mathf.MoveTowards(_inputs.X, 0, acceleration * 2 * Time.deltaTime);
            SetAnimation(idle, false, 1f);

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
                if (!_hasJumped || _hasJumped && !_hasDoubleJumped)
                {
                    ExecuteJump(new Vector2(_rb.velocity.x, _jumpForce), _hasJumped); // Ground jump
                    SetAnimation(jump, false, 1);
                }
            }
        }

        void ExecuteJump(Vector3 dir, bool doubleJump = false)
        {
            _rb.velocity = dir;
            //_jumpLaunchPoof.up = _rb.velocity;
            //_jumpParticles.Play();
            _hasDoubleJumped = doubleJump;
            _hasJumped = true;
            if (!_hasDoubleJumped)
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerJump, transform.position);
            }
            else
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerDoubleJump, transform.position);
            }
        }

        // Fall faster and allow small jumps. _jumpVelocityFalloff is the point at which we start adding extra gravity. Using 0 causes floating
        if (_rb.velocity.y < _jumpVelocityFalloff || _rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rb.velocity += _fallMultiplier * Physics.gravity.y * Vector3.up * Time.deltaTime;
    }

    #endregion

    #region Dash

    [Header("Dash")] [SerializeField] private float _dashSpeed = 15;
    [SerializeField] private float _dashLength = 1;
    [SerializeField] private float dashCooldown = 1.5f;
    // [SerializeField] private ParticleSystem _dashParticles;
    //[SerializeField] private Transform _dashRing;
    [SerializeField] private float wallDetectorRange = 3;
    //[SerializeField] private ParticleSystem _dashVisual;

    public static event Action OnStartDashing, OnStopDashing;

    private bool _hasDashed;
    private bool _dashing;
    private bool _dashCdReady;
    private float _timeStartedDash;
    private float _dashCdTimer;
    private Vector3 _dashDir;


    private void HandleDashing()
    {
        DashCooldown();
        if (Input.GetKeyDown(KeyCode.Mouse1) && !_hasDashed && _dashCdReady)
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerDash);

            //_dashDir = new Vector3(_inputs.RawX, _inputs.RawY).normalized;
            //if (_dashDir == Vector3.zero) _dashDir = !_spriteRenderer.flipX ? Vector3.left : Vector3.right;
            if (_facingLeft)
            {
                _dashDir = Vector3.left;
            }
            else
            {
                _dashDir = Vector3.right;
            }
            //_dashRing.up = _dashDir;
            //_dashParticles.Play();
            _dashing = true;
            _hasDashed = true;
            _dashCdTimer = 0;
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
                //_rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y > 3 ? 3 : _rb.velocity.y);
                _rb.velocity = Vector3.zero;
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

    void DashCooldown()
    {
        _dashCdTimer += Time.deltaTime;
        if (_dashCdTimer >= dashCooldown)
        {
            _dashCdReady = true;
            _dashCdTimer = dashCooldown;
        }
        else
        {
            _dashCdReady = false;
        }
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
    [Header("Aiming and Shooting Setup")]
    [Header("Aiming and Shooting")]
    [SerializeField] Transform bulletPos;
    [SerializeField] Transform bulletPosAnchor;
    [SerializeField] Pooler pooler;
    [SerializeField] GameObject[] bulletsUI;
    [SerializeField] HPBar bulletRegenBar;
    GameObject currentObj;
    Vector3 lookPos;
    Vector3 shotDir;

    [Header("Shooting Settings")]
    [SerializeField] float bulletForce = 5;
    [SerializeField] int maxMagicalBullets = 3;
    [SerializeField] [Tooltip("MB = Magical Bullets")] int mbRegenTime = 3;

    int _currentMgCharges;
    float _mgRegenTimer;


    void HandleShooting()
    {
        ShotInput();
        MbRegen();
        BulletUI();
        BulletBar();
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
    void ShotInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_currentMgCharges > 0)
            {
                SoundManager.PlaySound(SoundManager.Sound.PlayerShot, transform.position);

                currentObj = pooler.SpawnFromPool("Hat", bulletPos.position, bulletPos.rotation);
                currentObj.GetComponent<Rigidbody>().velocity = shotDir * -bulletForce;
                _currentMgCharges--;
            }
            else
            {
                SoundManager.PlaySound(SoundManager.Sound.EmptyAmmo, transform.position);
                Debug.Log("No bullets");
            }
            _mgRegenTimer = 0;
        }
    }

    void MbRegen()
    {
        if (_mgRegenTimer < mbRegenTime && _currentMgCharges < maxMagicalBullets)
        {
            _mgRegenTimer += Time.deltaTime;
        }
        if (_mgRegenTimer >= mbRegenTime)
        {
            _currentMgCharges = maxMagicalBullets;
            _mgRegenTimer = 0;
        }
    }

    void BulletBar()
    {
        bulletRegenBar.SetHealth(_mgRegenTimer);
    }

    void BulletUI()
    {
        for (int i = 0; i < bulletsUI.Length; i++)
        {
            if (i < _currentMgCharges)
            {
                bulletsUI[i].SetActive(true);
            }
            else
            {
                bulletsUI[i].SetActive(false);
            }
        }
    }

    public void AddMgCharge(int toAdd)
    {
        _currentMgCharges += toAdd;
        if (_currentMgCharges >= maxMagicalBullets)
        {
            _currentMgCharges = maxMagicalBullets;
        }
    }
    void HandleRotation()
    {
        bulletPosAnchor.transform.LookAt(lookPos);
    }

    #endregion

    #region GoUpDownPlatforms

    [SerializeField] Collider[] _downPlats;
    [SerializeField] Collider[] _upPlats;

    void HandleGoUpDownPlatforms()
    {
        ResetPlatsCollision();

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
                if (plat.bounds.min.y > _collider.bounds.max.y && plat != null)
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
            _upPlats = Physics.OverlapSphere(new Vector3(transform.position.x, _collider.bounds.max.y), 0.5f);
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
            _downPlats = Physics.OverlapSphere(new Vector3(transform.position.x, _collider.bounds.min.y), 0.5f);
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

    GameObject[] _allPlats;
    Collider[] _nearbyPlats;

    void ResetPlatsCollision()
    {
        _nearbyPlats = Physics.OverlapSphere(transform.position, 2.3f);
        if (_nearbyPlats.Length == 1)
        {
            foreach (var plat in _allPlats)
            {
                Physics.IgnoreCollision(plat.GetComponent<Collider>(), _collider, false);
            }
        }
    }

    #endregion

    #region FaceDirection
    void HandleFaceDir()
    {
        if (bulletPos.transform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
    #endregion

    #region WildMagic

    [Header("WildMagic")]
    [SerializeField] float _wmDuration = 5;
    float _wmTimer;
    int _tempRegenTime;

    void HandleWildMagic()
    {
        if (_wmTimer > 0)
        {
            mbRegenTime = 0;
            _wmTimer -= Time.deltaTime;
        }
        else
        {
            mbRegenTime = _tempRegenTime;
        }
    }

    public void ActivateWildMagic()
    {
        _wmTimer = _wmDuration;
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

    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        animationSkeleton.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }


    public IEnumerator LerpRotation(float duration,int Ypos)
    {
        float t = 0;
        while (t < duration)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.position.x, Ypos, transform.position.z), t / duration);
            //print("Inside");

            //duration = 0;
        }

    }

}