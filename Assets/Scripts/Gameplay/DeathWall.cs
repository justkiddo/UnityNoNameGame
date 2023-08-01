using root;
using UnityEngine;

public class DeathWall : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") )
        {
            _player.TakeDamage(10000);
        }
    }
}
