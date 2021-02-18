using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Represents the state of a grid at any one time.
    /// </summary>
    public struct GridState : IEquatable<GridState>
    {
        /// <summary>
        ///     Represents an empty grid state, with every cell being dead.
        /// </summary>
        public static readonly GridState Empty = new();

        private List<Point>? _livingCells;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridState" /> struct by accepting a collection of living cell
        ///     locations.
        /// </summary>
        /// <param name="aliveCells">The locations of all living cells.</param>
        public GridState(IEnumerable<Point> aliveCells) : this()
        {
            _livingCells = new List<Point>(aliveCells);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridState" /> struct by accepting a collection of cells.
        /// </summary>
        /// <param name="cells">The cells to consume.</param>
        public GridState(IEnumerable<Cell> cells) : this()
        {
            var livingCells = cells.Where(c => c.State == CellState.Alive).Select(c => c.Location);
            _livingCells = new List<Point>(livingCells);
        }

        /// <summary>
        ///     Gets or sets the state of a specified cell.
        /// </summary>
        /// <param name="cell">The cell whose state to get or set.</param>
        /// <value>A <see cref="CellState" /> representing the state of the cell.</value>
        public CellState this[Point cell]
        {
            get => GetStateAt(cell);
            set => SetStateAt(cell, value);
        }

        /// <summary>
        ///     Gets the bounds of the current state.
        /// </summary>
        /// <value>A <see cref="Api.Bounds" /> instance representing the bounds of this grid state.</value>
        public Bounds Bounds
        {
            get
            {
                var minPoint = Point.Empty;
                var maxPoint = Point.Empty;

                foreach (var cell in LivingCells.Where(c => c.IsAlive))
                {
                    minPoint = minPoint.Min(cell.Location);
                    maxPoint = maxPoint.Max(cell.Location);
                }

                return new Bounds(minPoint, maxPoint);
            }
        }

        /// <summary>
        ///     Gets a collection of locations where cells are alive.
        /// </summary>
        /// <value>An <see cref="IReadOnlyCollection{T}" /> of <see cref="Point" /> values.</value>
        public IReadOnlyCollection<Cell> LivingCells => (_livingCells ?? new List<Point>()).Select(p => new Cell(p)).ToArray();

        /// <summary>
        ///     Indicates whether two <see cref="GridState" /> instances are considered equal.
        /// </summary>
        /// <param name="left">The first state.</param>
        /// <param name="right">The second state.</param>
        /// <returns><see langword="true" /> if both states are considered equal, or <see langword="false" /> otherwise.</returns>
        public static bool operator ==(GridState left, GridState right) => left.Equals(right);

        /// <summary>
        ///     Indicates whether two <see cref="GridState" /> instances are considered unequal.
        /// </summary>
        /// <param name="left">The first state.</param>
        /// <param name="right">The second state.</param>
        /// <returns>
        ///     <see langword="true" /> if both states are considered unequal, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator !=(GridState left, GridState right) => !left.Equals(right);

        /// <summary>
        ///     Generates a <see cref="GridState" /> by applying a diff upon a previous state.
        /// </summary>
        /// <param name="previousState">The previous state on which to act.</param>
        /// <param name="diff">The diff to apply.</param>
        /// <returns>
        ///     A new <see cref="GridState" /> which applies <paramref name="diff" /> upon <paramref name="previousState"/>.
        /// </returns>
        public static GridState FromDiff(GridState previousState, GridStateDiff diff)
        {
            var livingCells = new List<Cell>(previousState.LivingCells);

            foreach (var cell in diff.ChangedCells)
            {
                switch (cell.State)
                {
                    case CellState.Alive when !livingCells.Contains(cell):
                        livingCells.Add(cell);
                        break;

                    case CellState.Dead when livingCells.Contains(cell):
                        livingCells.Remove(cell);
                        break;
                }
            }

            return new GridState(livingCells);
        }

        /// <summary>
        ///     Gets all dead neighbour cells for a particular cell.
        /// </summary>
        /// <param name="cell">The cell whose dead neighbours to get.</param>
        /// <returns>A collection of all dead neighbours to <paramref name="cell" />.</returns>
        /// <remarks>
        ///     All cells returned by this method will have a <see cref="Cell.State" /> of <see cref="CellState.Dead" />.
        /// </remarks>
        public IReadOnlyCollection<Cell> GetDeadNeighbours(Cell cell)
        {
            return GetNeighbours(cell).Where(n => !n.IsAlive).ToArray();
        }

        /// <summary>
        ///     Gets all living neighbour cells for a particular cell.
        /// </summary>
        /// <param name="cell">The cell whose living neighbours to get.</param>
        /// <returns>A collection of all living neighbours to <paramref name="cell" />.</returns>
        /// <remarks>
        ///     All cells returned by this method will have a <see cref="Cell.State" /> of <see cref="CellState.Alive" />.
        /// </remarks>
        public IReadOnlyCollection<Cell> GetLivingNeighbours(Cell cell)
        {
            return GetNeighbours(cell).Where(n => n.IsAlive).ToArray();
        }

        /// <summary>
        ///     Gets all adjacent neighbour cells for a particular cell.
        /// </summary>
        /// <param name="cell">The cell whose neighbour to get.</param>
        /// <returns>A collection of all adjacent neighbours to <paramref name="cell" />.</returns>
        /// <remarks>This method does not ignore dead cells. Both living and dead neighbours are captured.</remarks>
        public IReadOnlyCollection<Cell> GetNeighbours(Cell cell)
        {
            var directions = new[]
            {
                cell.Location + Direction.Up,
                cell.Location + Direction.Down,
                cell.Location + Direction.Right,
                cell.Location + Direction.Left,
                cell.Location + Direction.Up + Direction.Right,
                cell.Location + Direction.Up + Direction.Left,
                cell.Location + Direction.Down + Direction.Right,
                cell.Location + Direction.Down + Direction.Left
            };

            var livingCells = _livingCells ?? new List<Point>();
            return directions.Select(direction =>
                                  new Cell(direction, livingCells.Contains(direction) ? CellState.Alive : CellState.Dead))
                             .ToArray();
        }

        /// <summary>
        ///     Sets the state of a specified cell.
        /// </summary>
        /// <param name="cell">The cell whose state to change.</param>
        /// <param name="state">Optional. The new state of the cell. Defaults to <see cref="CellState.Alive" />.</param>
        public void SetStateAt(Point cell, CellState state)
        {
            _livingCells ??= new List<Point>();
            var isAlive = _livingCells.Contains(cell);

            if (!isAlive && state == CellState.Alive)
                _livingCells.Add(cell);
            else if (state == CellState.Dead)
                _livingCells.Remove(cell);
        }

        /// <summary>
        ///     Gets the state of a specified cell.
        /// </summary>
        /// <param name="cell">The cell whose state to get.</param>
        /// <returns><see cref="CellState.Alive" /> if the cell is alive, or <see cref="CellState.Dead" /> otherwise.</returns>
        public CellState GetStateAt(Point cell)
        {
            if (_livingCells is null)
                return CellState.Dead;

            return _livingCells.Contains(cell) ? CellState.Alive : CellState.Dead;
        }

        /// <inheritdoc />
        public bool Equals(GridState other)
        {
            return LivingCells.All(s => other.LivingCells.Contains(s))
                && other.LivingCells.All(s => other.LivingCells.Contains(s));
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is GridState other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => (_livingCells != null ? _livingCells.GetHashCode() : 0);
    }
}
