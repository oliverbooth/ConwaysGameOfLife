﻿namespace ConwaysGameOfLife.Api.Rendering
{
    /// <summary>
    ///     Represents the base class for all <see cref="GridState" /> renderers.
    /// </summary>
    public abstract class GridStateRenderer
    {
        /// <summary>
        ///     Renders the grid state using the defined rendering method.
        /// </summary>
        /// <param name="grid">The grid state to render.</param>
        /// <param name="diff">The difference in state since the previous tick.</param>
        public abstract void Render(in GridState grid, in GridStateDiff diff);
    }
}
