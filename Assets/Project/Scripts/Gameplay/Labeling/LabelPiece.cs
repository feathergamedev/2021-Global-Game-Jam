using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelPiece : MonoBehaviour
{
    public ELabelType LabelType;

    public EEffectType EffectType;
    public EEffectRate EffectRate;

    [SerializeField] private List<Color> allLabelColors;
    [SerializeField] private Image labelBackgroundImage;

    [SerializeField] private Text labelNameText;

    private bool isAssembled = false;

    public void Setup(EEffectType Type, EEffectRate Rate)
    {

        labelBackgroundImage.color = allLabelColors[Random.Range(0, allLabelColors.Count)];

        EffectType = Type;
        EffectRate = Rate;

        if (Type == EEffectType.ThankYou && Rate == EEffectRate.ThankYou)
        {
            labelNameText.text = $"ThankYou!";
        }
        else if (Type == EEffectType.None && Rate == EEffectRate.None)
        {
            labelNameText.text = $"Nothing";
        }
        else if (Type == EEffectType.None)
        {
            labelNameText.text = $"{Rate.Label()}";
        }
        else if(Rate == EEffectRate.None)
        {
            labelNameText.text = $"{Type.Label()}";
        }
        else
        {
            labelNameText.text = $"{Type.Label()} {Rate.Label()}";
        }
    }

    public void GetPointerEntered()
    {
        LabelManager.instance.GetOrderPriority(transform);
    }

    public void GetClicked()
    {
        if (isAssembled)
        {
            LabelManager.instance.DispersePiece(this);
            isAssembled = false;
        }
    }

    public void GetDragged()
    {
        if (isAssembled)
            return;

        LabelManager.instance.HoldingPiece(this);
    }

    public void GetReleased()
    {
        LabelManager.instance.ReleasePiece(this);
    }

    public void GetAssembled()
    {
        isAssembled = true;
    }
}
