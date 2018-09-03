using System;
using System.Collections.Generic;
namespace NumberToWordRepresentations
{
	public static class ConsoleWrapper
	{
		public static Dictionary<string, Commnad> Commands = new Dictionary<string, Commnad>(){
			{"/add",new Commnad("/add","/add name path",(string text) => {
				try
				{
					var inputParams = text.Split(' ');
					JsonHandler.AddPackage(inputParams[1],inputParams[2]);
				}
				catch { throw new ConsoleExeption("bad params");}
			})},
			{"/change",new Commnad("/change","/change name",(string text) => {
				try
				{
					var inputParams = text.Split(' ');
					Converter.ChangeLanguage(inputParams[1]);
					Console.WriteLine("language: "+Converter.LanguageModel.Language);
				}
				catch { throw new ConsoleExeption("bad params");}
			})},
			{"/langs",new Commnad("/langs","/langs",(string text) => {
				foreach(var language in Converter.GetLanguages())
				{
					Console.WriteLine(language);
				}
			})},
			{"/help",new Commnad("/help","/help",(string text) => {
				foreach(var commnad in Commands)
				{
					Console.WriteLine(commnad.Value.Description);
				}
			})},
			{"/exit",new Commnad("/exit","/exit",(string text) => {
				Environment.Exit(-1);
			})}};
		public static Action<string> DefaultComman = (string text) => {
			Console.WriteLine(NumberToWordRepresentations.Converter.Convert(text));
		};
	}
	public class Commnad
	{
		public string CommnadText;
		public string Description;
		public Action<string> Func;
		public Commnad(string commnadText, string description, Action<string> func)
		{
			CommnadText = CommnadText;
			Description = description;
			Func = func;
		}
	}
}
