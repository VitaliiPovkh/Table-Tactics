using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[RequireComponent(typeof(Unit))]
public class SelectableUnit : MonoBehaviour
{
    private Color unitColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        unitColor = spriteRenderer.color;
    }

    public void Select()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    public void Deselect()
    {
        spriteRenderer.color = unitColor;
    }

    //&&&
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (Unit != null)
        {
            Unit.OnAttackAreaEnter(other);
        }   
    }

    public Unit Unit { get; private set; }
}
