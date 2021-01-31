using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [TitleGroup("Basic")]
    [SerializeField] private AudioSource BGM;
    [SerializeField] private PlayerController player;
    [SerializeField] private StageBase curStage;
    [SerializeField] private StageBase curDemoEnvironment;
    [SerializeField] private StageArguments curStageArguments;

    [SerializeField] private List<GameObject> classifiedStage;

    [SerializeField] private Transform tutorialPanel;

    [TitleGroup("Demo Environment")]
    [SerializeField] private Text demoStateText;
    [SerializeField] private CanvasGroup nightLayout;

    [TitleGroup("UI")]
    [SerializeField] private Text curStageText;

    //第幾個StageBase
    private int curStageIndex = 0;

    //同個StageBase裡面的ArgumentsCandidate編號
    private int curArgsIndex = -1;

    private bool isPotionCurrentlyUsed = false;
    private int totalStageCount;
    private int curStageCount;

    private void Awake()
    {
        instance = this;

        for (var i = 0; i < classifiedStage.Count; i++)
        {
            totalStageCount += classifiedStage[i].GetComponent<StageBase>().argumentCandidates.Count;
        }

        DOTween.To(() => BGM.volume, x => BGM.volume = x, 0.6f, 1.0f);
    }

    public void DemoRoundFinished()
    {
        isPotionCurrentlyUsed = !isPotionCurrentlyUsed;
        ResetDemoEnvironment(isPotionCurrentlyUsed);
    }

    private void ResetDemoEnvironment(bool isPotionUsed)
    {
        if (curDemoEnvironment != null)
            Destroy(curDemoEnvironment.gameObject);

        UIManager.instance.CleanDynamicCanvas();

        curDemoEnvironment = Instantiate(curStage, transform).GetComponent<StageBase>();

        if (isPotionUsed)
        {
            demoStateText.text = "AFTER";
            demoStateText.color = Color.white;
            player.Setup(curStageArguments.PlayerInitPos, curStageArguments.Answer);

            DOTween.To(() => nightLayout.alpha, x => nightLayout.alpha = x, 0.5f, 0.5f);
        }
        else
        {
            demoStateText.text = "BEFORE";
            demoStateText.color = Color.black;
            player.Setup(curStageArguments.PlayerInitPos);

            DOTween.To(() => nightLayout.alpha, x => nightLayout.alpha = x, 0.3f, 0.5f);
        }
    }

    public bool LoadStage(int argsIndex)
    {
        curStageCount++;
        curStage = classifiedStage[curStageIndex].GetComponent<StageBase>();
        curArgsIndex = argsIndex;

        curStageText.text = $"Stage {curStageCount}/{totalStageCount}";

        if (curArgsIndex >= curStage.argumentCandidates.Count)
        {
            curStageIndex++;

            if (curStageIndex >= classifiedStage.Count)
            {
                Debug.LogError("No more stages.");
                StartCoroutine(EnterCreditPagePerformance());
                return false;
            }
            else
            {
                curStage = classifiedStage[curStageIndex].GetComponent<StageBase>();
                curArgsIndex = 0;
            }
        }
        else
        {
            Debug.LogError($"載入第{curArgsIndex}個Arguments.");
        }

        isPotionCurrentlyUsed = false;

        //答案從候選名單裡面隨便挑一個
        var argsCount = curStage.argumentCandidates.Count;
        curStageArguments = curStage.argumentCandidates[curArgsIndex];

        var potionCount = curStageArguments.potionCandidates.Count;
        curStageArguments.Answer = curStageArguments.potionCandidates[Random.Range(0, potionCount)];

        ResetDemoEnvironment(false);
        LabelManager.instance.SetupPieces(curStageArguments);

        return true;
    }

    public void LoadNextStage()
    {
        StartCoroutine(EnterNextStagePerformance());
    }


    private IEnumerator EnterCreditPagePerformance()
    {
        DOTween.To(() => BGM.volume, x => BGM.volume = x, 0f, 1.0f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ThankYou");
    }

    private IEnumerator EnterNextStagePerformance()
    {
        CameraManager.instance.CameraFadeOut();

        yield return new WaitForSeconds(0.65f);

        tutorialPanel.gameObject.SetActive(false);
        var isSuccessed = LoadStage(curArgsIndex + 1);

        if (isSuccessed)
        {
            yield return new WaitForSeconds(0.2f);
            CameraManager.instance.CameraFadeIn();
        }
    }

    public bool CheckAnswer(Potion potion)
    {
        var answer = curStageArguments.Answer;
        var isCorrect = (potion.Type == answer.Type && potion.Rate == answer.Rate);

        if (isCorrect)
        {
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
