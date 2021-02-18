using System.Drawing;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Defines a set of directions, denoted with <see cref="Size" /> values.
    /// </summary>
    public static class Direction
    {
        /// <summary>
        ///     Up. Vector &lt;0, -1&gt;
        /// </summary>
        /// <value>
        ///     A <see cref="Size" /> with <see cref="Size.Width" /> as <c>0</c> an <see cref="Size.Height" /> as <c>-1</c>.
        /// </value>
        public static readonly Size Up = new(0, -1);

        /// <summary>
        ///     Down. Vector &lt;0, 1&gt;
        /// </summary>
        /// <value>
        ///     A <see cref="Size" /> with <see cref="Size.Width" /> as <c>0</c> an <see cref="Size.Height" /> as <c>1</c>.
        /// </value>
        public static readonly Size Down = new(0, 1);

        /// <summary>
        ///     Left. Vector &lt;-1, 0&gt;
        /// </summary>
        /// <value>
        ///     A <see cref="Size" /> with <see cref="Size.Width" /> as <c>-1</c> an <see cref="Size.Height" /> as <c>0</c>.
        /// </value>
        public static readonly Size Left = new(-1, 0);

        /// <summary>
        ///     Right. Vector &lt;1, 0&gt;
        /// </summary>
        /// <value>
        ///     A <see cref="Size" /> with <see cref="Size.Width" /> as <c>1</c> an <see cref="Size.Height" /> as <c>0</c>.
        /// </value>
        public static readonly Size Right = new(1, 0);
    }
}
