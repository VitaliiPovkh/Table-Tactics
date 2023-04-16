
using UnityEngine;

public class Infantry : Unit
{
    public override void GetAttacked(IAttackVariational from) => from.Attack(this);
}
