using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHpBar : MonoBehaviour
{

    [SerializeField] private Image curHpBar;

    [SerializeField] private Transform target;

    public void Init(Transform target)
    {
        this.target = target;
        transform.position = target.position;

    }

    private void Update()
    {
        if (target)
            transform.position = target.position;
    }

    public void Set(float ratio)
    {
        DOTween.To(() => curHpBar.fillAmount, x => curHpBar.fillAmount = x, ratio, 0.2f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
