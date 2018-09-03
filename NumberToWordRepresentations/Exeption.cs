using System;
namespace NumberToWordRepresentations
{
    public class ConsoleExeption: Exception
    {
        public ConsoleExeption(string message)
            :base(message){}
    }
}