using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = ToolShed.Debug.GameDebug;

public class Rune : System.IEquatable<Rune>
{
    int[] _nodes;

    private List<Stroke> _strokes = new List<Stroke>();

    public List<Stroke> Strokes => _strokes;

    private string _analysisDebug = "";

    public Rune(int[] nodes)
    {
        this._nodes = nodes;
    }

    public Rune(Rune rune)
    {
        this._nodes = rune.Nodes;
    }

    public int[] Nodes => _nodes;

    public void Rotate()
    {
        var random = new Random();
        int rotateAmount = random.Next(-8, 8);
        Rotate(rotateAmount);
    }

    public void Rotate(int rotateAmount)
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i] != 8)
            {
                _nodes[i] += rotateAmount;
                _nodes[i] = Wrap8(_nodes[i]);
            }
        }
    }

    public Rune GetRotated(int rotateAmount)
    {
        for (int i = 0; i < _nodes.Length; i++)
        {
            if (_nodes[i] != 8)
            {
                _nodes[i] += rotateAmount;
                _nodes[i] = Wrap8(_nodes[i]);
            }
        }
        return this;
    }


    internal void FromList(List<int> symbolPath)
    {
        _nodes = symbolPath.ToArray();
    }

    public int Wrap8(int val)
    {
        if (val < 0)
            val += 8;
        else if (val > 7)
            val -= 8;

        return val;
    }

    public void PerformStrokeAnalysis()
    {
        List<int> runeNodes = new List<int>(_nodes);
        _strokes = new List<Stroke>();

        // Debug
        string strokeString = "";
        foreach (var n in runeNodes)
            strokeString += $"{n},";
        strokeString = strokeString.Remove(strokeString.Length-1);
        _analysisDebug += $"Checkin strokes in [{strokeString}] \n";

        // Order Matters
        CheckStraightStrokes(ref runeNodes);
        CheckShortStraightStrokes(ref runeNodes);
        CheckForInnerStraightStrokes(ref runeNodes);
        CheckForCurves(ref runeNodes);

        Debug.Log(_analysisDebug);
    }

    private void CheckStraightStrokes(ref List<int> runeNodes)
    {
        // Check if not empty
        // Check if at least 2 nodes
        if (runeNodes.Count < 2) return;

        _analysisDebug += $"Checking for straight stroke for {runeNodes} \n";

        int currentIndex = 0;

        while (currentIndex + 1 < runeNodes.Count)
        {
            _analysisDebug += $"Current Index: {currentIndex} \n";

            // If 3 nodes, Difference between Mathf.Abs([0]-[2]) == 4 && [1] == 8
            if (currentIndex + 2 < runeNodes.Count)
            {
                if (runeNodes[currentIndex+1] == 8 &&
                    Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex + 2]) == 4)
                {
                    _analysisDebug += $"[3] Detected Straight stroke [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]},{runeNodes[currentIndex+2]}] \n";

                    // Then its a straight stroke
                    Stroke s = new Stroke();
                    s.Type = Stroke.StrokeType.Straight;
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    _strokes.Add(s);
                }
                else if (Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex + 1]) == 4)
                {
                    _analysisDebug += $"[3] Detected Straight stroke [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Then its a straight stroke
                    Stroke s = new Stroke();
                    s.Type = Stroke.StrokeType.Straight;
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    _strokes.Add(s);
                }
                else
                {
                    _analysisDebug += $"[3] Straight stroke Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]},{runeNodes[currentIndex+2]}] \n";

                    // Just increment index
                    currentIndex++;
                }
            }
            else if (currentIndex + 1 < runeNodes.Count)
            {
                // If 2 nodes, Difference between Mathf.Abs([0]-[1]) == 4
                if(Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex + 1]) == 4)
                {
                    _analysisDebug += $"[2] Detected Straight stroke in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Then its a straight stroke
                    Stroke s = new Stroke();
                    s.Type = Stroke.StrokeType.Straight;
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    _strokes.Add(s);
                }
                else
                {
                    _analysisDebug += $"[2] Straight stroke Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Just increment index
                    currentIndex++;
                }
            }
            else
            {
                Debug.LogWarning("Should never get here");

                // Just increment index
                currentIndex++;
            }
        }
    }

    private void CheckShortStraightStrokes(ref List<int> runeNodes)
    {
        // Check if not empty
        // Check if at least 2 nodes
        if (runeNodes.Count < 2) return;
        _analysisDebug += $"Checking for short straight stroke for {runeNodes} \n";

        int currentIndex = 0;

        while (currentIndex + 1 < runeNodes.Count)
        {
            if (currentIndex + 2 < runeNodes.Count)
            {
                // check either end of stroke
                bool notPartOfLongStroke =
                    (runeNodes[currentIndex] != 8) ?
                        (currentIndex > 1) ?
                            // Look ahead and back to nodes
                            ((Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex + 2]) != 4) &&
                            (Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex - 2]) != 4)) :
                            // or just look ahead 2 nodes
                            (Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex + 2]) != 4)
                    :
                        // if first node is 8 then true, or if prev and next node not 4 difference
                        (currentIndex <= 0) || Mathf.Abs(runeNodes[currentIndex - 1] - runeNodes[currentIndex + 1]) != 4;

                // Check 3 nodes.
                // If 2 nodes and one of them is 8 and if its not part of a long stroke
                if ((runeNodes[currentIndex] == 8 || runeNodes[currentIndex+1] == 8) && notPartOfLongStroke)
                {
                    _analysisDebug += $"Detected short straight stroke in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Then its a straight stroke
                    Stroke s = new Stroke();
                    s.Type = Stroke.StrokeType.ShortStraight;
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex]);
                    _strokes.Add(s);
                }
                else
                {
                    _analysisDebug += $"Short straight stroke Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Just increment index
                    currentIndex++;
                }
            }
            else if (currentIndex + 1 < runeNodes.Count) // Last 2 nodes
            {
                // check either end of stroke
                bool notPartOfLongStroke =
                    (runeNodes[currentIndex] != 8) ?
                        // look back a couple nodes (last node case)
                        (Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex - 2]) != 4)
                    :
                        // if prev and next node not 4 difference (second to last node case)
                        Mathf.Abs(runeNodes[currentIndex - 1] - runeNodes[currentIndex + 1]) != 4;

                // At the end of the stroke and theres only 2 nodes left
                // Check 2 nodes.
                // If 2 nodes and one of them is 8
                if ((runeNodes[currentIndex] == 8 || runeNodes[currentIndex + 1] == 8) && notPartOfLongStroke)
                {
                    _analysisDebug += $"Detected short straight stroke in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Then its a straight stroke
                    Stroke s = new Stroke();
                    s.Type = Stroke.StrokeType.ShortStraight;
                    s.Nodes.Add(runeNodes[currentIndex++]);
                    s.Nodes.Add(runeNodes[currentIndex]);
                    _strokes.Add(s);
                }
                else
                {
                    _analysisDebug += $"Short straight stroke Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Just increment index
                    currentIndex++;
                }
            }
        }
    }

    private void CheckForInnerStraightStrokes(ref List<int> runeNodes)
    {
        // Check if not empty
        // Check if at least 2 nodes
        if (runeNodes.Count < 2) return;

        _analysisDebug += $"Checking for inner straight stroke for {runeNodes} \n";

        int currentIndex = 0;

        while (currentIndex + 1 < runeNodes.Count)
        {
            // Check 2 nodes.
            // If 2 nodes, Difference between Mathf.Abs([0]-[1]) == 2
            if ((Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 2 ||
                Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 6 ||  // for the 2 lines that cross the boundary
                Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 3 ||  // triganglular
                Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 5 ) &&   // boundaries 
                !(runeNodes[currentIndex] == 8 || runeNodes[currentIndex + 1] == 8)) // and not part of short straight
            {
                _analysisDebug += $"Detected inner straight stroke in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                // Then its a straight stroke
                Stroke s = new Stroke();
                s.Type = Stroke.StrokeType.InnerStraight;
                s.Nodes.Add(runeNodes[currentIndex++]);
                s.Nodes.Add(runeNodes[currentIndex++]);
                _strokes.Add(s);
            }
            else
            {
                _analysisDebug += $"Inner straight stroke Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                // Just increment index
                currentIndex++;
            }
        }
    }

    private void CheckForCurves(ref List<int> runeNodes)
    {
        // Check if not empty
        // Check if at least 2 nodes
        if (runeNodes.Count < 2) return;

        _analysisDebug += $"Checking for curve stroke for {runeNodes} \n";

        int currentIndex = 0;
        bool runStarted = false;
        Stroke s = new Stroke();

        while (currentIndex + 1 < runeNodes.Count)
        {
            // Center node is never part of curve
            if (runeNodes[currentIndex] == 8)
            {
                _analysisDebug += $"Skipping {runeNodes[currentIndex]} \n";
                currentIndex++;
                continue;
            }

            // Check 2 nodes.
            // If 2 nodes, Difference between Mathf.Abs([0]-[1]) == 1
            if ((Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 1 ||
                Mathf.Abs(runeNodes[currentIndex] - runeNodes[currentIndex+1]) == 7 ) &&
                runeNodes[currentIndex] != 8 && runeNodes[currentIndex+1] != 8) // for the curves that cross the boundary
            {
                if (!runStarted) // Beginning of a run
                {
                    runStarted = true; // Start run
                    s = new Stroke();
                    s.Type = Stroke.StrokeType.Curve;
                }
                // Still in run

                // Add current index and increment. We know the next is part of it but
                // we need to compare it against its next, so we increment once
                s.Nodes.Add(runeNodes[currentIndex++]);
            }
            else
            {
                if (runStarted) // run was started and now ended
                {
                    // The current index and its next are not sequential but we know the current
                    // index is part of the previous run since run was previously started.

                    // Add current index
                    s.Nodes.Add(runeNodes[currentIndex++]);

                    // Add curve stroke to list of strokes
                    _strokes.Add(s);

                    // End run
                    runStarted = false;

                    // Debug
                    string strokeString = "";
                    foreach (var n in s.Nodes)
                        strokeString += $"{n},";
                    strokeString = strokeString.Remove(strokeString.Length-1);
                    _analysisDebug += $"Detected curve in {strokeString} \n";
                }
                else // Run was not started
                {
                    _analysisDebug += $"Curve Not detected in [{runeNodes[currentIndex]},{runeNodes[currentIndex + 1]}] \n";

                    // Just increment index
                    currentIndex++;
                }
            }
        }

        // Last one
        if (runStarted) // run was started and now ended
        {
            // The current index and its next are not sequential but we know the current
            // index is part of the previous run since run was previously started.

            // Add current index
            s.Nodes.Add(runeNodes[currentIndex]);

            // Add curve stroke to list of strokes
            _strokes.Add(s);

            // Debug
            string strokeString = "";
            foreach (var n in s.Nodes)
                strokeString += $"{n},";
            strokeString = strokeString.Remove(strokeString.Length-1);
            _analysisDebug += $"Detected curve in {strokeString} \n";
        }
    }

    public bool Equals(Rune other)
    {
        if (other.Nodes.Length != Nodes.Length)
            return false;

        for (int index = 0; index < Nodes.Length; index++)
        {
            if (Nodes[index] != other.Nodes[index])
                return false;
        }

        return true;
    }

    public bool Equivalent(Rune other)
    {
        if (IsCyclic() && other.IsCyclic())
        {
            return IsRotated(other);
        }

        return false;
    }

    private bool IsRotated(Rune other)
    {
        if (Equals(other))
            return true;

        for (int count = 0; count < 7; count++)
        {
            if (Equals(other.GetRotated(1)))
                return true;
        }

        return false;
    }

    public bool IsCyclic()
    {
        if (Nodes.Length >1)
            return Nodes[0] == Nodes[Nodes.Length - 1];
        return false;
    }
}

public class Stroke
{
    private List<int> _nodes;
    public List<int> Nodes { get => _nodes; set => _nodes = value; }
    public enum StrokeType {None, Straight, ShortStraight, InnerStraight, Curve }
    private StrokeType _type;
    public StrokeType Type { get => _type; set => _type = value; }

    public Stroke()
    {
        Nodes = new List<int>();
    }

    public bool Equals(Stroke s)
    {
        if ( this == null && s == null )
            return true;

        if ((this != null && s == null) ||
            (this == null && s != null))
        {
            return false;
        }

        if (Nodes.Count != s.Nodes.Count)
            return false;

        bool equal = true;

        for (int i = 0; i < Nodes.Count; i++ )
        {
            if (Nodes[i] != s.Nodes[i])
            {
                equal = false;
                break;
            }
        }

        return equal;
    }

    // public static bool operator ==(Stroke lhs, Stroke rhs)
    // {
    //     // if (lhs == null && rhs == null)
    //     //     return true;

    //     // if ((lhs != null && rhs == null) ||
    //     //     (lhs == null && rhs != null))
    //     // {
    //     //     return false;
    //     // }

    //     if (lhs.Count != rhs.Count)
    //         return false;

    //     bool equal = true;

    //     for (int i = 0; i < lhs.Count; i++ )
    //     {
    //         if (lhs[i] != rhs[i])
    //         {
    //             equal = false;
    //             break;
    //         }
    //     }

    //     return equal;
    // }

    // public static bool operator !=(Stroke lhs, Stroke rhs)
    // {
    //     if (lhs == null && rhs == null)
    //         return false;

    //     if ((lhs == null && rhs != null) ||
    //         (lhs != null && rhs == null))
    //     {
    //         return true;
    //     }

    //     if (lhs.Count != rhs.Count)
    //         return true;

    //     bool notEqual = false;

    //     for (int i = 0; i < lhs.Count; i++ )
    //     {
    //         if (lhs[i] != rhs[i])
    //         {
    //             notEqual = true;
    //             break;
    //         }
    //     }

    //     return notEqual;
    // }
}
