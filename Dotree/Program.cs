using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        public string Levels { get; init; }

        [Description("Regex expression for entries to search. Defaults to all.")]
        [CommandOption("-p|--pattern")]
        public string SearchPattern { get; init; }

        public override ValidationResult Validate()
        {
            if (!string.IsNullOrEmpty(Levels))
            {
                if (!int.TryParse(Levels, out _))
                {
                    return ValidationResult.Error("--levels must be a non-negative integer");
                }
            }

            return ValidationResult.Success();
        }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        TryGetConfig(out var config);
        config = config with
        {
            MaxDepth = string.IsNullOrEmpty(settings.Levels) ? config.MaxDepth : int.Parse(settings.Levels),
            SearchPattern = string.IsNullOrEmpty(settings.SearchPattern) ? config.SearchPattern : settings.SearchPattern,
        };

        var searchPath = Path.Join(Directory.GetCurrentDirectory(), settings.SearchPath);
        var tree = new FileTree(searchPath, config: config);
        AnsiConsole.MarkupLine(tree.ToString());

        return 0;
    }

    private static bool TryGetConfig(out TreeConfig config)
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