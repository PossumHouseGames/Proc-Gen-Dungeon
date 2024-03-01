using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class LinkModule : ArcaneModule
{
    // [ExportGroup("Link Module")]
    private LinkModuleLink[] _links;

    public override void Init(ModuleData data)
    {
        LinkModuleData linkModuleData = (LinkModuleData) data;

        // for (int count = 0; count < _links.Length; count++)
        // {
        //     _links[count].gameObject.SetActive(count < linkModuleData.NumberOfLinks);
        //     _links[count].Set(linkModuleData);
        // }
    }
}
