using System;
using System.Text;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Parser
{
    string[] lines;


	public Parser(string inFile)
	{
        try
        {
            lines = System.IO.File.ReadAllLines(@inFile);
            Console.Write(lines[0]);
        }
        catch { Console.Write("Input file not found. Place input file in same directory as the executable."); }
	}


}
