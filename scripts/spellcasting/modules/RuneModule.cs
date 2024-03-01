using System.Collections;
using System.Collections.Generic;

public partial class RuneModule : ArcaneModule
{
    private Rune _solutionRune;
    private RuneController _runeToTestAgainst;

    public override void SubmitRune(RuneController rune)
    {
        ClearRuneEntry();

        base.SubmitRune(rune);
        _runeToTestAgainst = rune;

        // Invoke("AnalyzeRune", 1f);
    }

    private void AnalyzeRune()
    {
        // if (_runeToTestAgainst.Rune.Equals(_solutionRune))
        // {
        //     CloseModule();
        //     OnModuleSolved?.Invoke(this);
        // }
    }

    private void ClearRuneEntry()
    {
        // if (_runeToTestAgainst)
        // {
        //     _runeToTestAgainst.gameObject.SetActive(true);
        //     GameObject objectToDestroy = _runeToTestAgainst.gameObject;
        //     _runeToTestAgainst.transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate { Destroy(objectToDestroy); });
        // }
    }
}
