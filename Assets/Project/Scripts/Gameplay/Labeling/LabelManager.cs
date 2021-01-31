using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class LabelManager : MonoBehaviour
{
    public static LabelManager instance;

    [TitleGroup("Basic")]
    [SerializeField] private Button checkButton;
    private RectTransform rectTransform;


    [TitleGroup("Piece")]
    [SerializeField] private GameObject fullPiecePrefab;
    [SerializeField] private GameObject effectTypePiecePrefab;
    [SerializeField] private GameObject effectRatePiecePrefab;
    [SerializeField] private Transform labelPoolTransform;
    [SerializeField] private Transform orderPriorityTransform;
    [SerializeField] private RectTransform allowedMovingArea;

    private LabelPiece currentHoldingPiece;

    private List<LabelPiece> assembledPieces;
    private Potion assembledPotion;
    private bool isFullyAssembled = false;

    public bool isHoldingPiece => currentHoldingPiece != null;

    public bool IsFullyAssemBled
    {
        get
        {
            return isFullyAssembled;
        }
        set
        {
            isFullyAssembled = value;
            checkButton.interactable = isFullyAssembled;
            checkButton.GetComponent<Animator>().SetBool("isActivated", isFullyAssembled);
        }
    }

    private void Awake()
    {
        instance = this;

        rectTransform = GetComponent<RectTransform>();
        assembledPieces = new List<LabelPiece>();
        assembledPotion = ScriptableObject.CreateInstance<Potion>();        

        IsFullyAssemBled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        checkButton.onClick.AddListener(() => GameManager.instance.CheckAnswer(assembledPotion));
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingPiece)
        {
            var isPointerInSensor = rectTransform.rect.Contains(currentHoldingPiece.transform.localPosition);
            var position = CameraManager.instance.Cam.ScreenToWorldPos(Input.mousePosition);

            if (isPointerInSensor)
            {
                AssemblePiece(currentHoldingPiece);
            }
            else
            {
                var initPos = currentHoldingPiece.transform.position;

                currentHoldingPiece.transform.position = position;

                var isPostionAllowed = allowedMovingArea.rect.Contains(currentHoldingPiece.transform.localPosition);
                if (!isPostionAllowed)
                    currentHoldingPiece.transform.position = initPos;
            }
        }
    }

    public void SetupPieces(StageArguments args)
    {
        IsFullyAssemBled = false;

        RemoveOldPieces();

        for (var i = 0; i < args.potionCandidates.Count; i++)
        {
            var newPiece = Instantiate(fullPiecePrefab, labelPoolTransform).GetComponent<LabelPiece>();
            newPiece.transform.localScale = Vector3.one;
            newPiece.Setup(args.potionCandidates[i].Type, args.potionCandidates[i].Rate);
            ThrowPieceAway(newPiece);
        }

        void RemoveOldPieces()
        {
            foreach (Transform piece in labelPoolTransform)
                Destroy(piece.gameObject);

            foreach (Transform piece in orderPriorityTransform)
                Destroy(piece.gameObject);

            foreach (LabelPiece piece in assembledPieces)
                Destroy(piece.gameObject);

            assembledPieces.Clear();
        }
    }

    public void AssemblePiece(LabelPiece piece)
    {
        var beReplaced = assembledPieces.Find(x => x.LabelType == piece.LabelType);
        if (beReplaced != null)
        {
            beReplaced.GetClicked();
        }

        assembledPieces.Add(piece);
        currentHoldingPiece.transform.SetParent(transform);
        var newPiecePos = Vector3.zero;

        var pieceAmount = assembledPieces.Count;
        if (pieceAmount != 0)
        {
            if (pieceAmount == 1)
            {
                IsFullyAssemBled = piece.LabelType == ELabelType.Full;
                assembledPotion.Set(piece.EffectType, piece.EffectRate);
                newPiecePos = Vector3.zero;
            }
        }
        else
        {
            IsFullyAssemBled = false;
        }

        currentHoldingPiece.transform.localPosition = newPiecePos;
        currentHoldingPiece = null;

        piece.GetAssembled();
    }

    public void DispersePiece(LabelPiece piece)
    {
        assembledPieces.Remove(piece);
        piece.transform.SetParent(labelPoolTransform);
        IsFullyAssemBled = false;

        ThrowPieceAway(piece);
    }

    public void GetOrderPriority(Transform piece)
    {
        if (orderPriorityTransform.childCount != 0)
            orderPriorityTransform.GetChild(0).transform.SetParent(labelPoolTransform);

        piece.transform.SetParent(orderPriorityTransform);

        orderPriorityTransform.SetAsLastSibling();
    }

    public void HoldingPiece(LabelPiece piece)
    {
        currentHoldingPiece = piece;
    }

    public void ReleasePiece(LabelPiece piece)
    {
        currentHoldingPiece = null;
    }

    public void OnPointerEnter()
    {
        if (isHoldingPiece)
        {           
            Debug.Log($"Install {currentHoldingPiece.gameObject.name}.");
        }
    }

    public void OnPointerExit()
    {
        if (isHoldingPiece)
        {
            Debug.Log($"Uninstall {currentHoldingPiece.gameObject.name}.");
        }
    }

    private void ThrowPieceAway(LabelPiece piece)
    {
        var randomSeed = Random.Range(1, 101);
        var newRandomPos = Vector3.zero;
        if (randomSeed < 50)
            newRandomPos = new Vector3(Random.Range(-700f, -395), Random.Range(0, 280));
        else
            newRandomPos = new Vector3(Random.Range(397, 700f), Random.Range(0, 280));

        piece.transform.DOLocalMove(newRandomPos, Random.Range(0.2f, 0.5f)).SetEase(Ease.OutQuint);
    }

    public void DisposeCurrentPiece()
    {
        foreach (LabelPiece piece in assembledPieces)
            Destroy(piece.gameObject);

        assembledPieces.Clear();

        IsFullyAssemBled = false;
    }
}
