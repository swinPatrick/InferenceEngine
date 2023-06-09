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
        private List<SentenceElement> symbols;

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
            KB = aKB;
            symbols = GetSymbols(aKB);

            //update KB to be false
            foreach(SentenceElement rule in KB)
            {
                if(rule.Operator.GetType() != typeof(Itself) && rule.Operator.GetType() != typeof(Not))
                    rule.GetSymbols().ForEach(x => x.Value = 0);
            }

            // first row of truth table is all false
            List<SentenceElement> baseRow = new List<SentenceElement>();
            foreach (SentenceElement symbol in symbols)
            {
                baseRow.Add(new SentenceElement(symbol.Name, aValue: 0));
            }

            // create a row for each possible combination of true/false
            // check if row satisfies Rules
            // if so, add to KB
            // else, discard row
            // repeat until all rows have been checked
            // if KB is empty, return false
            // else, return true

            TruthRows = new List<List<SentenceElement>>();

            List<SentenceElement> newRow = new List<SentenceElement>(baseRow);
            
            // the number of possible rows = 2^symbols.
            double rowSum = Math.Pow(2, symbols.Count);
            
            // iterate through possible rows
            for (int i = 0; i < rowSum; i++)
            {
                // check row passes KB
                if (CheckRow(newRow, KB))
                {
                    // add row to truth table
                    TruthRows.Add(newRow.Select(s=> new SentenceElement(s.Name, aValue: s.Value)).ToList());
                }
                // progress to next row to test
                newRow = NextRow(newRow);
            }
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
            // else, if query symbol is not on sybol list, return yes
            if (symbols.Intersect(aQuery).Count() < aQuery.Count)
                return "YES";
            else
                return "NO";
        }


        // given a list of SentenceElements, return a list of SentenceElements that are symbols
        public List<SentenceElement> GetSymbols(List<SentenceElement> aGivenListOfSentences)
        {
            // create a new empty list to hold found symbols
            List<SentenceElement> symbols = new List<SentenceElement>();

            foreach (SentenceElement aSingleSentence in aGivenListOfSentences)
            {
                List<SentenceElement> newSymbols = aSingleSentence.GetSymbols();
                //symbols.AddRange(newSymbols);
                foreach (SentenceElement newSymbol in newSymbols)
                {
                    if (symbols.Any(x => x.Name == newSymbol.Name))
                        continue;
                    symbols.Add(newSymbol);
                }
            }
            return symbols;
        }

        // given a row, return the next row with updated element values (representing binary counting)
        private List<SentenceElement> NextRow(List<SentenceElement> aRow)
        {
            List<SentenceElement> nextRow = aRow.ToList();
            foreach (SentenceElement symbol in symbols)
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
            // dont touch original.
            List<SentenceElement> KB_copy = new List<SentenceElement>(aKB);

            // check each rule in KB, if any are false, return false
            foreach (SentenceElement rule in KB_copy)
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
