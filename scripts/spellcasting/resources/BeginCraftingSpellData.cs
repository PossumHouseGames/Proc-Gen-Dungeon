using System.Collections;
using System.Collections.Generic;

public partial class BeginCraftingSpellData : SpellData
{
    // [SerializeField] private GameObject _magicCirclePrefab;
    // public override void Cast(RuneVisualizer submittedRune)
    // {
    //     base.Cast(submittedRune);
    //
    //     Vector3 targetPosition = Camera.main.transform.forward * 3f + Camera.main.transform.position;
    //
    //     var go = Instantiate(_magicCirclePrefab, targetPosition, Quaternion.identity);
    //     go.transform.rotation = Quaternion.LookRotation(targetPosition - Camera.main.transform.position);
    //     Sequence mySequence = DOTween.Sequence();
    //     mySequence
    //         .Append(submittedRune.transform.DOScale(0, .5f))
    //         .Append(go.transform.DOScale(3, .5f).From(0))
    //         .SetDelay(0.5f);        
    // }
}
