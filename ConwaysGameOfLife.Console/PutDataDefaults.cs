using System;

namespace ConwaysGameOfLife.Console
{
    /// <summary>
    ///     Defines common and useful <see cref="PutData" /> values.
    /// </summary>
    public static class PutDataDefaults
    {
        /// <summary>
        ///     Black-on-black rendering.
        /// </summary>
        public static readonly PutData BlackOnBlack = new()
            { Background = ConsoleColor.Black, Foreground = ConsoleColor.Black };

        /// <summary>
        ///     Black-on-white rendering.
        /// </summary>
        public static readonly PutData BlackOnWhite = new()
            { Background = ConsoleColor.White, Foreground = ConsoleColor.Black };

        /// <summary>
        ///     White-on-white rendering.
        /// </summary>
        public static readonly PutData WhiteOnWhite = new()
            { Background = ConsoleColor.White, Foreground = ConsoleColor.White };

        /// <summary>
        ///     White-on-black rendering.
        /// </summary>
        public static readonly PutData WhiteOnBlack = new()
            { Background = ConsoleColor.Black, Foreground = ConsoleColor.White };
    }
}
