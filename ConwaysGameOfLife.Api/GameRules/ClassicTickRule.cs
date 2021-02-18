using System.Collections.Generic;

namespace ConwaysGameOfLife.Api.GameRules
{
    /// <summary>
    ///     Represents a tick rule which adheres to the classic definitions for Conway's Game of Life.
    /// </summary>
    public class ClassicTickRule : TickRule
    {
        /// <inheritdoc />
        public override GridStateDiff Tick(ref GridState gridState)
        {
            var modifiedCells = new List<Cell>();

            foreach (var livingCell in gridState.LivingCells)
            {
                if (IsOverpopulated(ref gridState, livingCell) || IsUnderpopulated(ref gridState, livingCell))
                    modifiedCells.Add(new Cell(livingCell.Location, CellState.Dead));

                foreach (var cell in gridState.GetNeighbours(livingCell))
                {
                    if (ShouldRepopulate(ref gridState, cell))
                        modifiedCells.Add(new Cell(cell.Location));
                }
            }

            return new GridStateDiff(modifiedCells);
        }

        private static bool ShouldRepopulate(ref GridState gridState, Cell cell) =>
            !cell.IsAlive && gridState.GetLivingNeighbours(cell).Count == 3;

        private static bool IsOverpopulated(ref GridState gridState, Cell cell) =>
            gridState.GetLivingNeighbours(cell).Count > 3;

        private static bool IsUnderpopulated(ref GridState gridState, Cell cell) =>
            gridState.GetLivingNeighbours(cell).Count < 2;
    }
}
