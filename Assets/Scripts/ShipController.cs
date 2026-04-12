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
    private bool m_isDead;

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

    // Update is called once per frame
    protected override void LateUpdate()
    {
        if (m_isDead)
            return;

        m_rigidbody.rotation -= (m_turnInput * (m_turnSpeed * 100.0f)) * Time.deltaTime;
        
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

        if (m_rigidbody.linearVelocity.magnitude > m_maxSpeed)
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
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out AsteroidController asteroid))
        {
            LoseHealth();
        } 
    }

    protected override void OnDie()
    {
        GameEvents.Instance.OnPlayerDie();

        m_isDead = true;
        m_spriteRenderer.enabled = false;
        m_colloder.enabled = false;
        m_rigidbody.simulated = false;

        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        m_rigidbody.position = Vector3.zero;
        m_rigidbody.linearVelocity = Vector2.zero;
        m_rigidbody.transform.position = Vector3.zero;
        m_rigidbody.rotation = 0;

        m_spriteRenderer.enabled = true;
        ResetHealth();

        yield return new WaitForSeconds(0.5f);
        m_isDead = false;
        m_rigidbody.simulated = true;

        yield return new WaitForSeconds(2.0f);
        m_colloder.enabled = true;

    }
}
