using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ToolShed.Debug;

public partial class BruteForceModule : ArcaneModule
{
    private Rune _solutionRune;
    private RuneController _runeToTestAgainst;
    private List<RuneController> strokeVisualizers = new List<RuneController>();

    [Export] private Color _correct = Colors.Green;
    [Export] private Color _existsButNotCorrect = Colors.Yellow;
    [Export] private Color _doesNotExist = Colors.Red;
    [Export] private Color _nodeColor = Colors.Green;
    [Export] private SpellNode[] _spellNodes;


    [ExportGroup("Debug")]
    [Export] private int _solutionStraight;
    [Export] private int _solutionShort;
    [Export] private int _solutionInner;
    [Export] private int _solutionCurves;

    public override void Init(ModuleData data)
    {
        BruteForceModuleData moduleData = (BruteForceModuleData) data;

        // Select a Rune
        _solutionRune = moduleData.RandomRune();

        // Rotate it
        _solutionRune.Rotate();

        _solutionRune.PerformStrokeAnalysis();

        _solutionStraight = _solutionRune.Strokes.Count(s => s.Type == Stroke.StrokeType.Straight);
        _solutionShort = _solutionRune.Strokes.Count(s => s.Type == Stroke.StrokeType.ShortStraight);
        _solutionInner = _solutionRune.Strokes.Count(s => s.Type == Stroke.StrokeType.InnerStraight);
        _solutionCurves = _solutionRune.Strokes.Count(s => s.Type == Stroke.StrokeType.Curve);
    }

    public override void SubmitRune(Rune rune)
    {
        // Analyze Strokes
        // Highlight correct stroke
        // Highlight correct but misplaced strokes
        // highlight incorrect strokes
        // If rune correct, but not roteated right, highlight nodes
        GameDebug.Log("Got rune");
    }

    public override void SubmitRune(RuneController rune)
    {
        ClearRuneEntry();

        _runeToTestAgainst = rune;

        base.SubmitRune(rune);

        // Invoke("AnalyzeRune", 1f);
    }

    private void ClearRuneEntry()
    {
        // if (_runeToTestAgainst)
        // {
        //     _runeToTestAgainst.gameObject.SetActive(true);
        //     GameObject objectToDestroy = _runeToTestAgainst.gameObject;
        //     _runeToTestAgainst.transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate { Destroy(objectToDestroy); });
        //
        //     foreach (var s in strokeVisualizers)
        //         Destroy(s.gameObject);
        //
        //     strokeVisualizers.Clear();
        //
        //     foreach (var sn in _spellNodes)
        //     {
        //         sn.Mat.DOColor(Color.clear, Shader.PropertyToID("_EmissionColor"), 0.5f);
        //         sn.Mat.DOColor(Color.clear, 0.5f);
        //     }
        // }
    }

    private void AnalyzeRune()
    {
        // Analyze Strokes
        // Highlight correct stroke
        // Highlight correct but misplaced strokes
        // highlight incorrect strokes
        // If rune correct, but not roteated right, highlight nodes

        // _runeToTestAgainst.Rune.PerformStrokeAnalysis();
        //
        // int straights = 0;
        // int shorts = 0;
        // int inner = 0;
        // int curves = 0;
        // bool runeMatches = true;
        // bool runeMatchesButIncorrectRotation = true;
        // float delayTime = 0.4f;
        // // foreach (Stroke s in _runeToTestAgainst.Rune.Strokes)
        // for (int i = 0; i < _runeToTestAgainst.Rune.Strokes.Count; i++)
        // {
        //     RuneController stroke = Instantiate(_runeToTestAgainst, _runeToTestAgainst.transform.position, _runeToTestAgainst.transform.rotation);
        //     stroke.transform.SetParent(transform.GetChild(0), true);
        //     stroke.Radius = _runeToTestAgainst.Radius;
        //     stroke.Set(new Rune(_runeToTestAgainst.Rune.Strokes[i].Nodes.ToArray()));
        //     strokeVisualizers.Add(stroke);
        //     Vector3 pos = stroke.transform.position;
        //     pos.z -= 0.01f * i;
        //     stroke.transform.position = pos;
        //
        //     IEnumerable<Stroke> runeStrokes = _solutionRune.Strokes.Where(ss => ss.Type == _runeToTestAgainst.Rune.Strokes[i].Type);
        //     bool strokeIsPresent = runeStrokes.Count() > 0;
        //
        //     // Debug.Log($"Stroke Present: {strokeIsPresent}");
        //
        //     if (strokeIsPresent)
        //     {
        //         bool strokeWithinCount = false;
        //
        //         switch (runeStrokes.First().Type)
        //         {
        //             case Stroke.StrokeType.Straight:
        //                 straights++;
        //                 strokeWithinCount = straights <= _solutionStraight;
        //                 break;
        //             case Stroke.StrokeType.ShortStraight:
        //                 shorts++;
        //                 strokeWithinCount = shorts <= _solutionShort;
        //                 break;
        //             case Stroke.StrokeType.InnerStraight:
        //                 inner++;
        //                 strokeWithinCount = inner <= _solutionInner;
        //                 break;
        //             case Stroke.StrokeType.Curve:
        //                 curves++;
        //                 strokeWithinCount = curves <= _solutionCurves;
        //                 break;
        //         }
        //
        //         bool runeEquals = false;
        //
        //         foreach (Stroke solutionStroke in runeStrokes)
        //         {
        //             if (solutionStroke.Equals(_runeToTestAgainst.Rune.Strokes[i]))
        //             {
        //                 runeEquals = true;
        //                 break;
        //             }
        //         }
        //
        //         if (runeEquals)
        //             stroke.Mat.DOColor(_correct, Shader.PropertyToID("_EmissionColor"), 1f).SetDelay(delayTime * i+1);
        //         else if (strokeWithinCount)
        //         {
        //             stroke.Mat.DOColor(_existsButNotCorrect, Shader.PropertyToID("_EmissionColor"), 1f).SetDelay(delayTime * i + 1);
        //             runeMatches = false;
        //         }
        //         else
        //         {
        //             stroke.Mat.DOColor(_doesNotExist, Shader.PropertyToID("_EmissionColor"), 1f).SetDelay(delayTime * i + 1);
        //             runeMatches = false;
        //             runeMatchesButIncorrectRotation = false;
        //         }
        //     }
        //     else
        //     {
        //         stroke.Mat.DOColor(_doesNotExist, Shader.PropertyToID("_EmissionColor"), 1f).SetDelay(delayTime * i + 1);
        //         runeMatches = false;
        //         runeMatchesButIncorrectRotation = false;
        //     }
        // }
        //
        // _runeToTestAgainst.gameObject.SetActive(false);
        //
        // if (runeMatchesButIncorrectRotation && !runeMatches)
        // {
        //     List<int> validNodes = new List<int>();
        //     int delayIndex = _runeToTestAgainst.Rune.Strokes.Count;
        //     // find matching nodes
        //     foreach (int node in _runeToTestAgainst.Rune.Nodes)
        //     {
        //         if (_solutionRune.Nodes.Contains(node))
        //         {
        //             delayIndex++;
        //             _spellNodes[node].Mat.DOColor(_nodeColor, Shader.PropertyToID("_EmissionColor"), 1f).SetDelay(delayTime * delayIndex);
        //             _spellNodes[node].Mat.DOColor(_correct, 1f).SetDelay(0.2f * delayIndex);
        //         }
        //     }
        // }
        // else if (runeMatches)
        // {
        //     // correct answer
        //     CloseModule();
        //     OnModuleSolved?.Invoke(this);
        // }
    }

    public override void ResetModule()
    {
        ClearRuneEntry();
    }
}
