using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

            List<SentenceElement> lCheckedNodes = new List<SentenceElement>();

            // Queue of symbol cases which must be met.
            Stack<SentenceElement> lAgenda = new Stack<SentenceElement>();


            // lidt of symbols in KB where type is itself or not
            List<SentenceElement> lGivenSymbols = new List<SentenceElement>(lAgenda.Where(x => x.Operator.GetType() == typeof(Itself) || x.Operator.GetType() == typeof(Not)));

            // reference between symbols and the required rule to infer them.
            Dictionary<SentenceElement, SentenceElement> lInferenceLink = new Dictionary<SentenceElement, SentenceElement>();

            // list of symbols required to solve the query.
            List<SentenceElement> lRequired = new List<SentenceElement>();

            foreach (SentenceElement givenQuery in Query)
            {
                // Add all symbols to the agenda
                foreach (SentenceElement s in givenQuery.GetSymbols())
                {
                    s.ParentElement = new SentenceElement("&", new And()); // add a parent element to the symbol. This is the root node of the tree.
                    s.ParentElement.RightElement = s; // add the symbol to the right side of the parent element.
                    lAgenda.Push(s); // add the symbol to the agenda.
                }
                
                // process agenda
                do
                {
                    // dequeue symbol. symbol is direct reference to the symbol under the rule which was added. 
                    SentenceElement dequeuedSymbol = lAgenda.Pop();

                    lCheckedNodes.Add(dequeuedSymbol); // add symbol to inferred list

                    // where dequeue value is not 1 (not already inferred), find rules which infer it and requirements to dequeue
                    foreach (SentenceElement rule in KB.Where(r => r.Requires(dequeuedSymbol).Count > 0)) //  dequeuSymbol is contained in the rule
                    {
                        List<SentenceElement> lRuleRequirements = new List<SentenceElement>(rule.Requires(dequeuedSymbol)); // get the requirements for the rule
                        // if requirement parent is itself, it is a fact.
                        if (lRuleRequirements[0].ParentElement == lRuleRequirements[0])
                        {
                            // check if givenQuery is solved.
                            if (givenQuery.GetSymbols().All(x => x.ParentElement.Check())) // if all symbols are true, it is solved.
                            {
                                // if it is solved, clear the agenda and continue.
                                lAgenda.Clear();

                                // loop through inferencelink dictionary for .requires to build requiredForQuery list.
                                lRequired.Add(dequeuedSymbol);

                                // add the last element of the list to the requiredForQuery list until the last element is not in the dictionary.
                                if(lInferenceLink.ContainsKey(lRequired.Last().ParentElement))
                                { while (lInferenceLink.ContainsKey(lRequired.Last().ParentElement))
                                    {
                                        lRequired.Add(lInferenceLink[lRequired.Last().ParentElement]);
                                    }
                                }
                                else
                                {
                                    while (lInferenceLink.ContainsKey(lRequired.Last()))
                                    {
                                        lRequired.Add(lInferenceLink[lRequired.Last()]);
                                    }
                                }
                                // break out of foreach loop
                                break;
                            }
                        }
                        else // if there is more than 1 requirement, it is a rule
                        {
                            lInferenceLink.Add(rule.LeftElement, dequeuedSymbol); // add the rule to the inference link
                            //lInferenceLink.Add(dequeuedSymbol, rule.LeftElement); // add the rule to the inference link

                            // left side of rule replaces dequeue symbol in tree.
                            dequeuedSymbol.ParentElement.LeftElement = rule.LeftElement;
                            

                            // add requirements to agenda
                            rule.LeftElement.GetSymbols().ForEach(x => lAgenda.Push(x));
                        }
                    }
                } while (lAgenda.Count > 0);

                // If sibling is null, or no parent conditions are true, no solution was found.
                if(givenQuery.GetSymbols().Any(x => x.ParentElement.LeftElement == null || !x.ParentElement.Check()))
                {
                    return "NO";
                }
            }

            // lAgenda is now empty, lInferred contains the list of required symbols.
            string inferredSymbols = "";
            // if there are inferred symbols, add them to the output string. 
            inferredSymbols = String.Join(", ", lRequired.Select(x => x.Operator.Symbol + x.Name));

            if (lCheckedNodes.Count > 0)
                return "YES: " + inferredSymbols;
            else
                return "NO";
        }
    }
}
