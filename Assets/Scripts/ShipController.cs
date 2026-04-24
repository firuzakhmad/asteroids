using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : BoundedEntity
{
    private float m_turnInput;
    private float m_forwardInput;
    [SerializeField]
    private float m_turnSpeed;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private float m_maxSpeed;

    [SerializeField]
    private float m_maxBoostingSpeed;
    [SerializeField]
    private float m_boostingSpeed;

    [SerializeField]
    private float m_stoppingPower;

    [SerializeField]
    private GameObject m_bulletPrefab;
    
    [SerializeField]
    private float m_fireDelay;

    [SerializeField]
    private float m_fireCount;

    [SerializeField]
    private bool m_isFiring;

    [SerializeField]
    private bool m_isBoosting;

    [SerializeField]
    private GameObject m_shield;

    [SerializeField]
    private bool m_isDead;

    private bool m_isInvulnerable;

    [SerializeField]
    private float m_easeTime;

    [Header("Sound Effects")]
    [SerializeField]
    private SoundEffectHandler m_fireSound;
    [SerializeField]
    private SoundEffectHandler m_deathSound;
    [SerializeField]
    private SoundEffectHandler m_hitSound;

    [Header("Other References")]
    [SerializeField]
    private Material m_fullScreenEffectMat;

    [SerializeField]
    private AnimationCurve m_fullscreenEase;

    void OnMove(InputValue value)
    {
        Vector2 moveInputDirection = value.Get<Vector2>();

        m_turnInput = moveInputDirection.x;
        m_forwardInput = moveInputDirection.y;
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            m_isFiring = true;
            m_fireCount = m_fireDelay; 
        }
        else
        {
            m_isFiring = false;
        }
    }

    void OnBoost(InputValue value)
    {
        if (value.isPressed)
        {
            m_isBoosting = true;
        }
        else
        {
            m_isBoosting = false;
        }
    }

    // Update is called once per frame
    protected override void LateUpdate()
    {
        if (m_isDead)
            return;

        m_rigidbody.rotation -= (m_turnInput * (m_turnSpeed * 100.0f)) * Time.deltaTime;
        
        if (m_isBoosting)
        {
            m_rigidbody.AddRelativeForceY(m_boostingSpeed * 100.0f * Time.deltaTime);
        }

        if (m_forwardInput > 0)
        {
            
            m_rigidbody.AddRelativeForceY((m_forwardInput * (m_moveSpeed * 100.0f)) * Time.deltaTime);
            
        }
        else if (m_forwardInput < 0)
        {
            m_rigidbody.linearVelocity = Vector2.Lerp(
                m_rigidbody.linearVelocity, 
                Vector2.zero, 
                m_stoppingPower * Time.deltaTime);
        }

        if (m_isBoosting && m_rigidbody.linearVelocity.magnitude > m_maxBoostingSpeed)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * m_maxBoostingSpeed;
            
        }

        else if (! m_isBoosting && m_rigidbody.linearVelocity.magnitude > m_maxSpeed)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * m_maxSpeed;
        }

        base.LateUpdate();

        HandleShooting();

        if (!Mouse.current.leftButton.isPressed)
        {
            m_isFiring = false;
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(m_bulletPrefab);
        bullet.transform.position = transform.position + (transform.up * 3f);
        bullet.transform.up = transform.up;
    }

    void HandleShooting()
    {
        m_fireCount += Time.deltaTime;

        if (m_isFiring && m_fireCount >= m_fireDelay)
        {
            SpawnBullet();
            m_fireCount = 0;

            m_fireSound.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_isInvulnerable)
            return;

        if (collision.gameObject.TryGetComponent(out AsteroidController asteroid))
        {
            m_hitSound.Play();
            LoseHealth();
            StartCoroutine(HitRoutine());
        } 
    }

    protected override void OnEnabled()
    {
        GameEvents.Instance.onRetryEvent += OnRetry;
        base.OnEnabled();
    }

    private void OnRetry()
    {
        SetPlayerAsDead();
        StartCoroutine(RespawnPlayer());
    }
    protected override void OnDisabled()
    {
        GameEvents.Instance.onRetryEvent -= OnRetry;
        base.OnDisabled();
    }

    protected void SetPlayerAsDead()
    {
        m_isDead = true;
        m_spriteRenderer.enabled = false;
        m_colloder.enabled = false;
        m_rigidbody.simulated = false;
    }

    protected override void OnDie()
    {
        GameEvents.Instance.OnPlayerDie();

        m_deathSound.Play();

        SetPlayerAsDead();

        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.75f);

        m_rigidbody.position = Vector3.zero;
        m_rigidbody.linearVelocity = Vector2.zero;
        m_rigidbody.transform.position = Vector3.zero;
        m_rigidbody.rotation = 0;
        m_rigidbody.simulated = true;
        m_colloder.enabled = true;
        m_isDead = false;
        m_isInvulnerable = true;

        m_spriteRenderer.enabled = true;
        ResetHealth();
        m_shield.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        m_shield.SetActive(false);
        m_isInvulnerable = false;
    }

    private IEnumerator HitRoutine()
    {
        float amount = 0.0f;
        while (amount < 1.0f)
        {
            yield return new WaitForSeconds(m_easeTime/100);
            amount += 0.01f;
            m_fullScreenEffectMat.SetFloat("_Amount", m_fullscreenEase.Evaluate(amount));
        }
        while (amount > 0.0f)
        {
            yield return new WaitForSeconds(m_easeTime/100);
            amount -= 0.01f;
            m_fullScreenEffectMat.SetFloat("_Amount", m_fullscreenEase.Evaluate(amount));
        }
    }
}
