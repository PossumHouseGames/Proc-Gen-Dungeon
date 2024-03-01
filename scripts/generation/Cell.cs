using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolShed.MazeGeneration
{
    public class Cell 
    {
        private HashSet<Cell> _links;

        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Level { get; private set; }

        public Cell North { get; set; }
        public Cell South { get; set; }
        public Cell East { get; set; }
        public Cell West { get; set; }
        public Cell Up { get; set; }
        public Cell Down { get; set; }

        public Cell[] Links => _links.ToArray();

        public enum NeighborDirection
        {
            North, South, East, West, Up, Down
        }
        public List<Cell> Neighbors
        {
            get
            {
                List<Cell> neighbors = new List<Cell>();
                if (North != null) neighbors.Add(North);
                if (South != null) neighbors.Add(South);
                if (East != null) neighbors.Add(East);
                if (West != null) neighbors.Add(West);
                if (Up != null) neighbors.Add(Up);
                if (Down != null) neighbors.Add(Down);
                return neighbors;
            }
        }
        
        public int CellIndex { get; set; } 

        public List<T> GetNeighbors<T>() where T : Cell
        {
            var neighbors = new List<T>();
            if (North != null) neighbors.Add((T)North);
            if (South != null) neighbors.Add((T)South);
            if (East != null) neighbors.Add((T)East);
            if (West != null) neighbors.Add((T)West);
            if (Up != null) neighbors.Add((T)Up);
            if (Down != null) neighbors.Add((T)Down);
            return neighbors;
        }

        public Cell(int row, int column, int level = 1)
        {
            Initialize(row, column, level);
        }

        public void Initialize(int row, int column, int level = 1)
        {
            this.Row = row;
            this.Column = column;
            this.Level = level;

            _links = new HashSet<Cell>();
        }

        public void Link(Cell cell, bool bidirectional = true)
        {
            _links.Add(cell);
            if (bidirectional) cell.Link(this, false);
        }
        
        public void Unlink(Cell cell, bool bidirectional = true)
        {
            _links.Remove(cell);
            if (bidirectional) cell.Unlink(this, false);
        }

        public void AddNeighbor(Cell cell, NeighborDirection direction, bool bidirectional = true)
        {
            switch (direction)
            {
                case NeighborDirection.North:
                    North = cell;
                    if (bidirectional) cell.South = this;
                    break;
                case NeighborDirection.South:
                    South = cell;
                    if (bidirectional) cell.North = this;
                    break;
                case NeighborDirection.East:
                    East = cell;
                    if (bidirectional) cell.West = this;
                    break;
                case NeighborDirection.West:
                    West = cell;
                    if (bidirectional) cell.East = this;
                    break;
                case NeighborDirection.Up:
                    Up = cell;
                    if (bidirectional) cell.Down = this;
                    break;
                case NeighborDirection.Down:
                    Down = cell;
                    if (bidirectional) cell.Up = this;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
        }

        public void RemoveNeighbor(NeighborDirection direction, bool bidirectional = true)
        {
            switch (direction)
            {
                case NeighborDirection.North:
                    if (bidirectional && North != null) North.South = null;
                    North = null;
                    break;
                case NeighborDirection.South:
                    if (bidirectional && South != null) South.North = null;
                    South = null;
                    break;
                case NeighborDirection.East:
                    if (bidirectional && East != null) East.West = null;
                    East = null;
                    break;
                case NeighborDirection.West:
                    if (bidirectional && West != null) West.East = null;
                    West = null;
                    break;
                case NeighborDirection.Up:
                    if (bidirectional && Up != null) Up.Down = null;
                    Up = null;
                    break;
                case NeighborDirection.Down:
                    if (bidirectional && Down != null) Down.Up = null;
                    Down = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public bool IsLinked(Cell cell) => cell != null && _links.Contains(cell);

        // public override string ToString() => $"Cell ({Row}, {Column}{(Level > 1 ? $", {Level}": "")})";
        public override string ToString() => $"Cell ({Column}, {Level}, {Row})";

        /// <summary>
        /// Run simple Djikstras to get the distance from this cell to every other cell
        /// in this cell's grid.
        /// </summary>
        /// <returns>The distances from this cell to every other.</returns>
        public Distances GetDistancesFromCell()
        {
            Distances distances = new Distances(this);

            List<Cell> frontier = new List<Cell> { this };

            while (frontier.Count > 0)
            {
                List<Cell> newFrontier = new List<Cell>();

                foreach (var cell in frontier)
                {
                    foreach (var linkedCell in cell.Links)
                    {
                        if (distances.Cells.ContainsKey(linkedCell)) continue;
                        distances[linkedCell] = distances[cell] + 1;
                        newFrontier.Add(linkedCell);
                    }
                }

                frontier = newFrontier;
            }

            return distances;
        }

        public bool IsEndCell()
        {
            int totalLinks = _links.Count;
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);
            if (hasUpLink) totalLinks--;
            if (hasDownLink) totalLinks--;
            
            if (totalLinks != 1)
                return false;
            
            return true;
        }

        public bool IsICell()
        {
            int totalLinks = _links.Count;
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);
            if (hasUpLink) totalLinks--;
            if (hasDownLink) totalLinks--;
            
            if (totalLinks!= 2)
                return false;
            
            return (hasNorthLink && hasSouthLink) || (hasWestLink && hasEastLink);
        }

        public bool IsLCell()
        {
            int totalLinks = _links.Count;
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);
            if (hasUpLink) totalLinks--;
            if (hasDownLink) totalLinks--;
            
            if (totalLinks!= 2)
                return false;

            return (hasNorthLink && hasEastLink) || (hasWestLink && hasNorthLink) || 
                   (hasSouthLink && hasEastLink) || (hasWestLink && hasSouthLink) ;
        }

        public bool IsTCell()
        {
            int totalLinks = _links.Count;
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);
            if (hasUpLink) totalLinks--;
            if (hasDownLink) totalLinks--;

            if (totalLinks!= 3)
                return false;
            
            return (hasNorthLink && hasEastLink && hasSouthLink) || (hasEastLink && hasSouthLink && hasWestLink) ||
                   (hasSouthLink && hasWestLink && hasNorthLink) || (hasWestLink && hasNorthLink && hasEastLink);
        }

        public bool IsCrossCell()
        {
            int totalLinks = _links.Count;
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);
            if (hasUpLink) totalLinks--;
            if (hasDownLink) totalLinks--;
            
            if (totalLinks != 4)
                return false;

            return hasNorthLink && hasEastLink && hasSouthLink && hasWestLink;
        }

        /// <summary>
        /// Has to match RoomData::GetRotation
        /// </summary>
        /// <returns></returns>
        public int GetRotation()
        {
            GetLinkDirections(out var hasNorthLink, out var hasSouthLink, out var hasWestLink, out var hasEastLink, out var hasUpLink, out var hasDownLink);

            if (IsEndCell())
            {
                if (hasNorthLink)
                    return 0;
                if (hasEastLink)
                    return 90;
                if (hasSouthLink)
                    return 180;
                if (hasWestLink)
                    return 270;
            }
            else if (IsICell())
            {
                if (hasNorthLink && hasSouthLink)
                    return 0;
                if (hasWestLink && hasEastLink)
                    return 90;
            }
            else if (IsLCell())
            {
                if (hasNorthLink && hasEastLink)
                    return 0;
                if (hasEastLink && hasSouthLink)
                    return 90;
                if (hasSouthLink && hasWestLink)
                    return 180;
                if (hasWestLink && hasNorthLink)
                    return 270;
            }
            else if (IsTCell())
            {
                if (hasEastLink && hasSouthLink && hasWestLink)
                    return 0;
                if (hasSouthLink && hasWestLink && hasNorthLink)
                    return 90;
                if (hasWestLink && hasNorthLink && hasEastLink)
                    return 180;
                if (hasNorthLink && hasEastLink && hasSouthLink)
                    return 270;
            }

            return 0;
        }
        
        private void GetLinkDirections(out bool hasNorthLink, out bool hasSouthLink, out bool hasWestLink,
            out bool hasEastLink, out bool hasUpLink, out bool hasDownLink)
        {
            hasNorthLink = false;
            hasSouthLink = false;
            hasWestLink = false; 
            hasEastLink = false;
            hasUpLink = false;
            hasDownLink = false;
            
            foreach (var linkedCell in _links)
            {
                if (linkedCell == North)
                    hasNorthLink = true;
                else if (linkedCell == South)
                    hasSouthLink = true;
                else if (linkedCell == West)
                    hasWestLink = true;
                else if (linkedCell == East)
                    hasEastLink = true;
                else if (linkedCell == Up)
                    hasUpLink = true;
                else if (linkedCell == Down)
                    hasDownLink = true;
            }
        } 
    }
}
