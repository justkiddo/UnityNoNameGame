using UnityEngine;



[CreateAssetMenu(menuName = "Tools/BossInfo", fileName = "BossInfo")]
public class BossInfo : ScriptableObject
{
    [field: SerializeField] public float health;
    [field: SerializeField] public int damage;
    [field: SerializeField] public float fireballSpeed;
    [field: SerializeField] public GameObject prefab;
}
