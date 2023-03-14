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

    //public void MoveGroup(Vector2 position)
    //{
    //    Unit nearestUnit = selectedUnits[0];
    //    foreach (Unit unit in selectedUnits)
    //    {
    //        if (Vector2.Distance(unit.transform.position, position) < Vector2.Distance(nearestUnit.transform.position, position))
    //        {
    //            nearestUnit = unit;
    //        }
    //    }




    //}

    public void MoveGroup(Vector2 position)
    {
        Vector2 totalPosition = Vector2.zero;
        foreach (Unit unit in selectedUnits)
        {
            if (unit != null)
            {
                totalPosition += (Vector2)unit.transform.position;
            }
        }

        Vector2 centredPosition = totalPosition / selectedUnits.Count;
        foreach (Unit unit in selectedUnits)
        {
            if (unit != null)
            {
                Vector2 startPosition = (Vector2)unit.transform.position - centredPosition;
                unit.MovementDirection = position + startPosition;
            }
        }

    }
}
