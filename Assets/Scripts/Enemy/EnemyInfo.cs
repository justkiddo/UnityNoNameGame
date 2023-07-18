using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tools/EnemyInfo", fileName = "EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public float FollowDistance { get; private set; }
    
}
