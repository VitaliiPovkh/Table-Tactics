
using UnityEngine;

public class Infantry : Unit
{
    public override void GetAttacked(IAttackVariant from) => from.Attack(this);
}
