using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Correct = 0,
    Wrong,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource[] allSFX;
    private void Awake()
    {
        instance = this;   
    }

    public void PlaySFX(SFXType type)
    {
        allSFX[(int)type].Play();
    }
}
