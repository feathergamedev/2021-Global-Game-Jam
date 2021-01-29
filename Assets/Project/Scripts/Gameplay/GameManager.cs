using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public StageManager Stage;

    [SerializeField] private EEffectType curSelectType;
    [SerializeField] private EEffectRate curSelectRate;

    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        Stage.LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var myAnswer = ScriptableObject.CreateInstance<Potion>();
            myAnswer.Type = curSelectType;
            myAnswer.Rate = curSelectRate;
                //new Potion(curSelectType, curSelectRate);
            CheckAnswer(myAnswer);
        }
    }

    public void LoadStage(int stageIndex)
    {
        Stage.LoadStage(stageIndex);
    }

    public void CheckAnswer(Potion potion)
    {
        var isCorrect = Stage.CheckAnswer(potion);
    }
}
