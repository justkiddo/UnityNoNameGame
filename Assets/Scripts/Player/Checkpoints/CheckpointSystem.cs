using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace root
{
  public class CheckpointSystem : MonoBehaviour
  {
    private List<GameObject> _checkpointsList;

    private void Awake()
    {
      _checkpointsList = GameObject.FindGameObjectsWithTag("Checkpoint").ToList();
    }


    public void SetCheckpoint(GameObject player, int index)
    {
      player.transform.position = _checkpointsList[index].transform.position;
    }

  }
}