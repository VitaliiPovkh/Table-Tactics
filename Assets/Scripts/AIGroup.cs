using FluentBehaviourTree;
using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class AIGroup : UnitGroup
{
    [SerializeField] private AIGroup enemyGroup;

    public override void SetGroup(List<AIUnit> unitsInGroup)
    {
        base.SetGroup(unitsInGroup);
        SetupGroup();

        enemyGroup = new AIGroup();
        enemyGroup.SetGroup(new List<SelectableUnit>());
    }

    public override void SetGroup(List<SelectableUnit> unitsInGroup)
    {
        base.SetGroup(unitsInGroup);
        SetupGroup();
    }

    public void UpdateLogic()
    {
        if (Behaviour != null)
        {
            TimeData time;
            time.deltaTime = Time.deltaTime;
            Behaviour.Update(time);
        }
    }

    public void AddUnit(Unit unit)
    {
        if (!UnitsInGroup.Contains(unit))
        {
            UnitsInGroup.Add(unit);
            SetupUnit(unit);

            Debug.Log($"ADDED! Threat level Infantry: {TotalInfantryThreat}");
            Debug.Log($"ADDED! Threat level Cavalry: {TotalCavalryThreat}");
        }
    }

    public void RemoveUnit(Unit unit)
    {
        if (UnitsInGroup.Contains(unit))
        {
            TotalInfantryThreat -= unit.InfantryThreatLevel;
            TotalCavalryThreat -= unit.CavalryThreatLevel;

            unit.NotifyDeath -= RemoveUnit;

            UnitsInGroup.Remove(unit);

            Debug.Log($"REMOVED! Threat level Infantry: {TotalInfantryThreat}");
            Debug.Log($"REMOVED! Threat level Cavalry: {TotalCavalryThreat}");
        }
    }

    private void SetupGroup()
    {
        foreach (Unit unit in UnitsInGroup)
        {
            SetupUnit(unit);
        }
    }

    private void SetupUnit(Unit unit)
    {
        TotalInfantryThreat += unit.InfantryThreatLevel;
        TotalCavalryThreat += unit.CavalryThreatLevel;

        unit.NotifyDeath += RemoveUnit;
    }

    public Type FirstUnitType
    {
        get => UnitsInGroup.Count > 0 ? UnitsInGroup[0].GetType() : null;
    }


    public float TotalInfantryThreat { get; private set; }
    public float TotalCavalryThreat { get; private set; }
    public AIGroup EnemyGroup => enemyGroup;
    public GroupBehaviour Behaviour { get; set; }
}
