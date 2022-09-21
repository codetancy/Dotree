using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Dotree;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp<DotreeCommand>();
return app.Run(args);


internal sealed class DotreeCommand : Command<DotreeCommand.Settings>
{
    internal sealed class Settings : CommandSettings
    {
        [Description("Path to search. Defaults to current directory.")]
        [CommandArgument(0, "[searchPath]")]
        [DefaultValue(".")]
        public string SearchPath { get; init; }

        [Description("Levels of nested directory to display. Defaults to all.")]
        [CommandOption("-l|--levels")]
        public int Levels { get; init; }

        [Description("Regex expression for entries to exclude. Defaults to none.")]
        [CommandOption("-e|--exclude")]
        public string ExcludePattern { get; init; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        TryGetConfig(out var config);

        var searchPath = Path.Join(Directory.GetCurrentDirectory(), settings.SearchPath);
        var tree = FileTree.Create(searchPath);
        tree.Display(config);

        return 0;
    }

    private bool TryGetConfig(out TreeConfig config)
    {
        var pathToConfig = Environment.GetEnvironmentVariable("DOTREE_CONFIG");

        try
        {
            if (pathToConfig is null)
            {
                config = new TreeConfig();
            }
            else
            {
                var json = File.ReadAllText(pathToConfig);
                config = JsonSerializer.Deserialize<TreeConfig>(json, new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true
                });
            }

            return true;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
        }

        AnsiConsole.MarkupLine("[red]An error happened while reading config file. Using default configuration.[/]");
        config = new TreeConfig();
        return true;
    }
}