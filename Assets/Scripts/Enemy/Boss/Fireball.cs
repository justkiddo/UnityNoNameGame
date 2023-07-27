using System.Collections;
using UnityEngine;
using Zenject;

namespace root
{
    public class Fireball : MonoBehaviour
    {
        public bool hit;

        private void Awake()
        {
            hit = false;
            StartCoroutine(DestroyFireball());
        }

    
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                hit = true;
            }
        }


        private IEnumerator DestroyFireball()
        {
            yield return new WaitForSeconds(3);
            Destroy(this.gameObject);
        }
    }
}