using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Visitor pattern
public interface IAttackVariational
{
    void Attack(Infantry enemy);
    void Attack(Cavalry enemy);
    void Attack(SiegeUnit enemy);

    //void Attack(Building enemy);
}
