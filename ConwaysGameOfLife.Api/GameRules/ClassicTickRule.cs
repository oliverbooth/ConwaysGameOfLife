using System.Collections.Generic;

namespace ConwaysGameOfLife.Api.GameRules
{
    /// <summary>
    ///     Represents a tick rule which adheres to the classic definitions for Conway's Game of Life.
    /// </summary>
    public class ClassicTickRule : TickRule
    {
        /// <inheritdoc />
        public override GridStateDiff Tick(in GridState gridState)
        {
            var modifiedCells = new List<Cell>();

            foreach (var livingCell in gridState.LivingCells)
            {
                if (IsOverpopulated(in gridState, livingCell) || IsUnderpopulated(in gridState, livingCell))
                    modifiedCells.Add(new Cell(livingCell.Location, CellState.Dead));

                foreach (var cell in gridState.GetNeighbours(livingCell))
                {
                    if (ShouldRepopulate(in gridState, cell))
                        modifiedCells.Add(new Cell(cell.Location));
                }
            }

            return new GridStateDiff(modifiedCells);
        }

        private static bool ShouldRepopulate(in GridState gridState, Cell cell) =>
            !cell.IsAlive && gridState.GetLivingNeighbours(cell).Count == 3;

        private static bool IsOverpopulated(in GridState gridState, Cell cell) =>
            gridState.GetLivingNeighbours(cell).Count > 3;

        private static bool IsUnderpopulated(in GridState gridState, Cell cell) =>
            gridState.GetLivingNeighbours(cell).Count < 2;
    }
}
