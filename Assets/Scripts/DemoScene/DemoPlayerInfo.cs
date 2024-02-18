using UnityEngine;

namespace root
{
    [CreateAssetMenu(menuName = "Tools/DemoPlayerInfo", fileName = "DemoPlayerInfo")]
    public class DemoPlayerInfo: ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float JumpHeight { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }

    }
}