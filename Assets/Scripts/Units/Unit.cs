using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Unit : MonoBehaviour, IAttackVariant
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private PolygonCollider2D attackArea;

    [Space(12)]
    [SerializeField] private float attackCooldown;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float currentHp;
    [SerializeField] private int currentAmmo;

    [SerializeField] private Commands command;

    private float armorCoeficient;
    private Coroutine attackCycle;

    private Unit target;
    private MovementScript movementScript;

    public event Action NotifyUntargeting;
    public event Action<Unit> NotifyDeath;
    public event Action NotifyHPChange;

    private void Awake()
    {
        movementScript = GetComponent<MovementScript>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
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

        RecalculateThreat();
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

    protected virtual void RecalculateThreat()
    {
        InfantryThreatLevel = info.Damage * info.InfantryDamageModifire * currentHp;
        CavalryThreatLevel = info.Damage * info.CavalryDamageModifire * currentHp;
    }

    public void RecieveDamage(float damage)
    {
        currentHp -= damage * armorCoeficient;
        NotifyHPChange?.Invoke();
        RecalculateThreat();
        if (currentHp <= 0)
        {
            NotifyUntargeting?.Invoke();
            NotifyDeath?.Invoke(this);
            StopAllCoroutines();

            gameObject.SetActive(false);
            Destroy(gameObject, 5);
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
        Transform emblemsParent = transform.GetChild(0);
        
        if (count > emblemsParent.childCount)
        {
            return;
        }

        for (int i = 0; i < emblemsParent.childCount; i++)
        {
            GameObject emblem = emblemsParent.GetChild(i).gameObject;
            if (i < count)
            {
                emblem.SetActive(true);
                continue;
            }
            emblem.SetActive(false);
        }

    }
    public void Move(Vector2 position)
    {
        movementScript.MovementDirection = position;
    }

    public void MoveToTarget()
    {
        if (Target != null)
        {
            Vector2 direction = (transform.position - Target.transform.position).normalized;
            movementScript.MovementDirection = (Vector2)Target.transform.position + direction * (Target.transform.lossyScale.y * 0.75f);
        }
        
    }

    public abstract void GetAttacked(IAttackVariant from);

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

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActiveAndEnabled) return;
        if (collision.TryGetComponent(out Unit unit))
        {
            if (unit.Command == command) return;
            if (Target == null) Target = unit;
            if (ReferenceEquals(Target, unit))
            {
                if (attackCycle != null)
                {
                    StopCoroutine(attackCycle);
                    attackCycle = null;
                }
                attackCycle = StartCoroutine(ManageAttack());
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Unit unit))
        {
            if (ReferenceEquals(Target, unit))
            {
                if (attackCycle != null)
                {
                    StopCoroutine(attackCycle);
                    attackCycle = null;
                }
            }
        }
        
    }

    private IEnumerator ManageAttack()
    {
        while (true)
        {
            if (IsRanger())
            {
                if (currentAmmo <= 0) continue;
                DecreaseAmmo();
            }
            if (Target == null)
            {
                yield break;
            }
            Target.GetAttacked(this);
            yield return new WaitForSecondsRealtime(attackCooldown);
        }
    }

    private void StopAttack()
    {
        Target = null;
    }
    
    public UnitInfo Info => info;
    protected int CurrentAmmo => currentAmmo;
    public bool DoesIgnoreCharge => info.DoesIgnoreCharge;
    public Unit Target 
    {
        get => target;
        set
        {
            if (ReferenceEquals(value, target)) return;

            if (attackCycle != null)
            {
                StopCoroutine(attackCycle);
                attackCycle = null;
            }

            if (target != null)
            {
                target.NotifyUntargeting -= StopAttack;
                target = value;

                if (value != null)
                {
                    target.NotifyUntargeting += StopAttack;
                }
            }
            else if (value != null)
            {
                target = value;
                target.NotifyUntargeting += StopAttack;
            }
        }
    }

    public float CurrentHP
    {
        get => currentHp;
        private set => currentHp = value;
    }

    public float InfantryThreatLevel { get; protected set; }
    public float CavalryThreatLevel { get; protected set; }
    public Commands Command => command;
}
