
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Enemy : MonoBehaviour
{
    private Unit unit;

    /* TODO:
     * - make cavalry sprites
     * - implement cavalry
     * - start base AI implementation
     * - imple,emt archers
     * - improvae AI with archers
     * - make some new maps
     * **/
    private void Start()
    {
        unit = GetComponent<Unit>();    
    }

    public Unit Unit => unit;

}
