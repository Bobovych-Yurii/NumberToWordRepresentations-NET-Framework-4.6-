using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
namespace NumberToWordRepresentations
{
    public static class Converter {
        
        public static LanguageModel LanguageModel;
        
        static Converter()
        {
            LanguageModel = JsonHandler.GetDefaultLanguage();
        } 
        public static void ChangeLanguage(string language){
           LanguageModel = JsonHandler.GetLangModelFromFile(language);
        }
        public static IEnumerable<string> GetLanguages(){
            return JsonHandler.GetLanguagesEnumerable();
        }
        public static string Convert(string numberInString){
            if(numberInString.IndexOf(LanguageModel.separator) != numberInString.LastIndexOf(LanguageModel.separator) )
            {
                throw new ConsoleExeption("bad number");
            }
            var numbers = numberInString.Split(LanguageModel.separator[0]);
			if(numbers[1].Length > 2)
				throw new ConsoleExeption("bad number");
			try
			{
                var upper = System.Convert.ToInt32(numbers[0]);
                var lower = System.Convert.ToInt32(numbers[1]);
                lower = lower < 10 ? lower*10 : lower;
                return LanguageModel.GetNumString(upper,lower);
            }
            catch {
                throw new ConsoleExeption("bad number");
            }
        }
       
       
        
    }
}