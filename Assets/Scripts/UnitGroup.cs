using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitGroup
{
    [SerializeField] private List<Unit> unitsInGroup;

    public virtual void SetGroup(List<AIUnit> unitsInGroup)
    {
        this.unitsInGroup = new List<Unit>();
        foreach (var selectable in unitsInGroup)
        {
            this.unitsInGroup.Add(selectable.Unit);
        }
    }

    public virtual void SetGroup(List<SelectableUnit> unitsInGroup)
    {
        this.unitsInGroup = new List<Unit>();
        foreach (var selectable in unitsInGroup)
        {
            this.unitsInGroup.Add(selectable.Unit);
        }
    }

    public void MoveGroup(Vector2 position, float radius)
    {
        float step = (Mathf.Deg2Rad * 360) / unitsInGroup.Count;

        unitsInGroup[0].Target = null;
        unitsInGroup[0].MovementScript.MovementDirection = position;
        for (int i = 1; i < unitsInGroup.Count; i++)
        {
            unitsInGroup[i].Target = null;
            Vector2 posOnCircle = new Vector2(Mathf.Sin(i * step), Mathf.Cos(i * step)) * radius;
            unitsInGroup[i].MovementScript.MovementDirection = posOnCircle + position;
        }
    }

    public void Attack(AIUnit enemy)
    {
        for (int i = 0; i < unitsInGroup.Count; i++)
        {
            unitsInGroup[i].Target = enemy;
            Vector2 direction = (unitsInGroup[i].transform.position - enemy.transform.position).normalized;
            unitsInGroup[i].MovementScript.MovementDirection = (Vector2)enemy.transform.position + direction * (enemy.transform.lossyScale.y * 0.75f); 
        }
    }

    protected List<Unit> UnitsInGroup => unitsInGroup;
    public Vector2 Position => unitsInGroup[0].transform.position;
    public bool HasArrived => unitsInGroup[0].MovementScript.IsPathFinished;

}
