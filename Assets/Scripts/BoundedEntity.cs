using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoundedEntity : MonoBehaviour
{
    protected SpriteRenderer m_spriteRenderer;
    protected Collider2D m_colloder;

    protected Rigidbody2D m_rigidbody;
    [SerializeField]
    protected Rect m_bounds;

    [SerializeField]
    protected int m_health;

    [SerializeField]
    protected int m_maxHealth = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_colloder = gameObject.GetComponent<Collider2D>();
    }

    protected virtual void LateUpdate()
    {
        if (!m_bounds.Contains(transform.position))
        {
            Vector2 position = transform.position;

            if (position.x < m_bounds.xMin)
            {
                position.x = m_bounds.xMax;
            }
            if (position.x > m_bounds.xMax)
            {
                position.x = m_bounds.xMin;
            }
            if (position.y < m_bounds.yMin)
            {
                position.y = m_bounds.yMax;
            }
            if (position.y > m_bounds.yMax)
            {
                position.y = m_bounds.yMin;
            }

            m_rigidbody.position = position;
        }
    }

    protected virtual void OnEnabled()
    {
        ResetHealth();
    }

    protected virtual void OnDisabled()
    {
    }

    protected void LoseHealth()
    {
        --m_health;
 
        if (m_health <= 0)
        {
            OnDie();
        }
    }

    protected void ResetHealth()
    {
        m_health = m_maxHealth;
    }

    protected virtual void OnDie()
    {
        
    }
}
