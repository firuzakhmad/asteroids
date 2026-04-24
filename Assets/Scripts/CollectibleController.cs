using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField]
    private int m_pickupScore;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ShipController ship))
        {
            GameEvents.Instance.AddToScore(m_pickupScore);
            Destroy(gameObject);
        }    
    }
}
