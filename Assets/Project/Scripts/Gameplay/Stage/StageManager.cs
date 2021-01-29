using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    [SerializeField] private StageArguments curStage;
    [SerializeField] private List<StageArguments> allStages;

    private int curStageIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStage(int stageIndex)
    {
        if (stageIndex >= allStages.Count)
        {
            Debug.LogError("全數關卡已結束，進入Ending場景。");
        }
        else
        {
            curStageIndex = stageIndex;
            curStage = allStages[curStageIndex];
        }

        //TODO: 根據當前的StageArguments設置UI
    }

    public bool CheckAnswer(Potion potion)
    {
        var answer = curStage.Answer;
        var isCorrect = (potion.Type == answer.Type && potion.Rate == answer.Rate);

        if (isCorrect)
        {
            Debug.Log("CORRECT ANSWER!!!!!!!!");
            LoadStage(curStageIndex + 1);
            return true;
        }
        else
        {
            var failMsg = $"It should be " +
                $"<color=yellow>{answer.Rate.Label()} of {answer.Type}</color>" +
                $", your answer is " +
                $"<color=red>{potion.Rate.Label()} of {potion.Type}</color>";

            Debug.Log(failMsg);

            return false;
        }
    }
}
