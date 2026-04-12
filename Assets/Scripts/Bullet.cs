using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_forwardSpeed;
    
    private float m_currentLifeTime;
    
    [SerializeField]
    private float m_maximumLifeTime;

    // private Rigidbody2D rb;

    // void Awake()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // void Start()
    // {
    //     rb.linearVelocity = transform.up * m_forwardSpeed;
    // }


    void Update()
    {
        transform.position += (transform.up * m_forwardSpeed) * Time.deltaTime;  

        m_currentLifeTime += Time.deltaTime;

        if (m_currentLifeTime > m_maximumLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
