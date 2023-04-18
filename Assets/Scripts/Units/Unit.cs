using System.Collections;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IAttackVariant
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

    public event Notify NotifyUntargeting;
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
        if (currentHp <= 0)
        {
            NotifyUntargeting?.Invoke();
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
        Transform emblemsParent = transform.GetChild(0);
        
        if (count > emblemsParent.childCount)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            emblemsParent.GetChild(i).gameObject.SetActive(true);
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

    public virtual void OnAttackAreaEnter(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (Target == null) Target = enemy;
            if (ReferenceEquals(Target, enemy))
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

    private IEnumerator ManageAttack()
    {
        while (true)
        {
            if (IsRanger())
            {
                if (currentAmmo <= 0) continue;
                DecreaseAmmo();
            }
            Target.Unit.GetAttacked(this);
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
    public MovementScript MovementScript { get; private set; }
    public Enemy Target 
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
                target.Unit.NotifyUntargeting -= StopAttack;
                target = value;

                if (value != null)
                {
                    target.Unit.NotifyUntargeting += StopAttack;
                }
            }
            else if (value != null)
            {
                target = value;
                target.Unit.NotifyUntargeting += StopAttack;
            }
        }
    }

    public float CurrentHP
    {
        get => currentHp;
        private set => currentHp = value;
    }
}
