using System;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class AIGroup : UnitGroup
{
    [SerializeField] private BehaviourTree groupBehaviour;

    public override void SetGroup(List<SelectableUnit> unitsInGroup)
    {
        base.SetGroup(unitsInGroup);
        SetupGroup();
    }

    public override void SetGroup(List<AIUnit> unitsInGroup)
    {
        base.SetGroup(unitsInGroup);
        SetupGroup();
    }

    private void SetupGroup()
    {
        foreach (Unit unit in UnitsInGroup)
        {
            TotalInfantryThreat += unit.InfantryThreatLevel;
            TotalCavalryThreat += unit.CavalryThreatLevel;

            unit.NotifyDeath += DecreaseThreat;
            unit.NotifyDeath += DecreaseGroup;
        }
    }

    private void DecreaseGroup(Unit unit)
    {
        UnitsInGroup.Remove(unit);
    }

    private void DecreaseThreat(Unit unit)
    {
        TotalInfantryThreat -= unit.InfantryThreatLevel;
        TotalCavalryThreat -= unit.CavalryThreatLevel;
    }

    public float TotalInfantryThreat { get; private set; }
    public float TotalCavalryThreat { get; private set; }

}
