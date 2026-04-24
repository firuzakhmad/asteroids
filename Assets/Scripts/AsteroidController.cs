using System;
using UnityEngine;

public class AsteroidController : BoundedEntity
{
    [SerializeField]
    private int m_size;

    [SerializeField]
    private float m_forcePower;

    [SerializeField]
    private float m_constantPower;

    [SerializeField]
    private float m_angularPower;

    public int Size => m_size;

    public event Action<AsteroidController> onAsteroidDie;
  
    private void Start()
    {
        // Pick a random direction
        Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;

        // Set velocity directly (no AddForce)
        float speed = m_forcePower; // reuse your variable as "speed"
        m_rigidbody.linearVelocity = direction * speed;

        // Add some spin
        m_rigidbody.angularVelocity = UnityEngine.Random.Range(-m_angularPower, m_angularPower);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            LoseHealth();
        }
    }

    protected override void OnDie()
    {
        onAsteroidDie?.Invoke(this);
    }
}
 