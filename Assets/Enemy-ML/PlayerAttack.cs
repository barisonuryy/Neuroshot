using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public List<AttackSO> combo;

    private float lastClickedTime;

    private float lastComboEnd;

    private int comboCounter;
    
    [SerializeField] private Animator _animator;

    [SerializeField] private Weapon _weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        ExitAttack();
    }

    void Attack()
    {
        if (Time.time - lastComboEnd > 0.2f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");
            if (Time.time - lastClickedTime >= 0.2f)
            {
                _animator.runtimeAnimatorController = combo[comboCounter].AnimatorOverrideController;
                _animator.Play("Attack",0,0);
                _weapon.damage = combo[comboCounter].damage;
                comboCounter++;
                lastClickedTime = Time.time;
                
                if (comboCounter >= combo.Count)
                {
                    comboCounter = 0;
                }
            }
        }
     
    }

    void ExitAttack()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
            _animator.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
        {
            Invoke("EndCombo",1);
        } 
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        
    }
}
