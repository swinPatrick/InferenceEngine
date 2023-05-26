using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class FileReader
    {
        static Operator[] operatorList = { new Entails(), new And(), new Or() };

        private static SentenceElement StringToSentenceElement(string aElementAsString)
        {
            /*
             *  Split by Operators.
             *      & : convert all and join
             *      ||: convert all and join
             *      =>: convert both sides
             *      ~ : apply to node
             *      (): not implemented.
             */
            SentenceElement newElement = null;
            // split string by all operators. (not including NOT) 
            string[] side = Regex.Split(aElementAsString, @"\s*(=>|\&|\|\|)\s*");

            if(side.Length == 1)
            {
                side = Regex.Split(side[0], @"\s*(~)\s*");
                // if length is 1, no ~
                if (side.Length == 1)
                    newElement = new SentenceElement(side[0], aValue: 1);
                else
                    newElement = new SentenceElement(side[2], new Not());
            }
            else
            {
                string pattern;
                // new pattern for each operator. (only handle one at a time).
                foreach(Operator op in operatorList)
                {
                    // create the respective pattern @"\s*(=>)\s*"
                    pattern = "(" + op.Symbol + ")";
                    Regex rgx = new Regex(pattern);
                    // apply the split to the string. we want 2 sides from it.
                    side = rgx.Split(aElementAsString, 2);
                    // populate element if split was effective (side.Length>1)
                    if( side.Length > 1)
                    {
                        newElement = new SentenceElement(op.Symbol, op);

                        newElement.LeftElement = StringToSentenceElement((string)side[0]);
                        newElement.LeftElement.ParentElement = newElement;

                        newElement.RightElement = StringToSentenceElement((string)(side[2]));
                        newElement.RightElement.ParentElement = newElement;

                        break;
                    }
                }
            }

            // catch error if newElement is null sommething has gone wrong.
            if (newElement == null)
                throw new Exception("Error in StringToSentenceElement. newElement is null.");

            return newElement;
        }
        
        public static string ReadFile(string filename, List<SentenceElement> aKB, List<SentenceElement> aQ)
        {
            string consoleOutput = "";

            try
            {
                string fileLine = "";
                string lineType = "";

                StreamReader reader = new StreamReader(filename);

                do
                {
                    if (fileLine.Contains("TELL"))
                    {
                        lineType = "TELL";
                        consoleOutput += "TELL\n";
                    }
                    else if (fileLine.Contains("ASK"))
                    {
                        lineType = "ASK";
                        consoleOutput += "ASK\n";
                    }
                    else
                    {
                        List<SentenceElement> lRow = new List<SentenceElement>();
                        // read row
                        fileLine = fileLine.Replace(" ", "");
                        string[] lineSelection = fileLine.Split(';');
                        foreach (string strRule in lineSelection)
                        {
                            if (strRule == "")
                                continue;
                            lRow.Add(StringToSentenceElement(strRule));

                            //consoleOutput += strRule + "\n";
                            consoleOutput += lRow.Last().ToString() + "\n";
                        }

                        // add to either KB or Query
                        if (lineType == "TELL")
                            aKB.AddRange(lRow);
                        else if (lineType == "ASK")
                            aQ.AddRange(lRow);
                    }

                    fileLine = reader.ReadLine();
                } while (fileLine != null);
            }
            catch (FileNotFoundException)
            {
                //The file didn't exist, show an error
                Console.WriteLine("Error: File \"" + filename + "\" not found.");
                Console.WriteLine("Please check the path to the file.");
                Environment.Exit(0);
            }
            catch (IOException)
            {
                //There was an IO error, show and error message
                Console.WriteLine("Error in reading \"" + filename + "\". Try closing it and programs that may be accessing it.");
                Console.WriteLine("If you're accessing this file over a network, try making a local copy.");
                Environment.Exit(0);
            }

            return consoleOutput;
        }
    }
}
