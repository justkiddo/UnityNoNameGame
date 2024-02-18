using UnityEngine;


[CreateAssetMenu(menuName = "Tools/DemoEnemyInfo", fileName = "DemoEnemyInfo")]
public class DemoEnemyInfo : ScriptableObject
{
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float FollowDistance { get; private set; }
    
}
