# Conway's Game of Life
A scalable .NET 5 implementation of Conway's Game of Life written in C#.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/oliverbooth/ConwaysGameOfLife/dotnet)
![GitHub open issues](https://img.shields.io/github/issues/oliverbooth/ConwaysGameOfLife)
![GitHub last commit](https://img.shields.io/github/last-commit/oliverbooth/ConwaysGameOfLife)
![GitHub repo size](https://img.shields.io/github/repo-size/oliverbooth/ConwaysGameOfLife)

---

## Using the app
Start the app, optionally with the `--tick-rate #` flag to specify how quickly the simulation should run.

Initialize your grid by using the arrow keys to move the cursor, and hitting space to toggle a cell's living/dead state.

Once the grid is set, hit Enter to begin the simulation. While the simulation is running, you can use the arrow keys to pan around the grid so that you can view cells that are beyond the viewport.

---

## Creating a custom renderer
Inherit `ConwaysGameOfLife.Api.Rendering.GridStateRenderer` and implement the `Render` method.

See below for an example renderer which simply outputs how many cells are alive in the current state:
```cs
using System;
using ConwaysGameOfLife.Api;
using ConwaysGameOfLife.Api.Rendering;

public class LivingCellRenderer : GridStateRenderer
{
    /// <inheritdoc />
    public override void Render(in GridState grid)
    {
        Console.WriteLine($"There are {grid.LivingCells.Count} living cells for this state.");
    }
}
```

Refer to `ConwaysGameOfLife.Console.ConsoleGridStateRenderer` for further example.

---

## Creating custom tick rules
Simulation ticks work by a system of diffs. Instead of storing and passing the complete state of a grid, we only need to pass what has changed since the last tick.

To use, inherit `ConwaysGameOfLife.Api.GameRules.TickRule` and implement the `Tick` method.

See below for an example tick rule which randomly kills a living cell, regardless of its neighbours:
```cs
using ConwaysGameOfLife.Api;
using ConwaysGameOfLife.Api.GameRules;

public class KillRandomCellTickRule : TickRule
{
    private static readonly Random Random = new();

    /// <inheritdoc />
    public override GridStateDiff Tick(in GridState gridState)
    {
        // must copy to array to index-access
        var cells = gridState.LivingCells.ToArray();
        var randomCell = cells[Random.Next(cells.Length)];

        // pass in array of cells (location + state) to create diff
        return new GridStateDiff(new[]
        {
            new Cell(randomCell.Location, CellState.Dead)
        });
    }
}
```

Refer to `ConwaysGameOfLife.Api.GameRules.ClassicTickRule` to see how the classic rules are implemented.