
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIUnit : MonoBehaviour, IGroupInitializer
{
    [SerializeField] private CircleCollider2D scanArea;

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        scanArea.gameObject.SetActive(false);
        //Temp
        scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
    }

    private void OnValidate()
    {
        //Temp
        scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        scanArea.gameObject.SetActive(false);
    }

    public AIGroup InitializeAIGroup()
    {
        AIGroup group = new AIGroup();

        float globalScanRadius = scanArea.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, globalScanRadius);

        List<AIUnit> unitsToGroup = new List<AIUnit>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out AIUnit unit))
            {
                if (ReferenceEquals(unit.Unit.Info, Unit.Info) && !unit.IsInGroup)
                {
                    unit.IsInGroup = true;
                    unitsToGroup.Add(unit);
                }
            }
        }

        group.SetGroup(unitsToGroup);
        return group;
    }

    public Unit Unit { get; private set; }
    public CircleCollider2D ScanArea => scanArea;
    public bool IsInGroup { get; set; }
}
