﻿using System.Collections.Generic;
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
            if (reader.ReadInt32() != 0xC601)
                throw new SerializationException("File is not a binary grid state.");

            int count = reader.ReadInt32();
            var cells = new List<Cell>();

            for (var i = 0; i < count; i++)
            {
                int x = reader.ReadInt32();
                int y = reader.ReadInt32();

                cells.Add(new Cell(new Point(x, y)));
            }

            return new GridState(cells);
        }

        /// <inheritdoc />
        public override void Write(Stream outputStream, GridState state)
        {
            using var writer = new BinaryWriter(outputStream);
            writer.Write(0xC601);

            IReadOnlyCollection<Cell>? cells = state.LivingCells;
            writer.Write(cells.Count);

            foreach (Cell cell in cells)
            {
                writer.Write(cell.Location.X);
                writer.Write(cell.Location.Y);
            }
        }
    }
}
