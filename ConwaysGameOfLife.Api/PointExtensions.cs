using System;
using System.Drawing;

namespace ConwaysGameOfLife.Api
{
    internal static class PointExtensions
    {
        public static Point Max(this Point point, Point other) => new(Math.Max(point.X, other.X), Math.Max(point.X, point.Y));
        public static Point Min(this Point point, Point other) => new(Math.Min(point.X, other.X), Math.Min(point.X, point.Y));
    }
}
