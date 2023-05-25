
﻿using InferenceEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class SentenceElement
    {
        public string Name { get; } // Contains either the symbol that represents the content of the node.
        public Operator Operator { get; set; }  // defines the type of functionality held by a node.
        public int Value { get; set; }  // value of a given node.
        
        // Nodes exist in a tree structure that can be navigated. with a Parent, a Left child, and a right Child.
        public SentenceElement LeftElement { get; set; }
        public SentenceElement RightElement { get; set; }
        public SentenceElement ParentElement { get; set; }


        public SentenceElement(string aName, Operator aOperator = null, int aValue = 0)
        {
            Name = aName;
            Operator = aOperator;
            Value = aValue;
            LeftElement = this;
            RightElement = this;
            ParentElement = this;
            if (aOperator == null)
                Operator = new Itself();
        }

        // Convienient functions to use the respective operator functionality
        public bool Check() { return Operator.Check(this); }
        public List<SentenceElement> Requires(SentenceElement aSentence = null) { return Operator.Requires(aSentence, this); }
        public SentenceElement Apply(SentenceElement aSentenceAgenda) { return Operator.Apply(aSentenceAgenda, this); }

        // returns a list of symbols which are below this one in the tree.
        public List<SentenceElement> GetSymbols(SentenceElement aSentence = null)
        {
            // search itself by default.
            if (aSentence == null)
                aSentence = this;

            // empty list of symbols. list will be filled with symbols in the sentence tree.
            List<SentenceElement> symbols = new List<SentenceElement>();

            // Given a sentenceElement, if it is a leaf then it is the only symbol.
            if (aSentence.Operator is Itself || aSentence.Operator is Not)
            {
                symbols.Add(aSentence);
            }
            else
            {
                // if the operator of sentence isn't a lead type, it will have left and right children. 
                symbols.AddRange(GetSymbols(aSentence.LeftElement));
                symbols.AddRange(GetSymbols(aSentence.RightElement));
            }
            return symbols;
        }
        
        public override string ToString()
        {
            return (Value == 1 ? "" : "~") + Name;
        }
    }
}
