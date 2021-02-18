using System;
using System.Drawing;

namespace ConwaysGameOfLife.Api
{
    internal static class PointExtensions
    {
        public static Point Max(this Point point, Point other) => new(Math.Max(point.X, other.X), Math.Max(point.Y, other.Y));
        public static Point Min(this Point point, Point other) => new(Math.Min(point.X, other.X), Math.Min(point.Y, other.Y));
    }
}
