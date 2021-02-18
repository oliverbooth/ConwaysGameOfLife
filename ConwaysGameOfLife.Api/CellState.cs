using System.ComponentModel;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     An enumeration of cell states.
    /// </summary>
    public enum CellState
    {
        [Description("The cell is dead.")] Dead,

        [Description("The cell is alive.")] Alive
    }
}
