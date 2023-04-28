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
                    allGroups.Add(leaderAIUnit.InitializeAIGroup());
                }
                else break; 
            }
        }
        foreach (AIGroup group in allGroups)
        {
            if (group.GetFirstUnitType() == typeof(Infantry))
            {
                infantryGroups.Add(group);
            }
            if (group.GetFirstUnitType() == typeof(Cavalry))
            {
                cavalryGroups.Add(group);
            }
        }
    }

    private void Update()
    {
    }

    public AIGroup FindScouts()
    {
        AIGroup scouts = allGroups[0];
        foreach (AIGroup group in allGroups)
        {
            if(scouts.GroupSpeed < group.GroupSpeed && !group.IsBusy)
            {
                scouts = group;
            }
        }
        return !scouts.IsBusy ? scouts : null;
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
