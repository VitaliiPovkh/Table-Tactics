using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitGroup
{
    private List<Unit> selectedUnits;

    public void SetGroup(List<Selectable> selectedUnits)
    {
        this.selectedUnits = new List<Unit>();
        foreach (var selectable in selectedUnits)
        {
            this.selectedUnits.Add(selectable.Unit);
        }
    }

    public void MoveGroup(Vector2 position, float radius)
    {
        float step = (Mathf.Deg2Rad * 360) / selectedUnits.Count;

        selectedUnits[0].MovementScript.MovementDirection = position;
        for (int i = 1; i < selectedUnits.Count; i++)
        {
            Vector2 posOnCircle = new Vector2(Mathf.Sin(i * step), Mathf.Cos(i * step)) * radius;
            selectedUnits[i].MovementScript.MovementDirection = posOnCircle + position;
        }
    }

    public void Attack(Enemy enemy)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Target = enemy;
        }

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            Vector2 direction = (selectedUnits[i].transform.position - enemy.transform.position).normalized;
            selectedUnits[i].MovementScript.MovementDirection = (Vector2)enemy.transform.position + direction * (enemy.transform.lossyScale.y * 0.75f); 
        }
    }


}
