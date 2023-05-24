using FluentBehaviourTree;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIUnit : MonoBehaviour
{
    [SerializeField] private UnitScanner unitScanner;

    private UnitBehaviour unitBehaviour;
    private MapInfo mapInfo;

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        //Temp
        unitScanner = transform.GetChild(transform.childCount - 1).GetComponent<UnitScanner>();
        ScanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        ScanArea.gameObject.SetActive(false);

        unitBehaviour = new UnitBehaviour(Unit);
    }


    private void OnValidate()
    {
        //Temp
        CircleCollider2D scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        scanArea.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (unitBehaviour == null) return;
        unitBehaviour.Update(new TimeData(Time.deltaTime));
    }

    public Unit Unit { get; private set; }
    private CircleCollider2D ScanArea { get; set; }
    public MapInfo MapInfo 
    { 
        get => mapInfo;
        set
        {
            mapInfo = value;
            unitBehaviour.MapInfo = value;
        }
    }

    

}
