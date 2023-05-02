using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GeneralAI : MonoBehaviour
{
    [SerializeField] private List<UnitInfo> allUnitTypes = new List<UnitInfo>();
    //TO DELETE
    [SerializeField] private List<SelectableUnit> allPlayersUnits;


    [SerializeField] private List<AIGroup> allGroups = new List<AIGroup>();

    [SerializeField] private List<AIGroup> cavalryGroups;
    [SerializeField] private List<AIGroup> infantryGroups;

    private void Start()
    {
        
        LoadArmiesData();
        allPlayersUnits = new List<SelectableUnit>(FindObjectsOfType<SelectableUnit>());
    }

    private void LoadArmiesData()
    {
        
        List<AIUnit> allAIUnits = new List<AIUnit>(FindObjectsOfType<AIUnit>());

        Shuffle(allAIUnits);

        foreach (UnitInfo unitType in allUnitTypes)
        {
            while (true)
            {
                AIUnit leaderAIUnit = allAIUnits.Find(u => ReferenceEquals(u.Unit.Info, unitType) && !u.IsInGroup);
                if (leaderAIUnit != null)
                {
                    AIGroup group = leaderAIUnit.InitializeAIGroup();
                    group.Behaviour = new GroupBehaviour(group, this);
                    allGroups.Add(group);
                }
                else break; 
            }
        }
        foreach (AIGroup group in allGroups)
        {
            if (group.FirstUnitType == typeof(Infantry))
            {
                infantryGroups.Add(group);
            }
            if (group.FirstUnitType == typeof(Cavalry))
            {
                cavalryGroups.Add(group);
            }
        }
    }

    private void Update()
    {
        allGroups.ForEach(g =>
        {
            g.UpdateLogic();
        });
    }

    public Vector2 GetEnemyRandomPosition()
    {
        if (allPlayersUnits.Count == 0)
        {
            return Vector2.zero;
        }
        return allPlayersUnits[Random.Range(0, allPlayersUnits.Count)].transform.position;
    }

    public SelectableUnit GetRandomEnemy()
    {
        if (allPlayersUnits.Count == 0)
        {
            return null;
        }
        return allPlayersUnits[Random.Range(0, allPlayersUnits.Count)];
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
