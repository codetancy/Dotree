using System.IO;
using Dotree;

var currentDirectory = Directory.GetCurrentDirectory();
var tree = FileTree.Create(currentDirectory);
tree.Display();
