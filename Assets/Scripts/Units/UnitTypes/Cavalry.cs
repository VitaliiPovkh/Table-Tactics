using UnityEngine;

public class Cavalry : Unit
{
    [Range(1f, 5f)]
    [SerializeField] private float chargeModifire = 2;

    public override void GetAttacked(IAttackVariant from) => from.Attack(this);

    public override void OnAttackAreaEnter(Collider2D other)
    {
        if (Target != null && other.TryGetComponent(out AIUnit enemy))
        {
            if (ReferenceEquals(Target, enemy))
            {
                Charge(Target.Unit);
                base.OnAttackAreaEnter(other);
            } 
        }
    }

    private void Charge(Unit enemy)
    {
        if (!enemy.DoesIgnoreCharge)
        {
            enemy.RecieveDamage(Info.Damage * chargeModifire - Info.Damage);
        }
    }

    protected override void RecalculateThreat()
    {
        base.RecalculateThreat();
        InfantryThreatLevel *= chargeModifire;
    }
}
