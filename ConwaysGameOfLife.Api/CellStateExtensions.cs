using System;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Extension methods for <see cref="CellState" />.
    /// </summary>
    public static class CellStateExtensions
    {
        /// <summary>
        ///     Returns the opposite cell state of a specified state.
        /// </summary>
        /// <param name="cellState">The cell state.</param>
        /// <returns>
        ///     <see cref="CellState.Alive" /> if <paramref name="cellState" /> is <see cref="CellState.Dead" />.
        ///     -or-
        ///     <see cref="CellState.Dead" /> if <paramref name="cellState" /> is <see cref="CellState.Alive" />.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="cellState" /> is neither <see cref="CellState.Alive" /> or <see cref="CellState.Dead" />.
        /// </exception>
        public static CellState Opposite(this CellState cellState) =>
            cellState switch
            {
                CellState.Alive => CellState.Dead,
                CellState.Dead => CellState.Alive,
                var _ => throw new ArgumentOutOfRangeException(nameof(cellState))
            };
    }
}
