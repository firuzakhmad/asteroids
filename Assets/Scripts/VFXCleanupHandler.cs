using Unity.VisualScripting;
using UnityEngine;

public class VFXCleanupHandler : MonoBehaviour
{
    private float m_currentTime;

    [SerializeField]
    private float m_lifeTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentTime >= m_lifeTime)
        {
            GameObject.Destroy(gameObject);
        }

        m_currentTime += Time.deltaTime;
    }
}
