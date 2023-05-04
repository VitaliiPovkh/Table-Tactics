using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MapInfo : MonoBehaviour
{
    [SerializeField] private UIController uiController;

    [SerializeField] private List<SelectableUnit> allPlayersUnits;
    [SerializeField] private List<AIUnit> allAIUnits;

    public IEnumerable<SelectableUnit> PlayerUnits => allPlayersUnits.AsEnumerable();

    private void Start()
    {
        allPlayersUnits = new List<SelectableUnit>(FindObjectsOfType<SelectableUnit>());
        allAIUnits = new List<AIUnit>(FindObjectsOfType<AIUnit>());

        allAIUnits.ForEach(u =>
        {
            u.GeneralAI = this;
            u.Unit.NotifyDeath += RemoveFromAi;
        });

        allPlayersUnits.ForEach(u => 
        {
            u.Unit.NotifyDeath += RemoveFromPlayer;
        });
    }

    private void RemoveFromAi(Unit unit)
    {
        AIUnit aiUnit = allAIUnits.Find(s => ReferenceEquals(unit, s.Unit));
        allAIUnits.Remove(aiUnit);
        if (allAIUnits.Count == 0)
        {
            uiController.ShowVictory();
        }
    }

    private void RemoveFromPlayer(Unit unit)
    {
        SelectableUnit selectable = allPlayersUnits.Find(s => ReferenceEquals(unit, s.Unit));
        allPlayersUnits.Remove(selectable);
        if (allPlayersUnits.Count == 0)
        {
            uiController.ShowDefeat();
        }
    }

}
