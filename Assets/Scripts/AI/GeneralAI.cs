using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;
using UnityEngine;

public class GeneralAI : MonoBehaviour
{
    [SerializeField] private BehaviourTree generalBehaviour;
    [SerializeField] private List<UnitInfo> allUnitTypes = new List<UnitInfo>();

    private Dictionary<(int, UnitInfo), AIGroup> aiArmy;
    private Dictionary<(int, UnitInfo), AIGroup> playersArmy;

    [SerializeField] private List<AIGroup> ai;
    [SerializeField] private List<AIGroup> player;


    private void Start()
    {
        aiArmy = new Dictionary<(int, UnitInfo), AIGroup>();
        playersArmy = new Dictionary<(int, UnitInfo), AIGroup>();

        LoadArmiesData();

        ai = aiArmy.Values.ToList();
        player = playersArmy.Values.ToList();
    }

    private void LoadArmiesData()
    {
        List<SelectableUnit> allPlayersUnits = new List<SelectableUnit>(FindObjectsOfType<SelectableUnit>());
        List<AIUnit> allAIUnits = new List<AIUnit>(FindObjectsOfType<AIUnit>());

        Shuffle(allPlayersUnits);
        Shuffle(allAIUnits);

        foreach (UnitInfo unitType in allUnitTypes)
        {
            int groupID = 0;
            while (true)
            {
                AIUnit leaderAIUnit = allAIUnits.Find(u => ReferenceEquals(u.Unit.Info, unitType) && !u.IsInGroup);
                if (leaderAIUnit != null)
                {
                    aiArmy.Add((groupID++, unitType), leaderAIUnit.InitializeAIGroup());
                }
                else break; 
            }

            groupID = 0;
            while (true)
            {
                SelectableUnit leaderPlayersUnit = allPlayersUnits.Find(u => ReferenceEquals(u.Unit.Info, unitType) && !u.IsInGroup);
                if (leaderPlayersUnit != null)
                {
                    playersArmy.Add((groupID++, unitType), leaderPlayersUnit.InitializeAIGroup());
                }
                else break;
            }
        }
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
