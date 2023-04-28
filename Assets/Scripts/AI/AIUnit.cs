using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIUnit : MonoBehaviour
{
    [SerializeField] private AIGroup group = null;
    
    [SerializeField] private UnitScanner unitScanner;


    private void Awake()
    {
        Unit = GetComponent<Unit>();
        //Temp
        unitScanner = transform.GetChild(transform.childCount - 1).GetComponent<UnitScanner>();
        ScanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        ScanArea.gameObject.SetActive(true);
    }

    private void Start()
    {
        unitScanner.NotifyIntruderEnter += AddUnitToPlayersGroup;
        unitScanner.NotifyIntruderLeave += RemoveUnitFromPlayersGroup;
    }

    private void OnValidate()
    {
        //Temp
        CircleCollider2D scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
        scanArea.gameObject.SetActive(false);
    }

    public AIGroup InitializeAIGroup()
    {
        AIGroup group = new AIGroup();

        float globalScanRadius = ScanArea.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, globalScanRadius);

        List<AIUnit> unitsToGroup = new List<AIUnit>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out AIUnit unit))
            {
                if (ReferenceEquals(unit.Unit.Info, Unit.Info) && !unit.IsInGroup)
                {
                    unit.Group = group;
                    unitsToGroup.Add(unit);
                }
            }
        }

        group.SetGroup(unitsToGroup);
        return group;
    }

    private void AddUnitToPlayersGroup(SelectableUnit unit)
    {
        group.EnemyGroup.AddUnit(unit.Unit);
    }

    private void RemoveUnitFromPlayersGroup(SelectableUnit unit)
    {
        group.EnemyGroup.RemoveUnit(unit.Unit);
    }

    public AIGroup Group 
    {
        get => group;
        set 
        {
            group = value;
            IsInGroup = group != null;
        }
    }

    public bool IsInGroup { get; private set; } = false;
    public Unit Unit { get; private set; }
    public CircleCollider2D ScanArea { get; private set; }
}
