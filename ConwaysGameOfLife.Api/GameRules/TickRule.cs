namespace ConwaysGameOfLife.Api.GameRules
{
    /// <summary>
    ///     Represents the base class for all tick rules.
    /// </summary>
    public abstract class TickRule
    {
        /// <summary>
        ///     Performs a tick on a specified grid state using the defined tick rule.
        /// </summary>
        /// <param name="gridState">The state of the grid on which to tick.</param>
        /// <returns>An instance of <see cref="GridStateDiff" /> indicating changes in cell state.</returns>
        public abstract GridStateDiff Tick(in GridState gridState);
    }
}
