using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class FileReader
    {
        public static string ReadFile(string filename, List<SentenceElement> aKB, List<SentenceElement> aQ)
        {
            string consoleOutput = "";

            try
            {
                string lineFromFile;
                StreamReader reader = new StreamReader(filename);
                lineFromFile = reader.ReadLine();
                string tempLine = "";

                if (lineFromFile == "TELL")
                {
                    lineFromFile = reader.ReadLine();
                    while (lineFromFile.Trim() != "ASK")
                    {
                        tempLine = tempLine.Trim() + lineFromFile;
                        string line = tempLine;//"p2=> p3; p3 => p1; c => e; b&e => f; f&g => h; p1=>d; p1&p3 => c; a; b;p2;";
                        line = line.Replace(" ", "");

                        lineFromFile = reader.ReadLine();
                        List<SentenceElement> RULES = new List<SentenceElement>();
                        SentenceElement temp_store;
                        string[] component_Parts = Regex.Split(line, @"(?<!^);");
                        foreach (string component in component_Parts)
                        {
                            consoleOutput += "\n" + component;

                            //string[] _Temp_String_Operator_set = { "=>", "||", "&", "~" };
                            //if no entails then values are just the value of the itself
                            Match Entails_Check = Regex.Match(component, @"\s*(=>)\s*");
                            if (Entails_Check.Success)
                            {
                                temp_store = new SentenceElement("=>", new Entails());

                                string[] temp_Section = Regex.Split(component, @"\s*(=>)\s*");
                                Match left_operators = Regex.Match(temp_Section[0], @"\s*(\&|\|\||~)\s*");
                                // if left has a logic section - currently doesn't work for multiple operators on left side but dont believe this is nessersary till general KB, this can be implemented with changing this to be a called function and recursivly called baesd on brackets
                                if (left_operators.Success)
                                {


                                    string[] temp_sec_set = Regex.Split(temp_Section[0], @"\s*(\&|\|\||~)\s*");
                                    switch (left_operators.Value)
                                    {
                                        case "||":
                                            {
                                                temp_store.LeftElement = new SentenceElement("||", new Or());
                                                break;
                                            }
                                        case "&":
                                            {
                                                temp_store.LeftElement = new SentenceElement("&", new And());
                                                break;
                                            }
                                        case "~":
                                            {
                                                temp_store.LeftElement = new SentenceElement("~", new Not());
                                                break;
                                            }
                                    }

                                    //Will probably implement a deeping loop on every success match = true check again with a whole loop and go 1 lvl deeper, then add to stack and op off one by one as we resurface
                                    temp_store.LeftElement.LeftElement = new SentenceElement(temp_sec_set[0], new Itself());
                                    temp_store.LeftElement.RightElement = new SentenceElement(temp_sec_set[2], new Itself());

                                    temp_store.RightElement = new SentenceElement(temp_Section[2], new Itself());
                                }
                                else
                                {
                                    temp_store.LeftElement = new SentenceElement(temp_Section[0], new Itself());
                                    temp_store.RightElement = new SentenceElement(temp_Section[2], new Itself());
                                }

                            }
                            else
                            {
                                temp_store = new SentenceElement(component, new Itself());
                                temp_store.Value = 1;
                            }
                            if (!string.IsNullOrEmpty(temp_store.Name))
                            {
                                aKB.Add(temp_store);
                            }


                        }
                    }
                }
                if (lineFromFile == "ASK")
                {
                    lineFromFile = reader.ReadLine();

                    aQ.Add(new SentenceElement(lineFromFile, new Itself()));

                }
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
