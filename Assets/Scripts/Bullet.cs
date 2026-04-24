using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_forwardSpeed;
    
    private float m_currentLifeTime;
    
    [SerializeField]
    private float m_maximumLifeTime;

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
