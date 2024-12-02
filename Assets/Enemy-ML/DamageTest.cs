using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    [SerializeField] private AttackSO attack;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerAttack>();
        if (player != null)
        {
            player.combo.Add(attack);
            Destroy(gameObject);
        }
    }
}
