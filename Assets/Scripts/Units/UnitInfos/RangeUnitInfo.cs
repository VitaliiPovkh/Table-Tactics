using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Units/Range Unit")]
public class RangeUnitInfo : UnitInfo
{
    [SerializeField] private int ammoAmount;
    public int AmmoAmount => ammoAmount;
}
