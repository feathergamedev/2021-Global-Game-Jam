using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Camera Cam;

    [SerializeField] private CameraFade camFade;

    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 MousePosToWorldPos()
    {
        return Cam.ScreenToWorldPos(Input.mousePosition);
    }

    public Vector3 WorldPosToScreenPos(Vector3 worldPos)
    {
        var result = Cam.WorldToScreenPoint(worldPos);
        result.z = 0;
        return result;
    }

    public void CameraFadeOut()
    {
        camFade.FadeOut();
    }

    public void CameraFadeIn()
    {
        camFade.FadeIn();
    }

    public void CameraShake()
    {
        Cam.transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }
}