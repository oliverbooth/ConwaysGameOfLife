using System;
using System.Drawing;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Represents a cell on a grid.
    /// </summary>
    public struct Cell : IEquatable<Cell>, IEquatable<Point>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell" /> struct.
        /// </summary>
        /// <param name="location">The location of the cell.</param>
        /// <param name="state">Optional. The state of the cell. Defaults to <see cref="CellState.Alive" />.</param>
        public Cell(Point location, CellState state = CellState.Alive)
        {
            if (!Enum.IsDefined(typeof(CellState), state))
                throw new ArgumentOutOfRangeException(nameof(state), $"Cell can only be {CellState.Alive} or {CellState.Dead}");

            Location = location;
            State = state;
        }

        /// <summary>
        ///     Gets a value indicating whether this cell is alive.
        /// </summary>
        /// <value><see langword="true" /> if the cell is alive, or <see langword="false" /> otherwise.</value>
        public bool IsAlive => State == CellState.Alive;

        /// <summary>
        ///     Gets the location of this cell.
        /// </summary>
        /// <value>A <see cref="Point" /> representing the location of the cell.</value>
        public Point Location { get; }

        /// <summary>
        ///     Gets or sets the state of this cell.
        /// </summary>
        /// <value>A <see cref="CellState" /> representing the state of the cell.</value>
        public CellState State { get; set; }

        /// <summary>
        ///     Indicates whether two <see cref="Cell" /> instances are considered equal.
        /// </summary>
        /// <param name="left">The first state.</param>
        /// <param name="right">The second state.</param>
        /// <returns><see langword="true" /> if both cells are considered equal, or <see langword="false" /> otherwise.</returns>
        public static bool operator ==(Cell left, Cell right) => left.Equals(right);

        /// <summary>
        ///     Indicates whether two <see cref="Cell" /> instances are considered unequal.
        /// </summary>
        /// <param name="left">The first state.</param>
        /// <param name="right">The second state.</param>
        /// <returns><see langword="true" /> if both cells are considered unequal, or <see langword="false" /> otherwise.</returns>
        public static bool operator !=(Cell left, Cell right) => !left.Equals(right);

        /// <summary>
        ///     Indicates whether the location of a <see cref="Cell" /> is equal to a <see cref="Point" />.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="location">The location.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Location" /> of <paramref name="cell" /> is equal to
        ///     <paramref name="location" />, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator ==(Cell cell, Point location) => cell.Equals(location);

        /// <summary>
        ///     Indicates whether the location of a <see cref="Cell" /> is not equal to a <see cref="Point" />.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="location">The location.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Location" /> of <paramref name="cell" /> is not equal to
        ///     <paramref name="location" />, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator !=(Cell cell, Point location) => !cell.Equals(location);

        /// <summary>
        ///     Indicates whether the location of a <see cref="Cell" /> is equal to a <see cref="Point" />.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Location" /> of <paramref name="cell" /> is equal to
        ///     <paramref name="location" />, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator ==(Point location, Cell cell) => cell.Equals(location);

        /// <summary>
        ///     Indicates whether the location of a <see cref="Cell" /> is not equal to a <see cref="Point" />.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="cell">The cell.</param>
        /// <returns>
        ///     <see langword="true" /> if the <see cref="Location" /> of <paramref name="cell" /> is not equal to
        ///     <paramref name="location" />, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator !=(Point location, Cell cell) => !cell.Equals(location);

        /// <inheritdoc />
        public bool Equals(Cell other) => Location.Equals(other.Location);

        /// <inheritdoc />
        public bool Equals(Point point) => Location.Equals(point);

        /// <inheritdoc />
        public override bool Equals(object? obj) =>
            obj switch
            {
                Cell cell => Equals(cell),
                Point point => Equals(point),
                _ => false
            };

        /// <inheritdoc />
        public override int GetHashCode() => Location.GetHashCode();
    }
}
