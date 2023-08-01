using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace root
{
  public class CheckpointSystem : MonoBehaviour
  {
    public List<GameObject> _checkpointsList;
    
    private GameplayInfo _gameplayInfo;
    

    private void Awake()
    {
      _checkpointsList = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
    }
    

  }
}