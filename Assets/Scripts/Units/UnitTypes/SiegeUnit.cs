using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeUnit : Unit
{
    [Range(0f, 100f)]
    [SerializeField] private float hitProbability;

    public override void GetAttacked(IAttackVariational from)
    {
        from.Attack(this);
    }

    public override void Attack(Cavalry enemy)
    {
        if (DidHit())
        {
            base.Attack(enemy);
        }
    }

    public override void Attack(Infantry enemy)
    {
        if (DidHit())
        {
            base.Attack(enemy);
        }
    }

    public override void Attack(SiegeUnit enemy)
    {
        if (DidHit())
        {
            base.Attack(enemy);
        }
    }

    public bool DidHit()
    {
        float hitRoll = Random.Range(0f, 1f);
        return hitRoll <= hitProbability / 100f;
    }
}
