using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UnitGroup
{
    [SerializeField] private List<Unit> unitsInGroup = new List<Unit>();

    public virtual void SetGroup(List<AIUnit> unitsInGroup)
    {
        if (this.unitsInGroup.Count > 0)
        {
            Unsubscribe();
            this.unitsInGroup.Clear();
        }
        foreach (AIUnit aiUnit in unitsInGroup)
        {
            this.unitsInGroup.Add(aiUnit.Unit);
            aiUnit.Unit.NotifyDeath += RemoveUnit;
        }
    }

    public virtual void SetGroup(List<SelectableUnit> unitsInGroup)
    {
        if (this.unitsInGroup.Count > 0)
        {
            Unsubscribe();
            this.unitsInGroup.Clear();
        }
        foreach (SelectableUnit selectable in unitsInGroup)
        {
            this.unitsInGroup.Add(selectable.Unit);
            selectable.Unit.NotifyDeath += RemoveUnit;
        }
    }

    private void Unsubscribe()
    {
        foreach (Unit unit in unitsInGroup)
        {
            if (unit != null)
            {
                unit.NotifyDeath -= RemoveUnit;
            }
        }
    }

    public void MoveGroup(Vector2 position, float radius)
    {
        if (unitsInGroup.Count == 0) return;
        
        float step = (Mathf.Deg2Rad * 360) / unitsInGroup.Count;

        unitsInGroup[0].Target = null;
        unitsInGroup[0].Move(position);
        for (int i = 1; i < unitsInGroup.Count; i++)
        {
            unitsInGroup[i].Target = null;
            Vector2 posOnCircle = new Vector2(Mathf.Sin(i * step), Mathf.Cos(i * step)) * radius;
            unitsInGroup[i].Move(posOnCircle + position);
        }
    }

    public void Attack(AIUnit enemy)
    {
        SetTarget(enemy.Unit);
    }

    public void Attack(SelectableUnit enemy)
    {
        SetTarget(enemy.Unit);
    }

    private void SetTarget(Unit unit)
    {
        if (unitsInGroup.Count == 0) return;

        GroupTarget = unit;
        for (int i = 0; i < unitsInGroup.Count; i++)
        {
            unitsInGroup[i].Target = unit;
            unitsInGroup[i].MoveToTarget();
        }
    }

    public void RemoveUnit(Unit unit)
    {
        unitsInGroup.Remove(unit);
    }


    protected List<Unit> UnitsInGroup => unitsInGroup;
    public Vector2 Position => unitsInGroup[0].transform.position;
    public Unit GroupTarget { get; private set; }
}
