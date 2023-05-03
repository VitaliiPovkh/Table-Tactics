using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void DeathEventSubscribe(Action<Unit> action)
    {
        Unit.NotifyDeath += action;
    }

    public void DeathEventUnsubscribe(Action<Unit> action)
    {
        Unit.NotifyDeath -= action;
    }

    public Unit Unit { get; private set; }
}
