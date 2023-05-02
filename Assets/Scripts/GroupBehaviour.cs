using FluentBehaviourTree;
using System.Collections;
using UnityEngine;

public class GroupBehaviour
{
    private BehaviourTreeBuilder builder;
    private IBehaviourTreeNode tree;

    private AIGroup group;
    private GeneralAI generalAI;

    private SelectableUnit enemy;


    private float lastUpdateTime = 0f;
    private float pathCalcCooldown = 3f;


    public GroupBehaviour(AIGroup group, GeneralAI generalAI)
    {
        this.group = group;
        this.generalAI = generalAI;

        builder = new BehaviourTreeBuilder();
        InitTree();
    }

    private void InitTree()
    {
        tree = builder
            .Selector("global-selector")
                .Sequence("init-enemy")
                    .Condition("search-enemy", t => group.GroupTarget == null)
                        .Do("find-enemy", FindEnemy)
                    .Do("set-target", SetTarget)
                .End()
                .Do("attack-action", AttackEnemy)
            .End().Build();
    }

    private BehaviourTreeStatus SetTarget(TimeData arg)
    {
        group.Attack(enemy);
        return BehaviourTreeStatus.Success;
    }

    public void Update(TimeData time)
    {
        tree.Tick(time);
        Debug.Log(group.GroupTarget == null);
    }

    private BehaviourTreeStatus AttackEnemy(TimeData arg)
    {
        if (group.GroupTarget == null)
        {
            return BehaviourTreeStatus.Success;
        }
        if (Time.time > lastUpdateTime)
        {
            group.Attack(enemy);
            lastUpdateTime = Time.time + pathCalcCooldown;
        }
        return BehaviourTreeStatus.Running;
    }

    private BehaviourTreeStatus FindEnemy(TimeData arg)
    {
        enemy = generalAI.GetRandomEnemy();
        return enemy != null ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
    }

}
