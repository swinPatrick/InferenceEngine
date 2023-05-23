using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.Algorithms
{
    internal class BC : Method
    {
        // Constructor
        public BC()
        {
            Name = "BC";
            KB = new List<SentenceElement>();
            Query = new List<SentenceElement>();
        }

        // Tell the engine what rules it has to work with
        public override void Tell(List<SentenceElement> aKB)
        {
            KB = aKB.ToList();
        }

        // Construct a tree and figure out what is required.
        // Input assumes aQuery elements value are set appropriately.
        public override string Ask(List<SentenceElement> aQuery)
        {
            // copy ASK
            Query = aQuery.ToList();

            List<SentenceElement> lInferred = new List<SentenceElement>();

            // Queue of symbol cases which must be met.
            Queue<SentenceElement> lAgenda = new Queue<SentenceElement>();
            

            // Add query values to the Agenda
            foreach (SentenceElement e in Query)
            {
                List<SentenceElement> eSymbols = new List<SentenceElement>();
                eSymbols.AddRange(e.GetSymbols());
                foreach(SentenceElement s in eSymbols)
                {
                    lAgenda.Enqueue(s);
                }
            }
            
            // Go through agenda (of things that are required), if something is required, add it to the list.
            while(lAgenda.Count > 0)
            {
                SentenceElement requiredSymbol = lAgenda.Dequeue();

                // check for contradictory requirements
                foreach(SentenceElement s in lInferred)
                {
                    if(s.Name == requiredSymbol.Name)
                    {
                        if(s.Value ==  requiredSymbol.Value)
                            continue;
                        else
                        {
                            // Catch contradictory requirements here. e.g. A && ~A
                        }
                    }
                }

                lInferred.Add(requiredSymbol);

                // now ask for what is required to make requiredSymbol satisfied.
                foreach(SentenceElement rule in KB)
                {
                    // get list of required symbols
                    List<SentenceElement> newRequirements = rule.Requires(requiredSymbol);

                    // if there are required symbols, enqueue them.
                    if(newRequirements != null)
                    {
                        foreach(SentenceElement s in newRequirements)
                            lAgenda.Enqueue((SentenceElement)s);
                    }
                }
            }

            // lAgenda is now empty, lInferred contains the list of required symbols.
            string inferredSymbols = "";
            inferredSymbols = String.Join(", ", lInferred);

            if (lInferred.Count > 0)
                return "YES: " + inferredSymbols;
            else
                return "NO";

            /*
            foreach(SentenceElement s in aQuery)
            {
                // what needs to be found
                SentenceElement query = s as SentenceElement;
                
                // search KB for query
                foreach(SentenceElement rule in KB)
                {
                    if(rule.Requires(query) != null)
                    {
                        // if rule is required to satisfy query, add it to the left side.
                        SentenceElement newLink = new SentenceElement("||", new Or());

                        // update newLink children = past requirement OR new requirement
                        newLink.RightElement = query.LeftElement;
                        newLink.LeftElement = rule;

                        // update past locations with new parent
                        rule.ParentElement = newLink;
                        query.LeftElement.ParentElement = newLink;

                        // add newLink as child of past requirement.
                        query.LeftElement = newLink;
                    }
                }
            }
            */
        }
    }
}
