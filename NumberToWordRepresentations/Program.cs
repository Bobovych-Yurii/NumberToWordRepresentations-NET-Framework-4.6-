using System;

namespace NumberToWordRepresentations
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.WriteLine("/help to see all commands\n"+ "current language: " + Converter.LanguageModel.Language);
			string input;
			while (true)
			{
				try
				{
					input = Console.ReadLine();
					var index = input.IndexOf(" ") == -1 ? input.Length : input.IndexOf(" ");
					var command = input.Substring(0, index);
					if (ConsoleWrapper.Commands.ContainsKey(command))
					{
						ConsoleWrapper.Commands[command].Func(input.Substring(index, input.Length - index));
					}
					else
					{
						ConsoleWrapper.DefaultComman(input);
					}
				}
				catch (ConsoleExeption ce)
				{
					Console.WriteLine(ce.Message);
				}
			}
		}
	}
}
