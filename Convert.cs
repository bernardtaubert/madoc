// This function converts a text file into markdown
public class Converter
{
  public static void Convert (string args)
  {
    string line;
    string inputfile;
    if (args.Equals(""))
      inputfile = "input.txt";
    else
      inputfile = args;

    string outputfile = "";

    if (inputfile.EndsWith(".txt"))
    {
      Boolean inZutaten = true;
      Boolean inZubereitung = false;

      StreamReader streamReader = new StreamReader(inputfile);

      if ((line = streamReader.ReadLine()) != null)
      {
        if (outputfile.Length == 0)
        {
          outputfile = line; // we assume that the first line contains the title of the document
          line = line.Insert(0, "## ");
        }

        Console.WriteLine("Converting \"" + inputfile + "\" to \"" + outputfile + "\" in markdown format.");
        StreamWriter streamWriter = new StreamWriter(outputfile + ".md");
        streamWriter.WriteLine(line);
        streamWriter.WriteLine(""); // add 1 additional empty line here

        while ((line = streamReader.ReadLine()) != null) // read the file line by line.
        {
          if (inZutaten)
          {
            if (line.TrimStart().StartsWith("Zutaten"))
            {
              line = line.Insert(0, "### ");
            }
            else if (line.TrimStart().StartsWith("Zubereitung"))
            {
              inZutaten = false;
              streamWriter.WriteLine(""); // add 1 additional empty line here
              line = line.Insert(0, "### ");
            }
            else if (line.Trim().Equals(""))
            {
              continue; // skip empty lines
            }
            else
            {
              line = line.TrimStart().Insert(0, "- ");
            }
            streamWriter.WriteLine(line);
          }
          else
          {
            if (line.Trim().Equals(""))
            {
              continue; // skip empty lines
            }
            else if (line.TrimStart().StartsWith(""))
            {
              continue; // skip empty lines
            }
            else
            {
              line = line.Replace("", "");
              if (line.Trim().StartsWith("Arbeitszeit") || line.Trim().StartsWith("Koch-/Backzeit") || line.Trim().StartsWith("Gesamtzeit") || line.Trim().StartsWith("Kochzeit"))
              {
                streamWriter.Write("__" + line + "__, ");
              }
              else
              {
                if (!inZubereitung)
                {
                  inZubereitung = true;
                  streamWriter.WriteLine("  ");
                }
                else
                {
                  streamWriter.WriteLine(line);
                }
              }
            }
          }
        }
        streamWriter.Close();
      }
    }
    else if (inputfile.EndsWith(".md"))
    {
      // do something else
    }
  }
}  