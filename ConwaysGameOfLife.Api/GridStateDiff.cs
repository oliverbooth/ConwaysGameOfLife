using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Represents a diff of <see cref="GridState" />.
    /// </summary>
    public readonly struct GridStateDiff
    {
        /// <summary>
        ///     Represents the null grid state diff, in that
        /// </summary>
        public static readonly GridStateDiff Null = new(ArraySegment<Cell>.Empty);

        private readonly List<Cell>? _changedCells;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridStateDiff" /> struct by accepting a collection of
        ///     <see cref="Cell" /> values, containing locations and their state changes.
        /// </summary>
        /// <param name="cells"></param>
        public GridStateDiff(IEnumerable<Cell> cells)
        {
            _changedCells = new List<Cell>(cells);
        }

        /// <summary>
        ///     Gets the cells which changed states in this diff.
        /// </summary>
        /// <value>
        ///     An <see cref="IReadOnlyCollection{T}" /> of <see cref="Cell" /> which indicates cell whose states that changed.
        /// </value>
        [NotNull]
        public IReadOnlyCollection<Cell> ChangedCells => _changedCells?.ToArray() ?? ArraySegment<Cell>.Empty;
    }
}
