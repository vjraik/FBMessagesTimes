using System;
using System.Collections.Generic;
using System.Linq;

namespace FBMessagesTimes
{
    class Program
    {
                
        static void Main(string[] args)
        {
            Console.WriteLine("Give the full path to the location of the txt file that contains the messages in original HTML.");
            string location = Console.ReadLine();
            string Path = @location;
            string messagesHTML = GetFile(location);

            //We are looking for this format.
            // string example = @"<span class=user>User Name</span><span class=meta>day, Month Date, Year at Time AmOrPm UTC+time</span>";

            Console.WriteLine("Write the name of the first user and press Enter.");
            string user1 = Console.ReadLine();

            string start1 = "<span class=user>" + user1 + "</span><span class=meta>";

            Console.WriteLine("Write the name of the second user and press Enter.");
            string user2 = Console.ReadLine();

            string start2 = "<span class=user>" + user2 + "</span><span class=meta>";

            Console.WriteLine("Give the full path to the location you want the results to go. Will overwrite existing file. Remember to add forward slash to the end if needed.");
            string locationTo = Console.ReadLine();

            WriteByName(locationTo, messagesHTML, start1, user1 + "Messages");

            WriteByName(locationTo, messagesHTML, start2, user2 + "Messages");

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        //Gets the timestamps of messages written by user whose name is in strt string and writes a new txt file containing line with format: day, Month Date, Year at Time AmOrPm
        private static void WriteByName(string locationTo, string messagesHTML, string start, string fileEnd)
        {
            
            List<string> timeStamps = GetTimes(messagesHTML, start);

            WriteFile(timeStamps, @locationTo + fileEnd + ".txt");
        }

        private static List<string> GetTimes(string messagesHTML, string start)
        {
            List<int> places = AllIndexesOf(messagesHTML, start);

            List<int> stops = FindEnds(messagesHTML, places, start);

            List<string> timeStamps = TimeStamps(messagesHTML, start, places, stops);
            return timeStamps;
        }

        private static List<string> TimeStamps(string messagesHTML, string start, List<int> places, List<int> stops)
        {
            List<string> timeStamps = TimeStamps(messagesHTML, places, stops);

            for (int i = 0; i < timeStamps.Count(); i++)
            {
                timeStamps[i] = timeStamps[i].Substring(start.Length, timeStamps[i].Length - start.Length - 1 - 7);
            }

            return timeStamps;
        }

        private static void WriteFile(List<string> timeStamps, string PathNew)
        {
            System.IO.File.WriteAllLines(PathNew, timeStamps);
        }

        private static List<string> TimeStamps(string messagesHTML, List<int> places, List<int> stops)
        {
            List<string> timeStamps = new List<string>();

            for (int i = 0; i < places.Count(); i++)
            {
                timeStamps.Add(messagesHTML.Substring(places[i], stops[i] - places[i] + 1));
            }
            return timeStamps;
        }

        private static List<int> FindEnds(string messageHTML, List<int> places, string start)
        {
            List<int> stops = new List<int>();

            int stLength = start.Length;

            for (int i = 0; i < places.Count(); i++)
            {
                stops.Add(messageHTML.IndexOf("<", places[i] + stLength + 1));
            }
            return stops;
        }

        public static List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }


        public static string GetFile(string path)
        {

            // Read the file as one string.
            string messagesHTML = System.IO.File.ReadAllText(path);

            return messagesHTML;
        }
    }


}

