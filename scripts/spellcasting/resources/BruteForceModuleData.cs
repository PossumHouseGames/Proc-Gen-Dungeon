using System;
using System.Collections;
using System.Collections.Generic;

public partial class BruteForceModuleData : ModuleData
{
    private Rune[] _runes;

    internal Rune RandomRune()
    {
        return _runes[new Random().Next(0, _runes.Length)];
    }
}
