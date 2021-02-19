using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Console.Abstractions;
using ConwaysGameOfLife.Api;
using ConwaysGameOfLife.Api.Serialization;
using ConwaysGameOfLife.Console;

namespace ConwaysGameOfLife
{
    /// <summary>
    ///     Represents an application implementation.
    /// </summary>
    internal class GameOfLifeApp
    {
        private static readonly Random Random = new();
        private readonly IConsole _console = new SystemConsole();
        private readonly ConsoleRenderer _renderer;
        private Options _options = new();
        private Simulation _simulation = null!;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameOfLifeApp" /> class.
        /// </summary>
        public GameOfLifeApp()
        {
            _renderer = new ConsoleRenderer(_console);
        }

        private Task CreateSimulationTask(CancellationTokenSource cancellationTokenSource) =>
            new(async () => await SimulationWorker(cancellationTokenSource), cancellationTokenSource.Token);

        private Task CreateUpdateTitleTask(CancellationTokenSource cancellationTokenSource) =>
            new(() => UpdateTitleWorker(cancellationTokenSource), cancellationTokenSource.Token);

        private GridState FetchInitialState()
        {
            _console.Clear(PutDataDefaults.BlackOnBlack);

            var gridState = new GridState();
            Point cursorPosition = Point.Empty;
            var done = false;

            while (!done)
            {
                ConsoleKey key = _console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Spacebar:
                        gridState.SetStateAt(cursorPosition, gridState[cursorPosition].Opposite());
                        break;

                    case ConsoleKey.UpArrow:
                        cursorPosition += Direction.Up;
                        break;

                    case ConsoleKey.DownArrow:
                        cursorPosition += Direction.Down;
                        break;

                    case ConsoleKey.LeftArrow:
                        cursorPosition += Direction.Left;
                        break;

                    case ConsoleKey.RightArrow:
                        cursorPosition += Direction.Right;
                        break;

                    case ConsoleKey.Enter:
                        done = true;
                        break;

                    case ConsoleKey.F: // random fill
                        for (var rx = 0; rx < _console.Width - 1; rx++)
                        for (var ry = 0; ry < _console.Height - 1; ry++)
                        {
                            var point = new Point(rx, ry);
                            gridState.SetStateAt(point, Random.NextDouble() > 0.5 ? CellState.Alive : CellState.Dead);
                        }

                        break;

                    case ConsoleKey.G: // partial fill
                        for (var i = 0; i < 50; i++)
                        {
                            int x = Random.Next(0, _console.Width - 1);
                            int y = Random.Next(0, _console.Height - 1);
                            gridState.SetStateAt(new Point(x, y), CellState.Alive);
                        }

                        break;

                    case ConsoleKey.R: // reset
                        for (var rx = 0; rx < _console.Width - 1; rx++)
                        for (var ry = 0; ry < _console.Height - 1; ry++)
                            gridState.SetStateAt(new Point(rx, ry), CellState.Dead);

                        break;
                }

                _renderer.Render(gridState, GridStateDiff.Null);

                int cursorX = Math.Clamp(cursorPosition.X, 0, _console.Width - 1);
                int cursorY = Math.Clamp(cursorPosition.Y, 0, _console.Height - 1);
                cursorPosition = new Point(cursorX, cursorY);
                System.Console.SetCursorPosition(cursorX, cursorY);
            }

            return gridState;
        }

        private void InputLoop(ref Task simulationTask, ref Task titleTask, ref CancellationTokenSource cancellationTokenSource)
        {
            var paused = false;
            while (true)
            {
                _renderer.FullRender = false;
                ConsoleKey key = _console.ReadKey(true).Key;
                var quit = false;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        quit = true;
                        break;

                    case ConsoleKey.Enter when paused:
                    case ConsoleKey.Spacebar when paused:
                        cancellationTokenSource = new CancellationTokenSource();
                        simulationTask = CreateSimulationTask(cancellationTokenSource);
                        titleTask = CreateUpdateTitleTask(cancellationTokenSource);

                        simulationTask.Start();
                        titleTask.Start();

                        paused = false;
                        break;

                    case ConsoleKey.Enter when !paused:
                    case ConsoleKey.Spacebar when !paused:
                        cancellationTokenSource.Cancel();
                        paused = true;
                        break;

                    case ConsoleKey.F5:
                        _simulation.Save($"grid-{DateTime.Now:yyyyMMddHHmmss}.dat", SerializationMode.Binary);
                        break;

                    case ConsoleKey.UpArrow:
                        _renderer.Clear();
                        _renderer.ViewportOffset -= Direction.Up;
                        break;

                    case ConsoleKey.DownArrow:
                        _renderer.Clear();
                        _renderer.ViewportOffset -= Direction.Down;
                        break;

                    case ConsoleKey.LeftArrow:
                        _renderer.Clear();
                        _renderer.ViewportOffset -= Direction.Left;
                        break;

                    case ConsoleKey.RightArrow:
                        _renderer.Clear();
                        _renderer.ViewportOffset -= Direction.Right;
                        break;
                }

                if (quit) break;
            }
        }

        /// <summary>
        ///     Runs the application.
        /// </summary>
        /// <param name="options">The options parsed from the command line.</param>
        public void Run(Options options)
        {
            System.Console.Title = "Conway's Game of Life";
            _options = options;

            GridState initialState;

            if (options.GridFile is { } file)
            {
                using FileStream? stream = File.OpenRead(file);
                try
                {
                    initialState = new BinaryGridStateSerializer().Read(stream);
                }
                catch (SerializationException)
                {
                    initialState = new AsciiGridStateSerializer().Read(stream);
                }

                new ConsoleRenderer(_console).Render(initialState, GridStateDiff.Null);

                while (_console.ReadKey(true).Key != ConsoleKey.Enter) ;
            }
            else
                initialState = FetchInitialState();

            _console.Clear(PutDataDefaults.BlackOnBlack);
            System.Console.CursorVisible = false;

            _simulation = new Simulation(initialState)
            {
                Renderer = _renderer,
                TickRate = options.TickRate
            };

            var cancellationTokenSource = new CancellationTokenSource();
            Task? simulationTask = CreateSimulationTask(cancellationTokenSource);
            Task? titleTask = CreateUpdateTitleTask(cancellationTokenSource);

            simulationTask.Start();
            titleTask.Start();

            InputLoop(ref simulationTask, ref titleTask, ref cancellationTokenSource);
        }

        private async Task SimulationWorker(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                _renderer.Clear();
                _renderer.Render(_simulation.InitialState, GridStateDiff.Null);

                await _simulation.RunAsync(_options.Generations, cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                // ignored
            }
        }

        private void UpdateTitleWorker(CancellationTokenSource cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
                System.Console.Title = $"Gen. #{_simulation.Generation}";

            System.Console.Title = "**PAUSED**";
        }
    }
}
