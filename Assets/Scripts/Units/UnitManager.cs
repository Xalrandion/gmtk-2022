using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private BaseUnitData MageData;
    [SerializeField] private BaseUnitData RangerData;
    [SerializeField] private BaseUnitData WarriorData;


    public BaseUnit GenerateUnit(bool isEnnemy)
    {
        var unitTemplates = new List<BaseUnitData> { MageData, RangerData, WarriorData };

        var selected = unitTemplates[Random.Range(0, 2)];

        var obj = Instantiate(isEnnemy ? selected.EnnemyPrefab : selected.PlayerPrefeb);
        var maxLife = Random.Range(0, 5);
        
        var resObj = obj.GetComponent<BaseUnit>();
        resObj.SetStats(maxLife, maxLife, Random.Range(1, 5));

        return resObj;
    }

    public void FuseUnits(BaseUnit target, BaseUnit component)
    {
        target.SetStats(target.HPgauge.Max + component.HPgauge.Max, target.HPgauge.Current + component.HPgauge.Current, target.BaseDamage + component.BaseDamage);
        Destroy(component);
    } 
}
