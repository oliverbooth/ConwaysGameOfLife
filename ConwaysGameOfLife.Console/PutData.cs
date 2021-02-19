using System;
using System.Drawing;
using Console.Abstractions;

namespace ConwaysGameOfLife.Console
{
    /// <summary>
    ///     Represents a wrapper for <see cref="PutCharData" /> so that <c>with</c> expressions can be used.
    /// </summary>
    public sealed record PutData
    {
        /// <summary>
        ///     Gets or sets the background color.
        /// </summary>
        /// <value>A <see cref="ConsoleColor" /> representing the background color.</value>
        public ConsoleColor Background { get; set; }

        /// <summary>
        ///     Gets or sets the foreground color.
        /// </summary>
        /// <value>A <see cref="ConsoleColor" /> representing the foreground color.</value>
        public ConsoleColor Foreground { get; set; }

        /// <summary>
        ///     Gets or sets the coordinates.
        /// </summary>
        /// <value>A <see cref="Point" /> representing the coordinates.</value>
        public Point Location
        {
            get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        ///     Gets or sets the X coordinate.
        /// </summary>
        /// <value>The X coordinate.</value>
        public int X { get; set; }

        /// <summary>
        ///     Gets or sets the Y coordinate.
        /// </summary>
        /// <value>The Y coordinate.</value>
        public int Y { get; set; }

        /// <summary>
        ///     Implicitly converts a <see cref="PutData" /> instance to a new instance of <see cref="PutCharData" />.
        /// </summary>
        /// <param name="data">The <see cref="PutData" /> object.</param>
        /// <returns>A <see cref="PutCharData" />.</returns>
        public static implicit operator PutCharData(PutData data) => new()
            { Background = data.Background, Foreground = data.Foreground, X = data.X, Y = data.Y };

        /// <summary>
        ///     Implicitly converts a <see cref="PutCharData" /> instance to a new instance of <see cref="PutData" />.
        /// </summary>
        /// <param name="data">The <see cref="PutCharData" /> object.</param>
        /// <returns>A <see cref="PutData" />.</returns>
        public static implicit operator PutData(PutCharData data) => new()
            { Background = data.Background, Foreground = data.Foreground, X = data.X, Y = data.Y };
    }
}
