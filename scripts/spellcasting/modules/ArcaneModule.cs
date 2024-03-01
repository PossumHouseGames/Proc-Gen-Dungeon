using Godot;
using ToolShed.Debug;


public partial class ArcaneModule : Node, ISpellCastingTarget
{
    // [SerializeField] private Interactable _interactable;
    // public Interactable Interaction => _interactable;
    // [SerializeField] private float _intensity = 1.5f;
    //
    // [SerializeField] protected Transform _circleContianer;

    private Material _mat;

    private Color _startColor;

    // public UnityAction<ArcaneModule> OnModuleSolved;

    private Vector3 startingPosition;

    protected virtual void Start()
    {
        // _interactable.FocusGainedAction.AddListener(HighlightModule);
        // _interactable.FocusLostAction.AddListener(NormalModule);
        // _interactable.InteractAction.AddListener(SelectModule);
        //
        // _mat = _interactable.GetComponent<Renderer>().material;
        // _startColor = _mat.GetColor("_EmissionColor");
        //
        // startingPosition = transform.position;

        //FloatAround();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        // if (InputManager.InputActions.CancelInput)
        //     CloseModule();
    }
    private void FloatAround()
    {
        // transform.DOLocalMove(UnityEngine.Random.insideUnitCircle * 0.2f + new Vector2(startingPosition.x, startingPosition.y), UnityEngine.Random.Range(15f, 25f))
        //     .OnComplete(delegate {
        //         FloatAround();
        //     });
    }

    public virtual void Init(ModuleData data)
    {
        // base class
    }

    public void CloseModule()
    {
        // _interactable.transform.DOLocalMoveZ(0f, 1f);
        // _interactable.transform.DOScale(1f, 1);
        // //InputManager.ToggleActionMap(InputManager.InputActions.Gameplay);
        // _interactable.Enabled = true;
        // if (this == MageController.Instance.CurrentTarget)
        //     MageController.Instance.SetSpellCastingTarget(null);
    }
    private void SelectModule()
    {
        // _interactable.transform.DOLocalMoveZ(-1f, 1f);
        // _interactable.transform.DOScale(2f, 1);
        // //InputManager.ToggleActionMap(InputManager.InputActions.UI);
        // _interactable.Enabled = false;
        //
        // MageController.Instance.SetSpellCastingTarget(this);
    }

    private void NormalModule()
    {
        // _mat.SetColor("_EmissionColor", _startColor);
    }

    private void HighlightModule()
    {
        // _mat.SetColor("_EmissionColor", _startColor * _intensity);
    }

    public virtual void SubmitRune(Rune rune)
    {
        GameDebug.Log("Base class call");
    }

    public virtual void SubmitRune(RuneController rune)
    {
        //Debug.Log("Base class call");
        // rune.transform.DOMove(_circleContianer.position, 1f);
        // rune.transform.DORotate(_circleContianer.rotation.eulerAngles, 1f);
        // Vector3 runeScale = rune.transform.localScale;
        // runeScale.x *= _circleContianer.localScale.x;
        // runeScale.y *= _circleContianer.localScale.y;
        // runeScale.z *= _circleContianer.localScale.z;
        // rune.transform.DOScale(runeScale, 1f).OnComplete(delegate { rune.transform.SetParent(transform.GetChild(0), true); });

    }

    public virtual void ResetModule()
    {

    }
}
