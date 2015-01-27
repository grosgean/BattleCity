using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type the number of points you have got:");
            int totalSecondsPlayed;
            totalSecondsPlayed = int.Parse(Console.ReadLine());
            const int SECONDS_PER_MINUTE = 60;

            int minutes = totalSecondsPlayed / SECONDS_PER_MINUTE;
            int seconds = totalSecondsPlayed % SECONDS_PER_MINUTE;

            Console.WriteLine("Minutes: " + minutes);
            Console.WriteLine("Seconds: " + seconds);
            Console.WriteLine();
        }
    }
}
