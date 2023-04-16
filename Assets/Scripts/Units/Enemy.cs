
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Enemy : MonoBehaviour
{
    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();    
    }

}
