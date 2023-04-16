using UnityEngine;

[RequireComponent(typeof(Unit))]
public class HPBarController : MonoBehaviour
{
    [SerializeField] private Transform hpBarTransform;

    private Unit unit;

    private float maxSize;

    private void Start()
    {
        unit = GetComponent<Unit>();
        unit.NotifyHPChange += UpdateBar;

        maxSize = hpBarTransform.localScale.x;
    }

    private void UpdateBar()
    {
        float hpPercentChanged = unit.CurrentHP / unit.Info.HP;

        if (hpPercentChanged <= 0) unit.NotifyHPChange -= UpdateBar;

        hpBarTransform.localScale = new Vector3()
        {
            x = maxSize * hpPercentChanged,
            y = hpBarTransform.localScale.y
        };
    }

}
