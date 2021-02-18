using CommandLine;
using ConwaysGameOfLife;

Parser.Default.ParseArguments<Options>(args).WithParsed(options => new GameOfLifeApp().Run(options));
