@tool
extends EditorScript


# Called when the script is executed (using File -> Run in Script Editor).
func _run():
	var csharpHelper = load("res://scripts/editor/RoomShapeImporterEditor.cs") 
	var helper = csharpHelper.new()
	helper._run()
	pass
