namespace ConwaysGameOfLife.Api.Rendering
{
    /// <summary>
    ///     Represents a renderer which does nothing.
    /// </summary>
    public sealed class NullRenderer : IRenderer
    {
        /// <inheritdoc />
        public void Clear()
        {
            // do nothing
        }

        /// <inheritdoc />
        public void Render(in GridState grid, in GridStateDiff diff)
        {
            // do nothing
        }
    }
}
