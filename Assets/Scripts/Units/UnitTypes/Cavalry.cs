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
        Unit enemy = collision.gameObject.GetComponent<Unit>();
        Charge(enemy);
        base.OnCollisionEnter2D(collision);
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
