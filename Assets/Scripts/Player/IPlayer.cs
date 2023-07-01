using UnityEngine;

namespace root
{
    public interface IPlayer
    {
        Vector3 GetCurrentPosition();
        void TakeDamage(float damage);
        float GetHealth();
    }
}