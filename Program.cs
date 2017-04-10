/*
 * Simple way to make a histogram based on a date/time range (by the second the way it is 
 * set up now), non linq based.
 * 
 * Input file can be a csv change the line[X] to reflect the date/time
 * 
 * Made this to read a log of the output of one of my other programs, maybe someone would find this useful.
 * 
 * Works great with Gnu Octave
 * IE in Octave:
 * data = load("<<your file name>>");
 * plot(data);
 * 
 * --John Rasmussen
 * --john.rasmussen33@gmail.com
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace histogram1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();
            List<events> theevents = new List<events>();
            List<events> hist1 = new List<events>();
            string[] line;
            int counter;
            int counter2;
            DateTime tmpDate;
            DateTime mintime;
            DateTime maxtime;
            DateTime dateCount;
            string output;

            input = File.ReadAllLines("<<your csv here>>").ToList();

            //copy list of string into object
            foreach(string s in input)
            {
                line = s.Split(',');
                events e = new events();
                e.eventDate = Convert.ToDateTime(line[1]);
                e.x = 0;
                e.y = 0;
                theevents.Add(e);
            }

            //build a list to count the number of events a second
            for (int i = 0; i < theevents.Count; i++)
            {
                counter = 0;
                counter2 = 0;

                if (i < theevents.Count - 1)
                {
                    tmpDate = theevents.ElementAt(i).eventDate;
                    while (counter < theevents.ElementAt(i + 1).eventDate.Millisecond)
                    {
                        counter = theevents.ElementAt(i).eventDate.Millisecond;
                        counter2++;
                        i++;
                    }

                    events tmpEvent = new events();

                    //round down to the nearest second
                    tmpEvent.eventDate = tmpDate.AddTicks(-tmpDate.Ticks % TimeSpan.TicksPerSecond);
                    tmpEvent.numhits = counter2;
                    hist1.Add(tmpEvent);
                }
            }

            //get the min and max times in the full event list and round them down to the nearest second
            mintime = theevents.ElementAt(0).eventDate;
            maxtime = theevents.ElementAt(theevents.Count-1).eventDate;
            mintime = mintime.AddTicks(-mintime.Ticks % TimeSpan.TicksPerSecond);
            maxtime = maxtime.AddTicks(-maxtime.Ticks % TimeSpan.TicksPerSecond);

            //loop from the min second in the full event list to the max second in the full list

            counter = 0;
            for (dateCount = mintime; dateCount < maxtime; dateCount = dateCount.AddSeconds(1))
            {
                if (counter < hist1.Count)
                { 
                    //if they match output otherwise output 0 for no events
                    if (hist1.ElementAt(counter).eventDate == dateCount)
                    {
                        //output = dateCount + "," + hist1.ElementAt(counter).numhits;
                        output = hist1.ElementAt(counter).numhits.ToString();
                        counter++;
                    }
                    else
                    {
                        //output = dateCount + ",0";
                        output = "0";
                    }

                    Console.WriteLine(output);
                }
            }

           //Console.ReadLine();
        }
    }

    //just a test object
    public class events
    {
        public DateTime eventDate;
        public float x;
        public float y;
        public int numhits;
    }
}
