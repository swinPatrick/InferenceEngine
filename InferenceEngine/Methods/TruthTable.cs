﻿

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

        // Model is a row of the truthTable.
        private List<SentenceElement> TruthRow;
        private List<List<SentenceElement>> TruthRows;

        // Constructor
        public TruthTable()
        {
            Name = "TT";
            KB = new List<SentenceElement>();
            Query = new List<SentenceElement>();
        }

        public override void Tell(List<SentenceElement> aKB)
        {
            Symbols = GetSymbols(aKB);

            // first row of truth table is all false
            TruthRow = new List<SentenceElement>();
            foreach (SentenceElement symbol in Symbols)
            {
                TruthRow.Add(new SentenceElement(symbol.Name, aValue: 0));
            }

            // create a row for each possible combination of true/false
            // check if row satisfies Rules
            // if so, add to KB
            // else, discard row
            // repeat until all rows have been checked
            // if KB is empty, return false
            // else, return true

            TruthRows = new List<List<SentenceElement>>();

            for (int i = 0; i < Math.Pow(2, Symbols.Count); i++)
            {
                if (CheckRow(TruthRow, aKB))
                {
                    TruthRows.Add(TruthRow);
                }
                TruthRow = NextRow(TruthRow);
            }

            // KnowledgeBase is now a list of all rows that satisfy KB
        }

        public override string Ask(List<SentenceElement> aQuery)
        {
            Query.AddRange(aQuery);
            // count how many rows of KB satisfy Query
            // if count is 0, return false
            // else, return true
            int count = 0;
            foreach (List<SentenceElement> row in TruthRows)
            {
                if (CheckRow(row, Query))
                {
                    count++;
                }
            }

            // return number of rows that satisfy Query (as sentence element with name as count)
            if (count > 0)
                return string.Format("YES: {0}", count);
            else 
                return "NO";
        }

        // given a list of SentenceElements, return a list of SentenceElements that are symbols
        private List<SentenceElement> GetSymbols(List<SentenceElement> aGivenListOfSentences)
        {
            // create a new empty list to hold found symbols
            List<SentenceElement> symbols = new List<SentenceElement>();

            foreach (SentenceElement aSingleSentence in aGivenListOfSentences)
            {
                List<SentenceElement> newSymbols = GetSymbols(aSingleSentence);
                symbols.AddRange(newSymbols.Distinct());
            }

            return symbols;
        }

        private List<SentenceElement> GetSymbols(SentenceElement aSentence)
        {
            // empty list of symbols. list will be filled with symbols in the sentence tree.
            List<SentenceElement> symbols = new List<SentenceElement>();

            // Given a sentenceElement, if it is a leaf then it is the only symbol.
            if(aSentence.Operator is Itself)
            {
                symbols.Add(aSentence);
            }
            else
            {
                // if the operator of sentence isn't a lead type, it will have left and right children. 
                Symbols.AddRange(GetSymbols(aSentence.LeftElement));
                Symbols.AddRange(GetSymbols(aSentence.RightElement));
            }
            return symbols;

        }

        // given a row, return the next row with updated element values (representing binary counting)
        private List<SentenceElement> NextRow(List<SentenceElement> aRow)
        {
            List<SentenceElement> nextRow = new List<SentenceElement>(aRow);
            foreach (SentenceElement symbol in Symbols)
            {
                // check the symbol that exists in row
                SentenceElement respectiveSymbolInRow = nextRow.Find(s=> s.Name == symbol.Name);
                if (respectiveSymbolInRow.Value == 0)
                {
                    // when found and is 0, binary count can be implemented. break.
                    respectiveSymbolInRow.Value = 1;
                    break;
                }
                else // if symbol is 1, previous symbol must have been 1. add 0 and continue to find first 0.
                {
                    respectiveSymbolInRow.Value = 0;
                }
            }
            return nextRow;
        }

        private bool CheckRow(List<SentenceElement> aRow, List<SentenceElement> aKB)
        {
            // check each rule in KB, if any are false, return false
            foreach (SentenceElement rule in aKB)
            {
                // Apply values in row to KB
                foreach(SentenceElement symbol in aRow)
                {
                    rule.Apply(symbol);
                }

                // make sure rule is satisfied.
                if(rule.Check() is false)
                    return false;
            }
            // once all rules have been checked, if no rule.Check was false, row is good.
            return true;
        }

    }
}