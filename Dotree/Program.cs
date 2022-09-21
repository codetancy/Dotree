using System;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        var searchPath = Path.Join(Directory.GetCurrentDirectory(), settings.SearchPath);
        var tree = FileTree.Create(searchPath);
        tree.Display();

        return 0;
    }
}