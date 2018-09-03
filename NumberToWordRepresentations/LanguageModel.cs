using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
namespace NumberToWordRepresentations
{
    public class LanguageModel
    {
        public Dictionary<string,string> exception;
        public Dictionary<string,string[]> numbers;
        public string separator;
        public string upCurencyBase;
        public string downCurencyBase;
        public string textSeparator;
        public string Language;
        private int curencynum;
        public LanguageModel(){}
        public LanguageModel(string json,string languageName)
        {
            var temp = JsonConvert.DeserializeObject<LanguageModel>(json);
            separator = temp.separator;
            upCurencyBase = temp.upCurencyBase;
            downCurencyBase = temp.downCurencyBase;
            textSeparator = temp.textSeparator;
            exception = new Dictionary<string, string>();
            numbers = new Dictionary<string,string[]>();
            Language = languageName;

            BindModel(json);
        }
        private void BindModel(string Json)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(Json));
            while(reader.Read())
            {
                if(!(reader?.Value?.ToString() == "data" || reader?.Value?.ToString() == "exception")) 
                    continue;
                
                if(reader?.Value?.ToString() == "data"){
                    SetData(reader);
                }
                else if(reader?.Value?.ToString() == "exception"){
                    SetExeptions(reader);
                }   
            }
        }
        private void SetExeptions(JsonTextReader reader)
        {
            reader.Read();
            while(reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                var prop = reader.Value;
                reader.Read();
                exception.Add(prop?.ToString(),reader.Value?.ToString());
            }
        }
        private void SetData(JsonTextReader reader)
        {
            string key;
            while(reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                if(reader.TokenType == JsonToken.PropertyName)
                {
                    key = reader.Value.ToString();
                    reader.Read();
                    var i = 0;
                    var values = new string[9]{"-","-","-","-","-","-","-","-","-"};
                    while(reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                       values[i++] = reader.Value.ToString();  
                    }
                    numbers.Add(key,values);
                }
            }
            
        } 
        public string GetNumString(int upper,int lower)
        {
            curencynum = 0;
            return GetIntegerString(upper) + textSeparator + GetIntegerString(lower);
        }
        private string GetIntegerString(int integer)
        {
            int billion = 1000000000;
            int mask = (int)Math.Pow(10,integer.ToString().Length-1);
            if(exception.Keys.FirstOrDefault() == integer.ToString()) 
               return exception[0.ToString()];
            else if(integer/mask<=2 && exception.Keys.Contains(mask.ToString()) && mask != 10)
            {
                var id = (integer/mask*Math.Pow(10,integer.ToString().Length-1)).ToString();
                if(!exception.ContainsKey(id)) SeparateThousands(integer,billion);
                return exception[id]+" "+ SeparateThousands(integer%mask,billion);
            }
            else
                return SeparateThousands(integer,billion);
        }
        private string GetHundredsString(int number,bool isLast)
        {
           return SeparateTen(number,100,isLast);
        }
        private string GetCurency(){
            var curency = curencynum == 0 ? "upCurency":"downCurency";
            curencynum++;
            return curency;
        }
        private string GetCurencyString(bool isLast,int id){
          return  isLast ? " "+numbers[GetCurency()][id]:" ";       
        }
        private string SeparateTen(int number, int separator ,bool isLast)
        {
            if(number == 0) return GetCurencyString(isLast,8);
            
            if(exception.ContainsKey(number.ToString()) && exception.Keys.FirstOrDefault() != number.ToString()) 
            {
                return exception[number.ToString()]+GetCurencyString(isLast,8);
            }
            int num = number / separator;
            
            if(separator > 1)
            {
                if(num != 0) return numbers[separator.ToString()][num-1] +" "+SeparateTen(number%separator,separator/10,isLast); 
                return SeparateTen(number%separator,separator/10,isLast);   
            }
            else {
                return numbers[separator.ToString()][num-1]+GetCurencyString(isLast,num-1);
            }
        }
        private int GetLastDigit(int integer)
        {
            var lastDigit = integer%10; 
            if(lastDigit == 0)
                lastDigit = 9;   
            return lastDigit;
        }
        public string GetGrade(int grade,int lastDigit){
            if(grade >=1000)
                return numbers[grade.ToString()][lastDigit-1]+" ";
            else return "";
        }
        private string SeparateThousands(int number, int separator)
        {
            string converted = "";
            var integer = number / separator;
            var fractional = number%separator;
            bool isLast = separator == 1;
            if(exception.ContainsKey(number.ToString()) && exception.First().Key != number.ToString())
            {
                return exception[number.ToString()]+" "+ numbers[GetCurency()][8];
            }
            if(integer > 0)
            {
                converted += GetHundredsString(integer,isLast);
                if(separator >= 1000) converted+= GetGrade(separator,GetLastDigit(integer));
            }

            if(isLast) return converted;
            converted += SeparateThousands(fractional,separator/1000);  
            return converted;
        }
    }
}