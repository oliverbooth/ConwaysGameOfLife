using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

namespace ConwaysGameOfLife.Api.Serialization
{
    /// <summary>
    ///     Represents a <see cref="GridStateSerializer" /> which serializes a state using binary.
    /// </summary>
    public class BinaryGridStateSerializer : GridStateSerializer
    {
        /// <inheritdoc />
        public override GridState Read(Stream inputStream)
        {
            using var reader = new BinaryReader(inputStream);
            if (reader.Read7BitEncodedInt() != 0xC601)
                throw new SerializationException("File is not a binary grid state.");

            var count = reader.Read7BitEncodedInt();
            var cells = new List<Cell>();

            for (int i = 0; i < count; i++)
            {
                var x = reader.Read7BitEncodedInt();
                var y = reader.Read7BitEncodedInt();

                cells.Add(new Cell(new Point(x, y)));
            }

            return new GridState(cells);
        }

        /// <inheritdoc />
        public override void Write(Stream outputStream, GridState state)
        {
            using var writer = new BinaryWriter(outputStream);
            writer.Write7BitEncodedInt(0xC601);

            var cells = state.LivingCells;
            writer.Write7BitEncodedInt(cells.Count);

            foreach (var cell in cells)
            {
                writer.Write7BitEncodedInt(cell.Location.X);
                writer.Write7BitEncodedInt(cell.Location.Y);
            }
        }
    }
}
