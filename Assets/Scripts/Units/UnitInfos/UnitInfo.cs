using UnityEngine;

public abstract class UnitInfo : ScriptableObject
{
    [SerializeField] private float unitPrice;
    [Space(20)]
    [SerializeField] private string unitName;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float damage;
    [Space(20)]
    [Range(0f, 3f)]
    [SerializeField] private float infantryModifire = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float cavalryModifire = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float siegeModifire = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float buildingModifire = 0f;
    [Space(20)]
    [SerializeField] private bool doesIgnoreCharge; 
    [SerializeField] private UnitHeaviness heaviness;
    [Space(20)]
    [SerializeField] private Sprite emblem;
    [SerializeField] private Sprite baseSprite;


    public float UnitPrice => unitPrice;
    public string UnitName => unitName;
    public float HP => hp;
    public float Speed => speed;
    public float AttackRange => attackRange;
    public bool DoesIgnoreCharge => doesIgnoreCharge;
    public UnitHeaviness Heaviness => heaviness;
    public float InfantryDamageModifire => infantryModifire;
    public float CavalryDamageModifire => cavalryModifire;
    public float SiegeDamageModifire => siegeModifire;
    public float BuildingDamageModifire => buildingModifire;
    public float Damage => damage;
    public Sprite Emblem => emblem;
    public Sprite BaseSprite => baseSprite; 
}
