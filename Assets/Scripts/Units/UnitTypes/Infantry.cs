
using UnityEngine;

public class Infantry : Unit
{
    public override void GetAttacked(IAttackVariant from) => from.Attack(this);

    protected override void RecalculateThreat()
    {
        base.RecalculateThreat();
        if (Info.DoesIgnoreCharge)
        {
            CavalryThreatLevel *= 1.5f;
        }
    }
}
