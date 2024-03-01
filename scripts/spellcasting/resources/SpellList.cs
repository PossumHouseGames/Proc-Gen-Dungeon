using System;
using System.Collections;
using System.Collections.Generic;
using Godot;


public partial class SpellList : Resource
{
    private List<SpellData> _spells;
    public List<SpellData> Spells => _spells;

    public bool Contains(RuneController submittedRune)
    {
        // foreach (var spell in _spells)
        // {
        //     if (spell.Rune.Equals(submittedRune.Rune) || spell.Rune.Equivalent(submittedRune.Rune))
        //         return true;
        // }

        return false;
    }

    public bool TryGetSpell(RuneController submittedRune, out SpellData spell)
    {
        // foreach (var s in _spells)
        // {
        //     if (s.Rune.Equals(submittedRune.Rune) || s.Rune.Equivalent(submittedRune.Rune))
        //     {
        //         spell = s;
        //         return true;
        //     }
        // }

        spell = null;
        return false;
    }
}
