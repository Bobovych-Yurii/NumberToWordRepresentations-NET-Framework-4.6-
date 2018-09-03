using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace NumberToWordRepresentations
{
    public static class JsonHandler
    {
        private const string initFilePath = "langsData.json";
        private static Dictionary<string,string> langPath = new Dictionary<string, string>();

        static JsonHandler()
        {
            var JsonObj = GetJsonFromFile(initFilePath);
            JsonTextReader reader = new JsonTextReader(new StringReader(JsonObj));
            while(reader.Read())
            {
                if (reader.Value != null)
                {
                    string prop = reader.Value.ToString();
                    reader.Read();
                    langPath.Add(prop,reader.Value.ToString());
                }
            }
            if(langPath.Count < 1) throw new ConsoleExeption("no data in init file");
            
        }
        public static void AddPackage(string language,string path)
        {
            var jsonObj = GetJsonFromFile(path);
            try
            {
                new LanguageModel(jsonObj,langPath.First().Key);
            }
            catch
            {
                throw new ConsoleExeption("wrong json");
            }
            AddPackageToFile(language,path);
        }
        private static void AddPackageToFile(string language,string path)
        {
            langPath.Add(language,path);
            string json = "{";
            foreach(var item in langPath)
            {
                json += "\""+item.Key+"\":"+"\""+item.Value+"\",";
            }
            json+="}";
            WriteToFile(json);
        }
        private static void WriteToFile(string json)
        {
            try
            {
                using(FileStream fs = new FileStream(initFilePath, FileMode.Open, FileAccess.Write))
                {   
                    using (StreamWriter wr = new StreamWriter(fs))
                    {   
                        wr.Write(json);
                    }           
                }
            }
            catch {
                throw new ConsoleExeption("bad file");
            }
        }
        public static LanguageModel GetLangModelFromFile(string language){
            if(!langPath.ContainsKey(language)) throw new ConsoleExeption("no such language package");
            var jsonObj = GetJsonFromFile(langPath[language]);
            return new LanguageModel(jsonObj,langPath.First().Key);
        }
        public static IEnumerable<string> GetLanguagesEnumerable(){
            return langPath.Keys;
        }
        
        public static LanguageModel GetDefaultLanguage()
        {
            var jsonObj = GetJsonFromFile(langPath.First().Value);
            return new LanguageModel(jsonObj,langPath.First().Key);
        }
        private static string GetJsonFromFile(string path)
        {
            string JsonObj = "";
            try
            {
                using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {   
                    using (StreamReader sr = new StreamReader(fs))
                    {   
                        JsonObj = sr.ReadToEnd();
                    }           
                }
            }
            catch {
                throw new ConsoleExeption("bad file");
            }
            return JsonObj;
        }
    }
}