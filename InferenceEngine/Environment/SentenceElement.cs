
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

        public string Name { get; set; }
        public Operator Operator { get; set; }
        public int Value { get; set; }
        public SentenceElement LeftElement { get; set; }
        public SentenceElement RightElement { get; set; }
        public SentenceElement ParentElement { get; set; }


        public SentenceElement(string aName, Operator aOperator = null, int aValue = 0)
        {
            Name = aName;
            Operator = aOperator;
            Value = aValue;
            LeftElement = null;
            RightElement = null;
            ParentElement = this;
            if (aOperator == null)
                Operator = new Itself();
        }


        public bool Check()
        {
            return Operator.Check(this);
        }
        

        public List<SentenceElement> Requires(SentenceElement aSentence = null)
        {

            return Operator.Requires(aSentence, this);
        }

        public SentenceElement Apply(SentenceElement aSentenceAgenda)
        {
            return Operator.Apply(aSentenceAgenda, this);
        }

        public SentenceElement GetRoot()
        {
            SentenceElement root = this;
            while (root.ParentElement != root)
            {
                root = root.ParentElement;
            }
            return root;
        }

        public List<SentenceElement> GetSymbols(SentenceElement aSentence = null)
        {
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
            string s = "";
            if( Operator is Itself ||
                Operator is Not)
                s = (Value == 1 ? "" : "~") + Name;
            else
            {
                s = "(" + LeftElement.ToString() + Operator.Symbol + RightElement.ToString() + ")";
            }
            return s;
        }

    }

}
