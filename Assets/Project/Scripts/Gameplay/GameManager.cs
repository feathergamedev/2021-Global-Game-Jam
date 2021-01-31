using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public StageManager Stage;

    [SerializeField] private Text gameTimerText;
    private float gameTimer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        gameTimer = 0;

        LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;

        TimeSpan span = TimeSpan.FromSeconds((double)gameTimer);
        gameTimerText.text = $"{span.ToString("ss\\.fff")}";
    }

    public void LoadStage(int stageIndex)
    {
        Stage.LoadStage(stageIndex);
    }

    public void CheckAnswer(Potion potion)
    {
        var isCorrect = Stage.CheckAnswer(potion);

        if (isCorrect)
        {
            StageManager.instance.LoadNextStage();
            SoundManager.instance.PlaySFX(SFXType.Correct);
        }
        else
        {
            LabelManager.instance.DisposeCurrentPiece();
            SoundManager.instance.PlaySFX(SFXType.Wrong);
        }
    }
}
