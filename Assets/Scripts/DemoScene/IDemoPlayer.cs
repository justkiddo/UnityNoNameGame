using UnityEngine;

namespace root
{
    public interface IDemoPlayer
    {
        Vector3 GetCurrentPosition();
        void TakeDamage(float damage);
        float GetHealth();
    }
}