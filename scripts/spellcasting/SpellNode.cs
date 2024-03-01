using Godot;
using System;

public partial class SpellNode : Node
{
	[Export] private Color _highlightColor;
	[Export] private Color _selectedColor;

	private Material _mat;
	public Material Mat { get => _mat; set => _mat = value; }
	private Color _startColor;

	public bool Selected {get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// _mat = GetComponent<Renderer>().material;
		// _startColor = _mat.GetColor("_EmissionColor");
	}

	public void Highlight()
	{
		// if (!Selected)
		// 	_mat.SetColor("_EmissionColor", _highlightColor);
	}

	public void Normal()
	{
		// if (!Selected)
		// 	_mat.SetColor("_EmissionColor", _startColor);
	}

	public void Select()
	{
		Selected = true;
		// _mat.SetColor("_EmissionColor", _selectedColor);
	}

	public void Deselect()
	{
		Selected = false;
		Normal();
	}
}
