using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Normal Attack")]
public class AttackSO : ScriptableObject
{
    // Start is called before the first frame update
    public AnimatorOverrideController AnimatorOverrideController;
    public float damage;
    
}
