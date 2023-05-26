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
            //foreach rule in KB: if rule is not a fact, set left element symbols to 0.
            foreach (SentenceElement rule in KB)
            {
                if (rule.Operator.Symbol != "")
                {
                    rule.LeftElement.GetSymbols().ForEach(x => x.Value = 0);
                }
            }
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
                    SentenceElement dequeuedSymbol = lAgenda.Pop();

                    //lCheckedNodes.Add(dequeuedSymbol); // add symbol to checked list


                    // find rules which effect the dequeued symbol
                    foreach (SentenceElement rule in KB.Where(r => r.Requires(dequeuedSymbol).Count > 0))
                    {
                        // create a list of the requirements for the rule
                        List<SentenceElement> lRuleRequirements = new List<SentenceElement>(rule.Requires(dequeuedSymbol)); // get the requirements for the rule

                        // if requirement parent is itself, or not, it is a statement.
                        if (lRuleRequirements[0].ParentElement == lRuleRequirements[0])
                        {

                            SentenceElement lastChecked = lRuleRequirements[0];
                            while (lInferenceLink.Any(x => lastChecked.Name == x.Key.Name))
                            { 
                                KeyValuePair<SentenceElement, SentenceElement> link = lInferenceLink.Where(x => x.Key.Name == lastChecked.Name).First();
                                link.Key.ParentElement.Apply(lRuleRequirements[0]);
                                if(link.Key.ParentElement.Check())
                                    link.Value.Value = 1;
                                lastChecked = link.Value;
                            }

                            // check if givenQuery is solved.
                            if (givenQuery.GetSymbols().All(x => x.ParentElement.Check())) // if all symbols are true, it is solved.
                            {
                                // if it is solved, clear the agenda and continue.
                                lAgenda.Clear();

                                // find in inferredlink list where symbol is the value. if key value is 1, add value to required list.
                                List<KeyValuePair<SentenceElement, SentenceElement>> links = lInferenceLink.Where(x => x.Value.Name == givenQuery.Name).ToList();
                                lRequired.Add(givenQuery);
                                while (links.Any(x => x.Key.Value == 1))
                                {
                                    lRequired.Add(links.FirstOrDefault(l => l.Key.Value == 1).Key);
                                    links.Remove(links.FirstOrDefault(l => l.Key.Value == 1));
                                    links.AddRange(lInferenceLink.Where(x => x.Value.Name == lRequired.Last().Name));
                                }
                                

                                break;
                            }
                        }
                        else // if there is more than 1 requirement, it is a rule
                        {
                            // add requirements to agenda and link them to the right side of the rule.
                            foreach(SentenceElement symbol in rule.LeftElement.GetSymbols())
                            {
                                lInferenceLink.Add(symbol, dequeuedSymbol); // link the symbol to the rule.
                                lAgenda.Push(symbol); // add the symbol to the agenda.
                            }

                            // left side of rule replaces dequeue symbol in tree.
                            dequeuedSymbol.ParentElement.LeftElement = rule.LeftElement;

                            // add requirements to agenda
                            rule.LeftElement.GetSymbols().ForEach(x => lAgenda.Push(x));
                        }
                    }
                } while (lAgenda.Count > 0);

                // If sibling is null, or no parent conditions are true, no solution was found.
                if (givenQuery.GetSymbols().Any(x => x.ParentElement.LeftElement == null || !x.ParentElement.Check()))
                {
                    return "NO";
                }
            }

            // lAgenda is now empty, lInferred contains the list of required symbols.
            string inferredSymbols = "";
            // if there are inferred symbols, add them to the output string. 
            inferredSymbols = String.Join(", ", lRequired.Select(x => x.Operator.Symbol + x.Name));

            if (lRequired.Count > 0)
                return "YES: " + inferredSymbols;
            else
                return "NO";
        }
    }
}
