/*
using InferenceEngine.Environment.Operators;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InferenceEngine
{
    internal class TruthTable : Method
    {
        // Symbols is a list of SentenceElements (leaf values).
        // no two elements have the same name.
        private List<SentenceElement> Symbols;

        // Model is a dictionary of SentenceElements and their values.
        // it is the current row of the truth table.
        private Dictionary<SentenceElement, int> Model;

        // Constructor
        public TruthTable(List<SentenceElement> aKnowledgeBase, List<SentenceElement> aQuery)
        {
            KnowledgeBase.Clear();
        }

        public override Tell(List<SentenceElement> aKB)
        {
            Symbols = GetSymbols(aKB);

            // first row of truth table is all false
            Model = new List<SentenceElement>();
            foreach (SentenceElement symbol in Symbols)
            {
                Model.Add(symbol, 0);
            }

            // create a row for each possible combination of true/false
            // check if row satisfies Rules
            // if so, add to KB
            // else, discard row
            // repeat until all rows have been checked
            // if KB is empty, return false
            // else, return true

            for (int i = 0; i < Math.Pow(2, Symbols.Count); i++)
            {
                if (CheckRow(Model, aKB))
                {
                    KnowledgeBase.Add(Model);
                }
                Model = NextRow(Model);
            }

            // KnowledgeBase is now a list of all rows that satisfy KB
        }

        public override List<SentenceElement> Ask(List<SentenceElement> aSentence)
        {
            Query.AddRange(aSentence);
            // count how many rows of KB satisfy Query
            // if count is 0, return false
            // else, return true
            int count = 0;
            foreach (List<SentenceElement> row in KnowledgeBase)
            {
                if (CheckRow(row, Query))
                {
                    count++;
                }
            }

            // return number of rows that satisfy Query (as sentence element with name as count)
            if (count > 0)
                return new List<SentenceElement> { new SentenceElement(aName: count.ToString()) };
            else return null;
        }

        // given a list of SentenceElements, return a list of SentenceElements that are symbols
        private List<SentenceElement> GetSymbols(List<SentenceElement> aSentence)
        {
            List<SentenceElement> symbols = new List<SentenceElement>();
            foreach (SentenceElement sentence in aSentence)
            {
                if (sentence.Operator is Itself)
                {
                    symbols.Add(sentence);
                }
                else
                {
                    symbols.AddRange(GetSymbols(sentence.Requires(sentence)));
                }
            }
            return symbols;
        }

        // given a row, return the next row with updated element values (representing binary counting)
        private List<SentenceElement> NextRow(List<SentenceElement> aRow)
        {
            List<SentenceElement> nextRow = new List<SentenceElement>();
            foreach (SentenceElement symbol in Symbols)
            {
                // check the symbol that exists in row
                if (aRow.Find(x => x.Name == symbol.Name) == 0)
                {
                    // when found and is 0, binary count can be implemented. break.
                    nextRow.Add(symbol, 1);
                    break;
                }
                else // if symbol is 1, previous symbol must have been 1. add 0 and continue to find first 0.
                {
                    nextRow.Add(symbol, 0);
                }
            }
            return nextRow;
        }

        private bool CheckRow(List<SentenceElement> aRow, List<SentenceElement> aKB)
        {
            // check each rule in KB, if any are false, return false
            foreach (SentenceElement rule in aKB)
            {
                // pass in values from model.
                if (rule.Check(aRow) == false)
                {
                    return false;
                }
            }
            return true;
        }

    }
}

*/