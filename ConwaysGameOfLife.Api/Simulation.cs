using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConwaysGameOfLife.Api.GameRules;
using ConwaysGameOfLife.Api.Rendering;
using ConwaysGameOfLife.Api.Serialization;

namespace ConwaysGameOfLife.Api
{
    /// <summary>
    ///     Represents an isolated simulation of Conway's Game of Life.
    /// </summary>
    public sealed class Simulation
    {
        private IRenderer _renderer = new NullRenderer();
        private TickRule _tickRule = new ClassicTickRule();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Simulation" /> class by initializing all cells as dead.
        /// </summary>
        public Simulation() : this(GridState.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Simulation" /> class by accepting an initial state of the grid.
        /// </summary>
        /// <param name="initialState">The initial state of the grid.</param>
        public Simulation(GridState initialState)
        {
            InitialState = initialState;
        }

        /// <summary>
        ///     Gets the current state of the grid.
        /// </summary>
        /// <value>A <see cref="GridState" /> representing the current state of the grid.</value>
        public GridState CurrentState { get; set; }

        /// <summary>
        ///     Gets the current generation of this simulation.
        /// </summary>
        /// <value>
        ///     Generations are 0-based. This value represents how many ticks have been performed since the initial state.
        /// </value>
        public int Generation { get; set; }

        /// <summary>
        ///     Gets the initial state of this simulation.
        /// </summary>
        /// <value>A <see cref="GridState" /> representing the initial state of the grid.</value>
        public GridState InitialState { get; }

        /// <summary>
        ///     Gets or sets the grid state renderer.
        /// </summary>
        /// <value>The renderer.</value>
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition", Justification = "Assigned value may be null")]
        public IRenderer Renderer
        {
            get => _renderer;
            set => _renderer = value ?? new NullRenderer();
        }

        /// <summary>
        ///     Gets or sets the tick rate of the simulation.
        /// </summary>
        /// <value>The tick rate of the simulation.</value>
        public double TickRate { get; set; } = 1.0;

        /// <summary>
        ///     Gets or sets the tick rule for this
        /// </summary>
        /// <value>The tick rule.</value>
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition", Justification = "Assigned value may be null")]
        public TickRule TickRule
        {
            get => _tickRule;
            set => _tickRule = value ?? new NullTickRule();
        }

        /// <summary>
        ///     Gets the state of the grid at a given generation.
        /// </summary>
        /// <param name="generation">The generations to iterate since the initial state of the grid</param>
        /// <returns>
        ///     A <see cref="GridState" /> representing the state of the grid after a number of generations equal to
        ///     <paramref name="generation" />.
        /// </returns>
        public GridState GetStateAtGeneration(int generation)
        {
            GridState state = InitialState;

            for (var g = 0; g < generation; g++)
                state = GridState.FromDiff(state, _tickRule.Tick(in state));

            return state;
        }

        /// <summary>
        ///     Runs this simulation, optionally for a specified number of generations.
        /// </summary>
        /// <param name="generations">The number of generations for which to run.</param>
        /// <param name="cancellationToken">The cancellation token for the simulation.</param>
        public async Task RunAsync(int generations = -1, CancellationToken cancellationToken = default, bool render = true)
        {
            CurrentState = InitialState;

            for (int generation = Generation; generations == -1 || generation < generations; generation++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Generation = generation;

                GridState currentState = CurrentState;
                GridStateDiff diff = _tickRule.Tick(in currentState);
                CurrentState = GridState.FromDiff(currentState, diff);

                if (render)
                    Renderer.Render(CurrentState, diff);

                await Task.Delay(TimeSpan.FromSeconds(1.0 / TickRate), cancellationToken);
            }
        }

        /// <summary>
        ///     Serializes the current state of the simulation to the specified file, using an
        ///     <see cref="AsciiGridStateSerializer" />.
        /// </summary>
        /// <param name="file">The file which should be written to.</param>
        /// <param name="serializationMode">
        ///     Optional. The serialization mode. Defaults to <see cref="SerializationMode.Ascii" />.
        /// </param>
        /// <exception cref="NotSupportedException">Binary serialization is not yet supported</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="serializationMode" /> is invalid.</exception>
        public void Save(string file, SerializationMode serializationMode = SerializationMode.Ascii)
        {
            using FileStream? stream = File.Create(file);
            Save(stream, serializationMode switch
            {
                SerializationMode.Ascii => new AsciiGridStateSerializer(),
                SerializationMode.Binary => new BinaryGridStateSerializer(),
                _ => throw new ArgumentOutOfRangeException(nameof(serializationMode))
            });
        }

        /// <summary>
        ///     Serializes the current state of the simulation to the specified stream, using a specified serializer.
        /// </summary>
        /// <param name="stream">The stream which should be written to.</param>
        /// <param name="serializer">The serializer responsible for writing.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serializer" /> is <see langword="null" />.</exception>
        public void Save(Stream stream, GridStateSerializer serializer)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));

            serializer.Write(stream, CurrentState);
        }
    }
}
