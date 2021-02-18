using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ConwaysGameOfLife.Api.Serialization
{
    /// <summary>
    ///     Represents a <see cref="GridStateSerializer" /> which serializes a state using ASCII characters.
    /// </summary>
    /// <remarks>
    ///     Since a Conway's Game of Life grid is infinite in size, this renderer will use the upper-left bound of the grid as its
    ///     0, 0 point.
    /// </remarks>
    public class AsciiGridStateSerializer : GridStateSerializer
    {
        /// <summary>
        ///     Gets or sets the character with which to render dead cells.
        /// </summary>
        /// <value>The character with which to render dead cells.</value>
        public char DeadCellChar { get; set; } = ' ';

        /// <summary>
        ///     Gets or sets the character with which to render living cells.
        /// </summary>
        /// <value>The character with which to render living cells.</value>
        public char LivingCellChar { get; set; } = '#';

        /// <inheritdoc />
        public override GridState Read(Stream inputStream)
        {
            using var reader = new StreamReader(inputStream, Encoding.ASCII);
            var livingCells = new List<Point>();
            var y = 0;

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == LivingCellChar)
                        livingCells.Add(new Point(x, y));
                }

                y++;
            }

            return new GridState(livingCells);
        }

        /// <inheritdoc />
        public override void Write(Stream outputStream, GridState state)
        {
            using var writer = new StreamWriter(outputStream, Encoding.ASCII);
            var bounds = state.Bounds;

            for (int y = bounds.Min.Y; y <= bounds.Max.Y; y++)
            {
                for (int x = bounds.Min.X; x <= bounds.Max.X; x++)
                {
                    var point = new Point(x, y);
                    writer.Write(state[point] == CellState.Alive ? LivingCellChar : DeadCellChar);
                }

                writer.WriteLine();
            }
        }
    }
}
