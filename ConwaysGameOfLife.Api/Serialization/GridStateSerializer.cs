using System.IO;

namespace ConwaysGameOfLife.Api.Serialization
{
    /// <summary>
    ///     Represents a <see cref="GridState" /> serializer, which can both serialize and deserialize a <see cref="GridState" />
    ///     to/from a <see cref="Stream" />.
    /// </summary>
    public abstract class GridStateSerializer
    {
        /// <summary>
        ///     Reads a grid state from the specified stream.
        /// </summary>
        /// <param name="inputStream">The stream to read.</param>
        /// <returns>A <see cref="GridState" /> as read from the input stream.</returns>
        public abstract GridState Read(Stream inputStream);

        /// <summary>
        ///     Writes a grid state to the specified stream.
        /// </summary>
        /// <param name="outputStream">The stream which should be written to.</param>
        /// <param name="state">The grid state to write.</param>
        public abstract void Write(Stream outputStream, GridState state);
    }
}
