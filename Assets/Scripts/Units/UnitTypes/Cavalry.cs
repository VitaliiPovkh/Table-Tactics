using UnityEngine;

public class Cavalry : Unit
{
    [SerializeField] private float chargeCooldown;
    [Range(1f, 5f)]
    [SerializeField] private float chargeModifire = 2;

    public override void GetAttacked(IAttackVariational from)
    {
        from.Attack(this);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Enemy enemy))
        {
            Unit unit = enemy.GetComponent<Unit>();
            Charge(unit);
            base.OnCollisionEnter2D(collision);
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
