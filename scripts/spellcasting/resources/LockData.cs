using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class LockData : Resource
{
    private ModuleData[] _modules;

    public ModuleData[] Modules => _modules;
    
    public void EvaluateForCompletion()
    {
        // Completed = Tasks.All(t => t.Completed);
    }
}
