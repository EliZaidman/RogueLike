using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTimeController : MonoBehaviour
{
    [HideInInspector]public static EnemyTimeController Instance { get; set; }
    public float currentTimeScale;
    [Tooltip("Percentage of normal scale")][SerializeField][Range(0, 1)] float enemySlowScale;
    [SerializeField] float bulletTimeMaxCharge = 10;
    [SerializeField] float bulletTimeChargeDepletionRate = 1;
    [SerializeField] HPBar bulletTimeBar;
    [SerializeField] float _currentCharge;
    [SerializeField] bool _isSlowed;
    bool available;
    
    
    
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
        bulletTimeBar.SetMaxHealth(bulletTimeMaxCharge);
    }

    private void Update()
    {
        HandleBulletTime();
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddToCharge(10);
        }

        bulletTimeBar.SetHealth(_currentCharge);
    }


    void HandleBulletTime()
    {
        HandleCharge();
        HandleCurrentTimeScale();
        HandleIsSlowed();
        HandleAudio();
    }

    public void HandleAudio()
    {
        if (_isSlowed)
        {
            SoundManager.PlaySound(SoundManager.Sound.BulletTimeActive);
        }
        if (available)
        {
            SoundManager.PlaySound(SoundManager.Sound.BulletTimeAvailable);
            available = false;
        }
    }

    public void AddToCharge(int toAdd)
    {
        if (_currentCharge < bulletTimeMaxCharge)
        {
            _currentCharge += toAdd;
        }
        if (_currentCharge >= bulletTimeMaxCharge)
        {
            _currentCharge = bulletTimeMaxCharge;
            available = true;
        }
    }
    
    void HandleIsSlowed()
    {
        if (_currentCharge <= 0 && _isSlowed)
        {
            _isSlowed = false;
            _currentCharge = 0;
            Debug.Log("Out of charge");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_isSlowed)
            {
                _isSlowed = false;
            }
            else if (!_isSlowed)
            {
                _isSlowed = true;
            }
        }
    }

    void HandleCharge()
    {
        if (_isSlowed)
        {
            _currentCharge -= bulletTimeChargeDepletionRate / 100;
        }
    }

    void HandleCurrentTimeScale()
    {
        if (_isSlowed)
        {
            currentTimeScale = enemySlowScale;
        }
        else
        {
            currentTimeScale = 1;
        }
    }
}
