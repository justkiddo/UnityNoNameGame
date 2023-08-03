using UnityEngine;



[CreateAssetMenu(menuName = "Tools/BossInfo", fileName = "BossInfo")]
public class BossInfo : ScriptableObject
{
    [field: SerializeField] public float Health{ get; private set; }
    [field: SerializeField] public int Damage{ get; private set; }
    [field: SerializeField] public float FireballSpeed{ get; private set; }
}
