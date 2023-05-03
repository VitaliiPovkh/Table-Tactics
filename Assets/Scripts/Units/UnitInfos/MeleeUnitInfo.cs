using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Units/Melee Unit")]
public class MeleeUnitInfo : UnitInfo 
{
    [Range(1f, 5f)]
    [SerializeField] private float chargeModifire = 2;

    public float ChargeModifire => chargeModifire;
}
