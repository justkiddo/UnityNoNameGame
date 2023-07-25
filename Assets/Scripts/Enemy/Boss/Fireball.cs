using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(DestroyFireball());
    }


    private IEnumerator DestroyFireball()
    {
        yield return new WaitForSeconds(6);
        Destroy(this.gameObject);
    }
}
