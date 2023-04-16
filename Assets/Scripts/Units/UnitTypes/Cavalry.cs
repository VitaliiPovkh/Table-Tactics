using UnityEngine;

public class Cavalry : Unit
{
    [SerializeField] private float chargeCooldown;
    [Range(1f, 5f)]
    [SerializeField] private float chargeModifire = 2;

    public override void GetAttacked(IAttackVariational from) => from.Attack(this);

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            Unit unit = enemy.GetComponent<Unit>();
            Charge(unit);
            base.OnTriggerEnter2D(other);
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
