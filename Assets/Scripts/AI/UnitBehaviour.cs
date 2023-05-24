using FluentBehaviourTree;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UnitBehaviour
{
    private BehaviourTreeBuilder builder;
    private IBehaviourTreeNode tree;

    private Unit unit;


    private float lastUpdateTime = 0f;
    private float pathCalcCooldown = 3f;


    public UnitBehaviour(Unit unit)
    {
        this.unit = unit;

        builder = new BehaviourTreeBuilder();
        InitTree();
    }

    private void InitTree()
    {
        tree = builder
            .Sequence("")
                .Do("find-enemy", FindEnemy)
                .Do("fight-enemy", ManageFight)
            .End().Build();
    }

    public void Update(TimeData time)
    {
        tree?.Tick(time);
    }

    private BehaviourTreeStatus FindEnemy(TimeData arg)
    {
        if (Time.time < lastUpdateTime) return BehaviourTreeStatus.Failure;

        Unit potentialEnemy = null;
        float minDistance = float.MaxValue;
        foreach (SelectableUnit playerUnit in MapInfo.PlayerUnits)
        {
            if (playerUnit == null)
            {
                continue;
            }

            float unitThreat = 0;
            float enemyThreat = 0;

            if (unit is Infantry)
            {
                enemyThreat = playerUnit.Unit.InfantryThreatLevel;
            }
            if (unit is Cavalry)
            {
                enemyThreat = playerUnit.Unit.CavalryThreatLevel;
            }

            if (playerUnit.Unit is Infantry)
            {
                unitThreat = unit.InfantryThreatLevel;
            }
            if (playerUnit.Unit is Cavalry)
            {
                unitThreat = unit.CavalryThreatLevel;
            }

            float distance = Vector2.Distance(unit.transform.position, playerUnit.transform.position);

            if (unitThreat >= enemyThreat && distance < minDistance)
            {
                minDistance = distance;
                potentialEnemy = playerUnit.Unit;
            }
        }

        if (potentialEnemy == null)
        {
            return BehaviourTreeStatus.Failure;
        }

        unit.Target = potentialEnemy;
        return BehaviourTreeStatus.Success;
    }


    private BehaviourTreeStatus ManageFight(TimeData arg)
    {
        if (unit.Target == null)
        {
            return BehaviourTreeStatus.Success;
        }

        unit.MoveToTarget();
        lastUpdateTime = Time.time + pathCalcCooldown;
        return BehaviourTreeStatus.Running;
    }

    public MapInfo MapInfo { get; set; }
    
}
