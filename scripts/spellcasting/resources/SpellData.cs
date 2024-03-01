using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class SpellData : Resource
{
    private Rune _rune;
    public Rune Rune => _rune;

    public virtual void Cast(RuneController submittedRune)
    {
        // Find a point in front of the player
        // Vector3 targetPosition = Camera.main.transform.forward * 2f + Camera.main.transform.position;
        // submittedRune.transform.DOMove(targetPosition, .5f);
    }
}
