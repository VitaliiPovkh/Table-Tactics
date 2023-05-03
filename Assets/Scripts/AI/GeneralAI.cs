using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class GeneralAI : MonoBehaviour
{
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
        });
    }

}
