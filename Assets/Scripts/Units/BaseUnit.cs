using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] public Gauge HPgauge = new(5 , 5);

    public BaseUnitData UnitData
    {
        get => unitData;
        protected set { unitData = value; }
    }
    [SerializeField] private BaseUnitData unitData;
    public float BaseDamage
    {
        get => baseDamage;
        private set { baseDamage = value; }
    }
    [SerializeField] private float baseDamage = 1;

    // Start is called before the first frame updat
    public float CalcDamage()
    {
        return BaseDamage;
    }

    private float CalcClassDmgBonus(BaseUnit target)
    {
        if (target.UnitData == unitData.BadMatch)
        {
            return unitData.BadMatchBonus;
        }
        if (target.UnitData == unitData.StrongMatch)
        {
            return unitData.StrongMatchBonus;
        }
        return 0;
    }

    public float CalcDamage(BaseUnit target)
    {
        var baseDmg = CalcDamage();
        return baseDmg + (baseDmg * CalcClassDmgBonus(target));
    }

    // Returns overflow damage
    public float Attack(BaseUnit target)
    {
        var inflictedDmg = CalcDamage(target);
        Debug.Log(gameObject.name + " attacks " + target.name + "for " + inflictedDmg + "damage has " + HPgauge.Current + "/" + HPgauge.Max + " hp");
        return target.ReceiveDamage(inflictedDmg, this);
    }   
    
    // Negative damage would heal
    // Returns overflow damage
    public float ReceiveDamage(float damage, BaseUnit attacker)
    {
        float nextHP = HPgauge.Current - damage;

        HPgauge.Current = nextHP;
        Debug.Log(gameObject.name + " attacked by " + attacker.name + " and received " + damage + " damages has " + HPgauge.Current + "/" + HPgauge.Max + " hp");
        return Mathf.Clamp(nextHP, float.MinValue, 0);
    }

    public void SetStats(float maxLife, float currentLife, float damage)
    {
        HPgauge.Max = maxLife;
        HPgauge.Current = currentLife;
        baseDamage = damage;
    }
}
