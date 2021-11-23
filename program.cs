using System;
using System.Collections.Generic;
using System.IO;

public class Program {

    public class Entry {
        public string firstName;
        public string lastName;
        public string date;
        public int division;
        public int points;
        public string summary;

        // Constructor functions in terms of core variables and the class for convenience
        public Entry(string firstName, string lastName, string date, int division, int points, string summary){
            this.firstName = firstName;
            this.lastName = lastName;
            this.date = date;
            this.division = division;
            this.points = points;
            this.summary = summary;
        }
        public Entry(Entry i){
            this.firstName = i.firstName;
            this.lastName = i.lastName;
            this.date = i.date;
            this.division = i.division;
            this.points = i.points;
            this.summary = i.summary;      
        }

    };

    // Counts the amount of lines in a CSV file
    public static int lineCount(string filePath){
        using (var reader = new StreamReader(filePath)){
            // Throwaway the top line
            reader.ReadLine();
            int i=0;
            while (reader.ReadLine() !=null){
                i++;
            }
            return i;
        }
    }


    public static int Main(string[] args){
        
        int lines = lineCount(args[0]);
        Entry[] entry = new Entry[lines];

        using(var reader = new StreamReader(args[0])){
            // Throw away the top line
            reader.ReadLine();

            int i=0;

            while (!reader.EndOfStream){
                var line = reader.ReadLine();
                var values = line.Split(',');
                
                // Checks if each line of the CSV file is valid
                if (values.Length !=6){
                    Console.WriteLine("Error: Line " + (i+1).ToString() + " of " + args[0] + " has " + values.Length + " parameters instead of 6!");
                    return -1;
                }

                entry[i] = new Entry(values[0],values[1],values[2],Int32.Parse(values[3]),Int32.Parse(values[4]),values[5].Substring(1,values[5].Length-2));
                i++;
            }
        }

        // Sorting Algorithm - it swaps entries if they are in the wrong order while looping through enough times to get each entry in the correct spot overall
        // (if both points and division are equal then the entries are not swapped
        for (int j=0; j<entry.Length-1; j++){
            for (int i=0; i<entry.Length-1-j; i++){
                if (entry[i].division > entry[i+1].division || entry[i].division == entry[i+1].division && entry[i].points < entry[i+1].points){
                    // Creates temp object to store the entry as it is swapped
                    Entry temp = new Entry(entry[i]);
                    entry[i] = entry[i+1];
                    entry[i+1] = temp;
                }   
            }
        }

        // Prints out required output in YAML format
        Console.WriteLine("records: ");

        int printLines = 3;
        if (printLines > lines){
            printLines = lines;
        }

        for (int i=0; i<printLines; i++){
            Console.WriteLine("- name: " + entry[i].firstName + " " + entry[i].lastName);
            Console.WriteLine("  details: " + "In division " + entry[i].division + " from "+ entry[i].date + " performing "+ entry[i].summary);
        }
        return 1;
    }
}

