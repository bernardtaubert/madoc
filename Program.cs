// This program converts a text file into markdown when the 'c' flag is set as first command line argument. The second argument must be the name of the inputfile.
// If only one command line argument is given, then this program (re-)formats the given inputfile in a specific markdown format.
// check extensions

// Argument parsing
switch (args.Length)
{
  case 2:
    if (args[0].Contains("c"))
    {
      Console.WriteLine("Converting " + args[1] + " now!");
      Converter.Convert(args[1]);
    }
    return;
  case 1:
    Console.WriteLine("Formatting " + args[0] + " now!");
    Formatter.Format(args[0]);
    return;
  default:
    Console.WriteLine("Formatting input.txt now!");
    Formatter.Format("input.md");  
    return;    
}