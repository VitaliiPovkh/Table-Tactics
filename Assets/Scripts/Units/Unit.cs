
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IAttackVariational
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private PolygonCollider2D attackArea;

    [Space(12)]
    [SerializeField] private float attackCooldown;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float currentHp;
    [SerializeField] private int currentAmmo;

    private float armorCoeficient;
    private Coroutine attackCycle;

    private Enemy target;

    public delegate void Notify();

    public event Notify NotifyDeath;
    public event Notify NotifyHPChange;
    

    protected void Start()
    {
        MovementScript = GetComponent<MovementScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = info.BaseSprite;

        foreach (Transform emblem in transform.GetChild(0))
        {
            emblem.GetComponent<SpriteRenderer>().sprite = info.Emblem;
        }        

        attackArea.transform.parent.localScale = new Vector2(1, info.AttackRange);

        currentHp = info.HP;
        attackCooldown = info.AttackCooldown;

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
        NotifyHPChange?.Invoke();
        if (currentHp < 0)
        {
            NotifyDeath?.Invoke();
            Destroy(gameObject);
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

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        if (Target == null && other.TryGetComponent(out Enemy enemy))
        {
            Target = enemy;
        }
        if (Target != null && attackCycle == null)
        {
            attackCycle = StartCoroutine(ManageAttack(Target.GetComponent<Unit>()));
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

    private void ResetTarget()
    {
        Target = null;
        StopCoroutine(attackCycle);
        attackCycle = null;
    }
    
    public UnitInfo Info => info;
    protected int CurrentAmmo => currentAmmo;
    public bool DoesIgnoreCharge => info.DoesIgnoreCharge;
    public MovementScript MovementScript { get; private set; }
    public Enemy Target 
    {
        get => target;
        set
        {
            if (value != null && target == null)   
            {
                target = value;
                Unit targetInfo = target.GetComponent<Unit>();
                targetInfo.NotifyDeath += ResetTarget;
            }
            if (value == null && target != null)
            {
                Unit targetInfo = target.GetComponent<Unit>();
                targetInfo.NotifyDeath -= ResetTarget;
            }
        }
    }

    public float CurrentHP
    {
        get => currentHp;
        private set => currentHp = value;
    }
}
