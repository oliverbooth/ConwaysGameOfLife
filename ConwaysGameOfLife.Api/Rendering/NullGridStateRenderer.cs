﻿namespace ConwaysGameOfLife.Api.Rendering
{
    /// <summary>
    ///     Represents a renderer which does nothing.
    /// </summary>
    public sealed class NullGridStateRenderer : GridStateRenderer
    {
        /// <inheritdoc />
        public override void Render(GridState grid)
        {
            // do nothing
        }
    }
}