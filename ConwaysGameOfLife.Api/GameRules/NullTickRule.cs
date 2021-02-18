namespace ConwaysGameOfLife.Api.GameRules
{
    /// <summary>
    ///     Represents a tick rule which does not change the state of the grid.
    /// </summary>
    public sealed class NullTickRule : TickRule
    {
        /// <inheritdoc />
        public override GridStateDiff Tick(ref GridState gridState)
        {
            return GridStateDiff.Null;
        }
    }
}
