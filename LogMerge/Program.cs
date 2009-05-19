using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace LogMerge
{
    class Program
    {
        public static DateTime CurrentDate = DateTime.Parse("05/01/2009");//DateTime.Parse("01/01/2007");
        public static DateTime EndDate = DateTime.Parse("06/01/2009");
        public static Hits ListOfHits = new Hits();
        public static Hashtable SortedByDates = new Hashtable();
        public static Hashtable DateAndFType = new Hashtable();
        public static List<String> FTypes = new List<String>();
        public static Hashtable TypesByMonth = new Hashtable();
        static void Main(string[] args)
        {
            ArrayList files = new ArrayList();
            foreach (string dir in args)
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    files.Add(file);
                }
            }
            foreach (String log in files)
            {
                CategorizeEntries(log);
            }
            GetAllFileTypes();
            string NEW_LINE = "\n";
            string PIPE = ",";
            string Output = PrintHeader(PIPE);
            foreach (String ftype in FTypes)
            {
                DateTime s = CurrentDate;
                Output += ftype.ToUpper() + PIPE;
                while (s < EndDate)
                {
                    int count = 0;
                    DateTime e = DateTime.Parse(CurrentDate.Year + "-" + CurrentDate.Month + "-" + DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month) + " 23:59:59");
                    IEnumerable<Hit> query = ListOfHits.Where(x => x.TypeOfFile.ToUpper() == ftype.ToUpper()).Where(x => x.Time >= s && x.Time <= e);
                    foreach (Hit hit in query)
                    {
                        count++;
                    }
                    Output += count.ToString() + PIPE;
                    s = s.AddMonths(1);
                }
                Output += NEW_LINE;
            }
            TextWriter tw = new StreamWriter("output.csv");
            tw.WriteLine(Output);
            tw.Close();
            System.Diagnostics.Process.Start("output.csv");
        }

        private static string PrintHeader(string PIPE)
        {
            string output = PIPE;
            DateTime s = CurrentDate;
            while (s < EndDate)
            {
                output += s.Month.ToString() + "/" + s.Year.ToString() + PIPE;
                s = s.AddMonths(1);
            }
            output += "\n";
            return output;
        }
        public static void CategorizeEntries(string logFile)
        {
            try
            {
                DateTime start = DateTime.Now;
                TextReader tr = new StreamReader(logFile);
                string line = String.Empty;
                while ((line = tr.ReadLine()) != null)
                    if (line.StartsWith("#"))
                        continue;
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Found " + ListOfHits.HitsTable.Count.ToString() + " Records...");
                        ListOfHits.Add(GetHit(line));
                    }

                TimeSpan length = DateTime.Now.Subtract(start);
                int averagePerSecond = ListOfHits.HitsTable.Count / length.Seconds;
                Console.Write("Averaged " + averagePerSecond + " entries per second.");
                Console.Write(" (" + length.Seconds.ToString() + " seconds total)");
                //Console.ReadKey();
            }

            catch (Exception ex)
            {
                Console.Error.Write(ex.Message + "\n" + ex.StackTrace);
            }
        }
        public static Hit GetHit(string line)
        {
            Hit h = new Hit();
            char[] splitter = new char[] { ' ' };
            string[] i = line.Split(splitter);
            h.Time = DateTime.Parse(i[0] + " " + i[1]); // Time
            h.Server = i[3];
            h.Verb = i[5];
            h.Location = i[6];
            h.Status = i[16];
            try
            {
                h.TypeOfFile = h.Location.Substring(h.Location.LastIndexOf(".")).ToUpper();
            }
            catch (ArgumentOutOfRangeException)
            {
                h.TypeOfFile = "DIRECTORY";
            }
            return h;
        }
        public static void GetAllFileTypes()
        {
            foreach (Hit h in ListOfHits)
            {
                if(!FTypes.Contains(h.TypeOfFile.ToUpper()))
                {
                    FTypes.Add(h.TypeOfFile.ToUpper());
                }
            }
        }
    }
}
