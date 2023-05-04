using FluentBehaviourTree;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIUnit : MonoBehaviour
{
    [SerializeField] private UnitScanner unitScanner;

    private UnitBehaviour groupBehaviour;
    private MapInfo generalAI;

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        //Temp
        unitScanner = transform.GetChild(transform.childCount - 1).GetComponent<UnitScanner>();
        ScanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        ScanArea.gameObject.SetActive(false);

        groupBehaviour = new UnitBehaviour(Unit);
    }


    private void OnValidate()
    {
        //Temp
        CircleCollider2D scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        scanArea.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (groupBehaviour == null) return;
        groupBehaviour.Update(new TimeData(Time.deltaTime));
    }


    public bool IsInGroup { get; private set; } = false;
    public Unit Unit { get; private set; }
    private CircleCollider2D ScanArea { get; set; }
    public MapInfo GeneralAI 
    { 
        get => generalAI;
        set
        {
            generalAI = value;
            groupBehaviour.GeneralAI = value;
        }
    }

    

}
