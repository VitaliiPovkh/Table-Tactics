using UnityEngine;

public class Cavalry : Unit
{
    

    public override void GetAttacked(IAttackVariant from) => from.Attack(this);

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (Target != null && other.TryGetComponent(out AIUnit enemy))
        {
            if (ReferenceEquals(Target, enemy))
            {
                Charge(Target);
                base.OnTriggerEnter2D(other);
            } 
        }
    }

    private void Charge(Unit enemy)
    {
        if (!enemy.DoesIgnoreCharge && Info is MeleeUnitInfo)
        {
            enemy.RecieveDamage(Info.Damage * ((MeleeUnitInfo)Info).ChargeModifire - Info.Damage);
        }
    }

    protected override void RecalculateThreat()
    {
        base.RecalculateThreat();
        if (Info is MeleeUnitInfo)
        {
            InfantryThreatLevel *= ((MeleeUnitInfo)Info).ChargeModifire;
        }
        
    }
}
