using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using InferenceEngine.Algorithms;

namespace InferenceEngine
{
    internal class Program
    {
        private static List<Method> methods;
        static void Main(string[] args)
        {
            InitMethods();
            // iengine <method> <filename>

            // check if input is valid
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: iengine <method> <filename>");
                Console.WriteLine("Methods:");
                foreach (Method m in methods)
                {
                    Console.WriteLine("\t" + m.Name);
                }
                return;
            }
            string method = args[0];
            string filename = args[1];
            // filename = "testHornKB.txt";

            // Initialise KnowledgeBase and Query variables.
            List<SentenceElement> KnowledgeBase = new List<SentenceElement>();
            List<SentenceElement> Query = new List<SentenceElement>();

            // Set variable that were read by document.
            //FileReader.ReadFile(filename, KnowledgeBase, Query);
            // otherwise, if filereader output is required:
            Console.WriteLine(FileReader.ReadFile(filename, KnowledgeBase, Query));

            // Set Method based on input <method>
            Method engineMethod = GetMethod(method);

            // Tell the engine the knowledgebase
            engineMethod.Tell(KnowledgeBase);

            // Ask the engine the query
            Console.WriteLine(engineMethod.Ask(Query));

            Console.ReadKey();
        }
       

        private static void InitMethods()
        { 
            methods = new List<Method>();
            methods.Add(new TruthTable());
            methods.Add(new FC());
            methods.Add(new BC());
        }

        private static Method GetMethod(string aName)
        {
            foreach (Method m in methods)
            {
                if (m.Name == aName)
                    return m;
            }
            return null;
        }
    }

}
