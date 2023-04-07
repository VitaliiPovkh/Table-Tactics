using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitGroup
{
    private List<Unit> selectedUnits;


    private const int FORMATION_LENGTH = 5;

    private const float FORMATION_OFFSET = 5f;

    public void SetGroup(List<Unit> selectedUnits)
    {
        this.selectedUnits = new List<Unit>(selectedUnits);
    }

    public void MoveGroup(Vector2 position, float radius)
    {
        float step = (Mathf.Deg2Rad * 360) / selectedUnits.Count;
        selectedUnits[0].MovementDirection = position;
        for (int i = 1; i < selectedUnits.Count; i++)
        {
            Vector2 posOnCircle = new Vector2(Mathf.Sin(i * step), Mathf.Cos(i * step)) * radius;
            selectedUnits[i].MovementDirection = posOnCircle + position;
        }
    }
}
