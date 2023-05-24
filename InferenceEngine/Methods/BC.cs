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
            Stack<SentenceElement> lAgenda = new Stack<SentenceElement>();


            // lidt of symbols in KB where type is itself or not
            List<SentenceElement> lGivenSymbols = new List<SentenceElement>(lAgenda.Where(x => x.Operator.GetType() == typeof(Itself) || x.Operator.GetType() == typeof(Not)));

            foreach (SentenceElement givenQuery in Query)
            {
                // Add all symbols to the agenda
                foreach (SentenceElement s in givenQuery.GetSymbols())
                {
                    s.Value = 0;
                    lAgenda.Push(s);
                }
                
                // process agenda
                do
                {
                    // dequeue symbol. symbol is direct reference to the symbol under the rule which was added. 
                    SentenceElement dequeuedSymbol = lAgenda.Pop();

                    lInferred.Add(dequeuedSymbol); // add symbol to inferred list

                    // where dequeue value is not 1 (not already inferred), find rules which infer it and requirements to dequeue
                    foreach (SentenceElement rule in KB.Where(r => r.Requires(dequeuedSymbol).Count > 0)) //  dequeuSymbol is contained in the rule
                    {
                        List<SentenceElement> lRuleRequirements = new List<SentenceElement>(rule.Requires(dequeuedSymbol)); // get the requirements for the rule
                        if(lRuleRequirements.Count == 1) // if there is 1 requirement, it is a fact
                        {
                            // check if givenQuery is solved.
                            if( givenQuery.ParentElement.Check() )
                            {
                                // if it is solved, clear the agenda and continue.
                                lAgenda.Clear();
                                break;
                            }
                        }
                        else // if there is more than 1 requirement, it is a rule
                        {
                            // left side of rule replaces dequeue symbol in tree.
                            dequeuedSymbol.ParentElement.LeftElement = rule.LeftElement;

                            // add requirements to agenda
                            rule.LeftElement.GetSymbols().ForEach(x => lAgenda.Push(x));
                        }
                    }
                } while (lAgenda.Count > 0);
            }

            // lAgenda is now empty, lInferred contains the list of required symbols.
            string inferredSymbols = "";
            // if there are inferred symbols, add them to the output string. Add a "!" if the operator is Not.
            inferredSymbols = String.Join(", ", lInferred.Select(x => x.Operator.GetType() == typeof(Not) ? "!" + x.Name : x.Name));

            if (lInferred.Count > 0)
                return "YES: " + inferredSymbols;
            else
                return "NO";
        }

        private SentenceElement AddJoint(SentenceElement aNewRequisite, SentenceElement aBaseElement, Operator @operator)
        {
            SentenceElement lJoiner =  new SentenceElement(@operator.Symbol, @operator);
            lJoiner.LeftElement = aNewRequisite;
            lJoiner.RightElement = aBaseElement;
            return lJoiner;
        }
    }
}
