using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "BaseUnitData", menuName = "Unit/UnitData")]
public class BaseUnitData : ScriptableObject
{
    public BaseUnitData StrongMatch;

    public float StrongMatchBonus;

    public BaseUnitData BadMatch;

    public float BadMatchBonus;

    public GameObject PlayerPrefeb;

    public GameObject EnnemyPrefab;
}
