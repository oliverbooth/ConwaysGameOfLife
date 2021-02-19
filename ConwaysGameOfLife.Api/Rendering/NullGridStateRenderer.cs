namespace ConwaysGameOfLife.Api.Rendering
{
    /// <summary>
    ///     Represents a renderer which does nothing.
    /// </summary>
    public sealed class NullGridStateRenderer : GridStateRenderer
    {
        /// <inheritdoc />
        public override void Render(in GridState grid, in GridStateDiff diff)
        {
            // do nothing
        }
    }
}
