using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace root
{
  public class CheckpointSystem : MonoBehaviour
  {
    public List<GameObject> _checkpointsList;
    
    private GameplayInfo _gameplayInfo;


    [Inject]
    private void Construct(GameplayInfo gameplayInfo)
    {
      _gameplayInfo = gameplayInfo;
    }
    
    
    private void Awake()
    {
      _checkpointsList = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
    }

    private void Update()
    {
      
    }

    public Vector3 GetCheckpoint()
    {
      return _checkpointsList[_gameplayInfo.SavedCheckpoint.Value].transform.position;
    }

    

    public void SetCheckpoint(GameObject player, int index)
    {
      player.transform.position = _checkpointsList[index].transform.position;
      _gameplayInfo.CurrentCheckpoint.Value = index;
    }

  }
}