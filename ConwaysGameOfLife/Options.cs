using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace ConwaysGameOfLife
{
    /// <summary>
    ///     Represents a class which contains deserialized command line options.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Dynamically instantiated")]
    internal sealed class Options
    {
        /// <summary>
        ///     Gets or sets the <c>generations</c> option.
        /// </summary>
        [Option('g', "generations", Required = false, Default = -1,
            HelpText = "The number of generations to simulate. Defaults to -1 (forever)")]
        public int Generations { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the <c>load</c> option.
        /// </summary>
        [Option('f', "load", Required = false, Default = null,
            HelpText = "The grid file to load")]
        public string? GridFile { get; set; } = null;

        /// <summary>
        ///     Gets or sets the <c>tick-rate</c> option.
        /// </summary>
        [Option('r', "tick-rate", Required = false, Default = 1.0,
            HelpText = "The rate at which the simulation should tick through generations. Defaults to 1.")]
        public double TickRate { get; set; } = 1.0;
    }
}
