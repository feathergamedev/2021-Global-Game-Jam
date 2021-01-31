using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Transform dynamicUIPanel;

    [SerializeField] private GameObject hpBarPrefab;

    private List<UIHpBar> allHpBars;

    private void Awake()
    {
        instance = this;
        allHpBars = new List<UIHpBar>();
    }

    public UIHpBar RequestHpBar()
    {
        var newBar = Instantiate(hpBarPrefab, dynamicUIPanel).GetComponent<UIHpBar>();
        allHpBars.Add(newBar);

        return newBar;
    }

    public void CleanDynamicCanvas()
    {
        foreach (Transform c in dynamicUIPanel)
            Destroy(c.gameObject);
    }
}
