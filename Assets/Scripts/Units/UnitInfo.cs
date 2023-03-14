using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Unit")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private float unitPrice;
    [Space(20)]
    [SerializeField] private string unitName;
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [Space(20)]
    [SerializeField] private bool doesIgnoreCharge;
    [SerializeField] private UnitHeaviness heaviness;
    [Space(20)]
    [SerializeField] private float infantryDamageModifire = 1f;
    [SerializeField] private float cavalryDamageModifire = 1f;
    [SerializeField] private float siegeDamageModifire = 1f;
    [Space(20)]
    [SerializeField] private float chargeDamageBonus;
    [Space(20)]
    [SerializeField] private bool isRanger;
    [SerializeField] private int ammoAmount;
    [Space(20)]
    [SerializeField] private Sprite emblem;
    [SerializeField] private Sprite baseSprite;


    public float UnitPrice => unitPrice;
    public string UnitName => unitName;
    public float HP => hp;
    public float Speed => speed;
    public float Damage => damage;
    public float AttackRange => attackRange;
    public bool DoesIgnoreCharege => doesIgnoreCharge;
    public UnitHeaviness Heaviness => heaviness;
    public float InfantryDamageModifire => infantryDamageModifire;
    public float CavalryDamageModifire => cavalryDamageModifire;
    public float SiegeDamageModifire => siegeDamageModifire;
    public bool IsRanger => isRanger;
    public int AmmoAmount => ammoAmount;
    public Sprite Emblem => emblem;
    public Sprite BaseSprite => baseSprite; 
}
