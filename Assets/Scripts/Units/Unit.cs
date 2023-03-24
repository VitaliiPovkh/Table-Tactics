
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : Selectable, IAttackVariational
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private PolygonCollider2D attackArea;
    
    [Range(1f, 1.7f)]
    [SerializeField] private float squadeScale = 1f;

    [Space(12)] //������� ����� � ��������� �������
    [SerializeField] private float attackCooldown;
    [Space(12)]
    [SerializeField] private MovementScript movementScript;

    private float currentHp;
    private int currentAmmo;

    //for upgrades
    private float unitPriceModifire;
    private float speedModifire;
    private float baseDamageModifire;
    private float attackRangeModifire;
    private float chargeDamageModifire;
    //

    private float armorCoeficient;
    private Coroutine attackCycle;

    //private Unit currentTarget;

    protected override void Start()
    {
        base.Start();
        movementScript.Info = info;
        SpriteRenderer.sprite = info.BaseSprite;

        foreach (Transform emblem in transform.GetChild(1))
        {
            emblem.GetComponent<SpriteRenderer>().sprite = info.Emblem;
        }        

        attackArea.transform.localScale = new Vector2(1, info.AttackRange);
        attackArea.transform.position += (Vector3)Vector2.up * info.AttackRange / 2 * 10;

        switch (info.Heaviness)
        {
            case UnitHeaviness.LIGHT:
                armorCoeficient = 1f;
                break;
            case UnitHeaviness.MEDIUM:
                armorCoeficient = 0.75f;
                break;
            case UnitHeaviness.HEAVY:
                armorCoeficient = 0.5f;
                break;
        }

        SetEmblem();
        
    }

    private void OnValidate()
    {
        SetEmblem();
    }
    public void RecieveDamage(float damage)
    {
        currentHp -= damage * armorCoeficient;
        if (currentHp < 0)
        {
            //...
        }
    }

    protected bool IsRanger()
    {
        return info is RangeUnitInfo;
    }

    protected void DecreaseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    

    private void SetEmblem()
    {
        int currentEmblemCount = (int)((squadeScale - 1) * 10);

        Transform allEmblems = transform.GetChild(1);

        if (currentEmblemCount == 0)
        {
            allEmblems.GetChild(0).gameObject.SetActive(true);
            return;
        }
        for (int i = 1; i < currentEmblemCount; i++)
        {
            if (i % 2 == 0)
            {
                allEmblems.GetChild(i).gameObject.SetActive(true);
                allEmblems.GetChild(i - 1).gameObject.SetActive(true);
            }

        }
        for (int i = allEmblems.childCount - 1; i > currentEmblemCount - 1; i--)
        {
            if (i % 2 == 0)
            {
                allEmblems.GetChild(i).gameObject.SetActive(false);
                allEmblems.GetChild(i - 1).gameObject.SetActive(false);
            }

        }
    }

    public abstract void GetAttacked(IAttackVariational from);

    public virtual void Attack(Infantry enemy)
    {
        enemy.RecieveDamage(info.Damage * info.InfantryDamageModifire);
    }

    public virtual void Attack(Cavalry enemy)
    {
        enemy.RecieveDamage(info.Damage * info.CavalryDamageModifire);
    }

    public virtual void Attack(SiegeUnit enemy)
    {
        enemy.RecieveDamage(info.Damage * info.SiegeDamageModifire);
    }

    // TODO: Implement target and command system
    //       Decompose class to "Movement" and "Attack" monobehs
    //       Create Input script with ALL inputs, 
    //       Divide cam script into "Move camera" methods
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Unit enemy = collision.gameObject.GetComponent<Unit>();
        if (attackCycle == null && enemy != null)
        {
            attackCycle = StartCoroutine(ManageAttack(enemy));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (attackCycle != null)
        {
            StopCoroutine(attackCycle);
            attackCycle = null;
        }
    }

    private IEnumerator ManageAttack(Unit enemy)
    {
        while (true)
        {
            if (IsRanger())
            {
                if (currentAmmo <= 0) break;
                DecreaseAmmo();
            }
            enemy?.GetAttacked(this);
            yield return new WaitForSecondsRealtime(attackCooldown);
        }
    }

    
    protected UnitInfo Info => info;
    protected int CurrentAmmo => currentAmmo;
    public bool DoesIgnoreCharge => info.DoesIgnoreCharge;

    //Change behaviour
    public Vector2 MovementDirection 
    { 
        get => movementScript.MovementDirection;
        set => movementScript.MovementDirection = value; 
    }
}
