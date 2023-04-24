using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class SelectableUnit : MonoBehaviour, IGroupInitializer
{
    [SerializeField] private CircleCollider2D scanArea;

    private Color unitColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Unit = GetComponent<Unit>();
        //Temp
        scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        unitColor = spriteRenderer.color;
        scanArea.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        //Temp
        scanArea = transform.GetChild(transform.childCount - 1).GetComponent<CircleCollider2D>();

        scanArea.gameObject.SetActive(false); 
    }

    public void Select()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    public void Deselect()
    {
        spriteRenderer.color = unitColor;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (Unit != null)
        {
            Unit.OnAttackAreaEnter(other);
        }   
    }


    public AIGroup InitializeAIGroup()
    {
        AIGroup group = new AIGroup();

        float globalScanRadius = scanArea.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, globalScanRadius);

        List<SelectableUnit> unitsToGroup = new List<SelectableUnit>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out SelectableUnit unit))
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


    public CircleCollider2D ScanArea => scanArea;
    public bool IsInGroup { get; set; }
    public Unit Unit { get; private set; }
}
