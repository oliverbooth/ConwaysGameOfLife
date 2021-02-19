using System.Drawing;
using Console.Abstractions;
using ConwaysGameOfLife.Api;
using ConwaysGameOfLife.Api.Rendering;

namespace ConwaysGameOfLife.Console
{
    /// <summary>
    ///     Represents a renderer which renders a <see cref="GridState" /> to a target <see cref="IConsole" />.
    /// </summary>
    public sealed class ConsoleRenderer : IRenderer
    {
        private readonly IConsole _console;
        private readonly char _aliveChar;
        private readonly char _deadChar;
        private readonly PutData _alive;
        private readonly PutData _dead;
        private bool _oneShotFullRender;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConsoleRenderer" /> class.
        /// </summary>
        /// <param name="console">The console to which this renderer should render.</param>
        /// <param name="aliveChar">Optional. The character with which to render living cells. Defaults to a space.</param>
        /// <param name="deadChar">Optional. The character with which to render dead cells. Defaults to a space.</param>
        /// <param name="alive">
        ///     Optional. A <see cref="PutData" /> indicating how living cells should be rendered.
        ///     Defaults to <see cref="PutDataDefaults.WhiteOnWhite" />.
        /// </param>
        /// <param name="dead">
        ///     Optional. A <see cref="PutData" /> indicating how dead cells should be rendered.
        ///     Defaults to <see cref="PutDataDefaults.BlackOnBlack" />.
        /// </param>
        public ConsoleRenderer(
            IConsole console,
            char aliveChar = ' ',
            char deadChar = ' ',
            PutData? alive = null,
            PutData? dead = null)
        {
            _console = console;
            _aliveChar = aliveChar;
            _deadChar = deadChar;
            _alive = alive ?? PutDataDefaults.WhiteOnWhite;
            _dead = dead ?? PutDataDefaults.BlackOnBlack;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether a full render should happen.
        /// </summary>
        public bool FullRender { get; set; } = true;

        /// <summary>
        ///     Gets or sets the viewport offset.
        /// </summary>
        public Size ViewportOffset { get; set; } = Size.Empty;

        /// <inheritdoc />
        public void Clear()
        {
            _oneShotFullRender = true;
        }

        /// <inheritdoc />
        public void Render(in GridState grid, in GridStateDiff diff)
        {
            if (FullRender || _oneShotFullRender)
                _console.Clear(_dead);

            foreach (var cell in FullRender || _oneShotFullRender ? grid.LivingCells : diff.ChangedCells)
            {
                var location = cell.Location + ViewportOffset;

                if (location.X < 0 || location.X >= _console.Width || location.Y < 0 || location.Y >= _console.Height)
                    continue;

                _console.PutChar(cell.IsAlive ? _aliveChar : _deadChar,
                    (cell.IsAlive ? _alive : _dead) with { X = location.X, Y = location.Y });
            }

            _oneShotFullRender = false;
        }
    }
}
