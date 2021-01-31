using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UIThankYou : MonoBehaviour
{
    [SerializeField] private CameraFade cameraFade;
    [SerializeField] private Transform topPanel;
    [SerializeField] private CanvasGroup bottomLeftCanvasGroup;
    [SerializeField] private Transform bottomPanel;

    private bool isPressed = false;

    private void Awake()
    {
        bottomLeftCanvasGroup.alpha = 0;
        topPanel.localPosition = new Vector3(0, 765, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PerformSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isPressed)
                return;

            StartCoroutine(ReplaySequence());

            isPressed = true;
        }

    }

    private IEnumerator PerformSequence()
    {
        topPanel.DOLocalMoveY(0f, 2f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2.2f);

        DOTween.To(() => bottomLeftCanvasGroup.alpha, x => bottomLeftCanvasGroup.alpha = x, 1.0f, 1.5f);
    }

    private IEnumerator ReplaySequence()
    {
        cameraFade.FadeOut();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Main");
    }
}
