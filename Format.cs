// This function is formatting an existing .md file
using System.Text.RegularExpressions;

public class Formatter
{

  internal static string formattedContent = "";

  internal static int[] counters = new int[64];

  public static void Format (string args)
  {
    string line;
    string previousLine;
    string inputfile;
    if (args.Equals(""))
      inputfile = "input.md";
    else
      inputfile = args;    

    string outputfile = "output.md";
    Console.WriteLine("Formatting \"" + inputfile + "\" to \"" + outputfile + "\" in markdown format.");

    // Replace Tabs by Spaces
	StreamReader streamReader = new StreamReader(inputfile);
	string formattedContent = "";
	int lineCount = 1;

	while ((line = streamReader.ReadLine()) != null) // read the file line by line.
	{
	lineCount++;
	formattedContent = formattedContent + line.Replace("\t", "  ") + "\n";
	}
	streamReader.Close();

    // Fix Indentation
    string originalContent = formattedContent;
    formattedContent = "";
    StringReader stringReader = new StringReader(originalContent); // foreach (string s in formattedContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) alternative
    for (int i = 0; i <= lineCount; i++) // read the string line by line
    {
      if ((line = stringReader.ReadLine()) == null)
        break;

      string trimmedLine = line.TrimStart();
      int spacesCount = line.Length - trimmedLine.Length;
      Match match1 = Regex.Match(trimmedLine, @"^[0-9]*\)", RegexOptions.IgnoreCase);
      Match match2 = Regex.Match(trimmedLine, @"^[0-9]*\.", RegexOptions.IgnoreCase);      
      if (spacesCount % 2 > 0 &&
           (trimmedLine.StartsWith("-") ||
            trimmedLine.StartsWith("+") ||
            trimmedLine.StartsWith("*")))
      {
        formattedContent = formattedContent + line.Insert(0, " ") + "\n";
        //Console.WriteLine(line); // for debug purposes
      }
      else if (spacesCount % 4 > 0 && (match1.Success || match2.Success))
      {
        for (int j = 0; j < 4 - spacesCount % 4; j++)
          line = line.Insert(0, " ");
        formattedContent = formattedContent + line + "\n";
      }
      else
      {
        formattedContent = formattedContent + line + "\n";
      }
    }
    stringReader.Close();

    // Add missing space if needed
    originalContent = formattedContent;
    formattedContent = "";
    stringReader = new StringReader(originalContent); // foreach (string s in formattedContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) alternative
    for (int i = 0; i <= lineCount; i++)
    {
      if ((line = stringReader.ReadLine()) == null)
        break;

      string trimmedLine = line.TrimStart();
      Match match1 = Regex.Match(trimmedLine, @"^[0-9]*\)", RegexOptions.IgnoreCase);
      Match match2 = Regex.Match(trimmedLine, @"^[0-9]*\.", RegexOptions.IgnoreCase);
      Match match3 = Regex.Match(trimmedLine, @"^[0-9]*\) ", RegexOptions.IgnoreCase);
      Match match4 = Regex.Match(trimmedLine, @"^[0-9]*\. ", RegexOptions.IgnoreCase);
      if (trimmedLine.StartsWith("-") && !trimmedLine.StartsWith("- "))
        formattedContent = formattedContent + line.Insert(line.IndexOf("-")+1, " ") + "\n";
      else if (trimmedLine.StartsWith("+") && !trimmedLine.StartsWith("+ "))
        formattedContent = formattedContent + line.Insert(line.IndexOf("+")+1, " ") + "\n";
      else if (trimmedLine.StartsWith("*") && !trimmedLine.StartsWith("* "))
        formattedContent = formattedContent + line.Insert(line.IndexOf("*")+1, " ") + "\n";
      else if (match1.Success && !match3.Success)
      {
        //Console.WriteLine(line); // for debug purposes
        int j = line.IndexOf(')');
        line = line.Insert(j+1, " ");
        formattedContent = formattedContent + line + "\n";
      }  
      else if (match2.Success && !match4.Success)
      {
        int j = line.IndexOf('.');
        line = line.Insert(j+1, " ");
        formattedContent = formattedContent + line + "\n";        
      }
      else
      {
        formattedContent = formattedContent + line + "\n";
      }
    }
    stringReader.Close();

    // Fix Numbering
    originalContent = formattedContent;
    stringReader = new StringReader(originalContent); // foreach (string s in formattedContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)) alternative
    CorrectNumbers (stringReader);
    stringReader.Close();

    // Write to file
    StreamWriter streamWriter = new StreamWriter(outputfile);
    streamWriter.Write(Formatter.formattedContent);
    streamWriter.Close();
  }

  private static void CorrectNumbers(StringReader stringReader)
  {
    string line = "";
    int indentLevel = 0;
    int previousIndentLevel = 0;
    char[] numericCharacters = {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
    while ((line = stringReader.ReadLine()) != null) 
    {
      string trimmedLine = line.TrimStart();
      Match match1 = Regex.Match(trimmedLine, @"^[0-9]*\) ", RegexOptions.IgnoreCase);
      Match match2 = Regex.Match(trimmedLine, @"^[0-9]*\. ", RegexOptions.IgnoreCase);

      if (match1.Success)
      {
        indentLevel = line.IndexOf(')');
        int indexOfNumber = line.IndexOfAny(numericCharacters);
        if (indentLevel >= previousIndentLevel)
        {
          counters[indentLevel/4]++;
          int number = counters[indentLevel/4];
          line = line.Remove(indexOfNumber, indentLevel - indexOfNumber);
          line = line.Insert(indentLevel-1, number.ToString());
          formattedContent = formattedContent + line + "\n";
        }
        else if (indentLevel < previousIndentLevel)
        {
          counters[previousIndentLevel/4] = 0;
          counters[indentLevel/4]++;
          int number = counters[indentLevel/4];
          line = line.Remove(indexOfNumber, indentLevel - indexOfNumber);
          line = line.Insert(indentLevel-1, number.ToString());              
          formattedContent = formattedContent + line + "\n";
        }
        //Console.WriteLine(line); // for debug purposes
      }
      else if (match2.Success)
      {
        indentLevel = line.IndexOf('.');
        int indexOfNumber = line.IndexOfAny(numericCharacters);        
        if (indentLevel >= previousIndentLevel)
        {
          counters[indentLevel/4]++;
          int number = counters[indentLevel/4];
          line = line.Remove(indexOfNumber, indentLevel - indexOfNumber);
          line = line.Insert(indentLevel-1, number.ToString());
          formattedContent = formattedContent + line + "\n";
        }
        else if (indentLevel < previousIndentLevel)
        {
          counters[previousIndentLevel/4] = 0;
          counters[indentLevel/4]++;
          int number = counters[indentLevel/4];
          line = line.Remove(indexOfNumber, indentLevel - indexOfNumber);
          line = line.Insert(indentLevel-1, number.ToString());              
          formattedContent = formattedContent + line + "\n";
        }
      }
      else
      {
        for (int i = 0; i < counters.Length; i++) { counters[i] = 0;}
        formattedContent = formattedContent + line + "\n";
      }
      previousIndentLevel = indentLevel;
    }
  }
}