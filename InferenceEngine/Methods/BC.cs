using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.Algorithms
{
    internal class BC
    {
        // Symbols is a list of SentenceElements (leaf values).
        // no two elements have the same name.
        private List<SentenceElement> Symbols;

        // Model is a dictionary of SentenceElements and their values.
        // it is the current row of the truth table.
        private Dictionary<SentenceElement, int> Model;

        // Constructor
        public BC(List<SentenceElement> aKnowledgeBase, List<SentenceElement> aQuery)
        {
            List<SentenceElement> KnowledgeBase = aKnowledgeBase;
            List<SentenceElement> Query = aQuery;
        }

    }
}
