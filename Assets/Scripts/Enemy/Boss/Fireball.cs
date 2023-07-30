using UnityEngine;

namespace root
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private GameObject hitParticle;
        
        private void OnCollisionEnter2D(Collision2D col)
        { 
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}