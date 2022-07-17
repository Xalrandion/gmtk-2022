using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatUI : MonoBehaviour
{
    [SerializeField] private BaseUnit unit;

    [SerializeField] private TMPro.TextMeshProUGUI UnitDamageText;
    [SerializeField] private TMPro.TextMeshProUGUI UnitHealthText;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UnitDamageText.SetText(unit.CalcDamage().ToString());
        UnitHealthText.SetText(Mathf.Ceil(unit.HPgauge.Current).ToString());
    }
}
