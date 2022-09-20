using Dotree;

var currentDirectory = "C:\\Users\\frank\\dev\\Dotree\\Dotree";// Directory.GetCurrentDirectory();
var tree = FileTree.Create(currentDirectory);
tree.Display();
// // string lightRed = "\x1b[1;31m";
// // Console.WriteLine($"{lightRed}Hello");
// Console.WriteLine("\u251C");
// Console.WriteLine("\u2500");
// Console.WriteLine("\u2502");
