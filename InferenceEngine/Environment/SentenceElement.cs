
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

        public string Name { get; }
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
            LeftElement = this;
            RightElement = this;
            ParentElement = this;
        }


        public bool Check()
        {
            return Operator.Check(this);
        }
        

        public List<SentenceElement> Requires(SentenceElement aSentence, List<SentenceElement> aSentenceRequirements)
        {

            return this.Operator.Requires(aSentence, this, aSentenceRequirements);
        }

        public SentenceElement Apply(SentenceElement aSentenceAgenda)
        {
            return this.Operator.Apply(aSentenceAgenda, this);
        }

    }
}
