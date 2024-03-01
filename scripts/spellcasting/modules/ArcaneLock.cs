using System;
using System.Collections;
using System.Collections.Generic;

using Godot;

public partial class ArcaneLock : Node
{   
    // [SerializeField] private LockData _data;
    // [SerializeField] private Transform _blockingField;
    // [SerializeField] private Transform _magicFieldTimer;
    // [SerializeField] private Transform _moduleContainer;
    //
    // [SerializeField] private Interactable _lockInteractable;

    private bool _useTimer;

    private bool _timerActive;
    private float _timeElapsed;
    private float _duration = 60;
    private int totalModules = 0;

    private List<ArcaneModule> _modules = new List<ArcaneModule>();
    private void Awake()
    {
        // foreach(ModuleData module in _data.Modules)
        // {
        //     var go = Instantiate(module.ModulePrefab, _moduleContainer);
        //     var mod = go.GetComponent<ArcaneModule>();
        //     mod.Init(module);
        //     mod.OnModuleSolved += HandleModuleSolved;
        //     totalModules++;
        //     _modules.Add(mod);
        //     mod.Interaction.Enabled = !_useTimer;
        // }
        //
        // _lockInteractable.Enabled = _useTimer;
    }

    private void HandleModuleSolved(ArcaneModule module)
    {
    //     module.transform.DOScale(Vector3.zero, 0.5f).SetDelay(1f)
    //         .OnComplete(delegate {
    //         module.gameObject.SetActive(false);
    //     }); ;
    //     totalModules--;
    //     if (totalModules == 0)
    //     {
    //         _timerActive = false;
    //         // Lock solved. 
    //         _blockingField.DOScaleY(0, 1f).SetDelay(2f)
    //             .OnComplete(delegate {
    //                 _blockingField.gameObject.SetActive(false);
    //             });
    //         _magicFieldTimer.DOScaleY(0, 1f).SetDelay(2f)
    //             .OnComplete(delegate {
    //                 _magicFieldTimer.gameObject.SetActive(false);
    // });
    //     }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // _lockInteractable.InteractAction.AddListener(BeginLockpicking);
    }

    private void BeginLockpicking()
    {
        // Begin Timer
        // _magicFieldTimer.DOScaleY(0, 1f).onComplete = () => _timerActive = true;

        // foreach(var mod in _modules)
        // {
        //     mod.Interaction.Enabled = true;
        // }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // if (_timerActive)
        // {
        //     _timeElapsed += Time.deltaTime;
        //
        //     var scaleChange  = _magicFieldTimer.localScale;
        //     scaleChange.y = Mathf.Clamp01(_timeElapsed / _duration);
        //     _magicFieldTimer.localScale = scaleChange;
        //
        //     if (_timeElapsed >= _duration)
        //     {
        //         _timerActive = false;
        //         _timeElapsed = 0;
        //         // Reset lock
        //         ResetLock();
        //     }
        // }
        //
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     totalModules = _data.Modules.Length;
        //     _blockingField.gameObject.SetActive(true);
        //     _magicFieldTimer.gameObject.SetActive(true);
        //
        //     _blockingField.DOScaleY(1, 1f);
        //     _magicFieldTimer.DOScaleY(1, 1f);
        //
        //     _timerActive = false;
        //     _timeElapsed = 0;
        //
        //     ResetLock();
        // }
    }

    public void ResetLock()
    {
        // foreach(var mod in _modules)
        // {
        //     mod.gameObject.SetActive(true);
        //     mod.transform.DOScale(1f, 0.5f).SetDelay(1f);
        //     mod.ResetModule();
        //     mod.CloseModule();
        //     mod.Interaction.Enabled = false;
        // }
    }
}
