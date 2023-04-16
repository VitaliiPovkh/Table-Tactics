
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IAttackVariational
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private PolygonCollider2D attackArea;
    
    [Range(1f, 1.7f)]
    [SerializeField] private float squadeScale = 1f;

    [Space(12)] //Вынести боёвку в отдельный монобех
    [SerializeField] private float attackCooldown;

    private SpriteRenderer spriteRenderer;

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

    

    protected void Start()
    {
        MovementScript = GetComponent<MovementScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = info.BaseSprite;

        foreach (Transform emblem in transform.GetChild(1))
        {
            emblem.GetComponent<SpriteRenderer>().sprite = info.Emblem;
        }        

        attackArea.transform.parent.localScale = new Vector2(1, info.AttackRange);

        

        switch (info.Heaviness)
        {
            case UnitHeaviness.LIGHT:
                armorCoeficient = 1f;
                SetEmblem(1);
                break;
            case UnitHeaviness.MEDIUM:
                armorCoeficient = 0.75f;
                SetEmblem(3);
                break;
            case UnitHeaviness.HEAVY:
                armorCoeficient = 0.5f;
                SetEmblem(5);
                break;
        }
    }

    private void OnValidate()
    {
        switch (info.Heaviness)
        {
            case UnitHeaviness.LIGHT:
                SetEmblem(1);
                break;
            case UnitHeaviness.MEDIUM:
                SetEmblem(3);
                break;
            case UnitHeaviness.HEAVY:
                SetEmblem(5);
                break;
        }
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

    

    private void SetEmblem(int count)
    {
        Transform emblemsParent = transform.GetChild(1);
        
        if (count >= emblemsParent.childCount)
        {
            return;
        }

        GameObject[] emblems = new GameObject[emblemsParent.childCount];

        for (int i = 0; i < emblems.Length; i++)
        {
            emblems[i] = emblemsParent.GetChild(i).gameObject;
        }

        for (int i = 0; i < count; i++)
        {
            emblems[i].SetActive(true);
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Unit enemy = collision.gameObject.GetComponent<Unit>();
        //if (attackCycle == null && enemy != null)
        //{
        //    attackCycle = StartCoroutine(ManageAttack(enemy));
        //}

        if (collision.collider.TryGetComponent(out Enemy enemy))
        {
            Target = enemy;
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
            enemy.GetAttacked(this);
            yield return new WaitForSecondsRealtime(attackCooldown);
        }
    }

    
    public UnitInfo Info => info;
    protected int CurrentAmmo => currentAmmo;
    public bool DoesIgnoreCharge => info.DoesIgnoreCharge;
    public MovementScript MovementScript { get; private set; }
    public Enemy Target { get; set; }
}
