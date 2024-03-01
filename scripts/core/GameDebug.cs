using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ToolShed.Debug;

public static class GameDebug
{
	private static List<IDebugWrapper> debugWrappers = new List<IDebugWrapper>();
	
	public static void Log(string message)
	{
		foreach (var wrapper in debugWrappers)
			wrapper.Log(message);
	}
	
	public static void LogWarning(string message)
	{
		foreach (var wrapper in debugWrappers)
			wrapper.LogWarning(message);
	}
	
	public static void LogError(string message)
	{
		foreach (var wrapper in debugWrappers)
			wrapper.LogError(message);
	}

	public static void RegisterDebugWrapper(IDebugWrapper wrapper)
	{
		if (!debugWrappers.Contains(wrapper))
		{
			debugWrappers.Add(wrapper);
			Log($"Registered: {wrapper.Name}.");
		}
		else
		{
			LogError($"{wrapper.Name} already registered.");
		}
	}
}

public interface IDebugWrapper
{
	string Name { get; }
	void Log(string message);
	void LogWarning(string message);
	void LogError(string message);
}
