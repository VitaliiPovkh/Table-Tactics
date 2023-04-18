using UnityEngine;

public class Cavalry : Unit
{
    [SerializeField] private float chargeCooldown;
    [Range(1f, 5f)]
    [SerializeField] private float chargeModifire = 2;

    public override void GetAttacked(IAttackVariant from) => from.Attack(this);

    public override void OnAttackAreaEnter(Collider2D other)
    {
        base.OnAttackAreaEnter(other);
        if (Target != null)
        {
            Charge(Target.Unit);
        }
    }

    private void Charge(Unit enemy)
    {
        if (enemy == null) return;

        if (!enemy.DoesIgnoreCharge)
        {
            enemy.RecieveDamage(Info.Damage * chargeModifire - Info.Damage);
        }
        enemy.GetAttacked(this);
    }
}
