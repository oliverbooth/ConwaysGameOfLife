using System;
using System.Drawing;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Represents an axis aligned bounding box.
    /// </summary>
    public readonly struct Bounds : IEquatable<Bounds>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Bounds" /> struct.
        /// </summary>
        /// <param name="min">The minimum point.</param>
        /// <param name="max">The maximum point.</param>
        public Bounds(Point min, Point max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        ///     Gets the minimum point of this <see cref="Bounds" /> instance.
        /// </summary>
        /// <value>A <see cref="Point" /> representing the minimum point.</value>
        public Point Min { get; }

        /// <summary>
        ///     Gets the maximum point of this <see cref="Bounds" /> instance.
        /// </summary>
        /// <value>A <see cref="Point" /> representing the maximum point.</value>
        public Point Max { get; }

        /// <summary>
        ///     Gets the size of this <see cref="Bounds" /> instance.
        /// </summary>
        /// <value>A <see cref="System.Drawing.Size "/> representing the size of the bounds.</value>
        public Size Size => new(Max.X - Min.X, Max.Y - Min.Y);

        /// <summary>
        ///     Indicates whether two <see cref="Bounds" /> instances are considered equal.
        /// </summary>
        /// <param name="left">The first bounds.</param>
        /// <param name="right">The second bounds.</param>
        /// <returns><see langword="true" /> if both bounds are considered equal, or <see langword="false" /> otherwise.</returns>
        public static bool operator ==(Bounds left, Bounds right) => left.Equals(right);

        /// <summary>
        ///     Indicates whether two <see cref="Bounds" /> instances are considered unequal.
        /// </summary>
        /// <param name="left">The first bounds.</param>
        /// <param name="right">The second bounds.</param>
        /// <returns>
        ///     <see langword="true" /> if both bounds are considered unequal, or <see langword="false" /> otherwise.
        /// </returns>
        public static bool operator !=(Bounds left, Bounds right) => !left.Equals(right);

        /// <inheritdoc />
        public bool Equals(Bounds other) => Min.Equals(other.Min) && Max.Equals(other.Max);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Bounds other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"{{ Min={Min}, Max={Max}, Size={Size} }}";
    }
}
