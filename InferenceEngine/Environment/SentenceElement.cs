
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
            if (aOperator == null)
                Operator = new Itself();
        }


        public bool Check()
        {
            return Operator.Check(this);
        }
        

        public List<SentenceElement> Requires(SentenceElement aSentence, List<SentenceElement> aSentenceRequirements)
        {

            return Operator.Requires(aSentence, this, aSentenceRequirements);
        }

        public SentenceElement Apply(SentenceElement aSentenceAgenda)
        {
            return Operator.Apply(aSentenceAgenda, this);
        }

    }

    class SentenceComparer : IEqualityComparer<SentenceElement>
    {
        public bool Equals(SentenceElement x, SentenceElement y)
        {
            // sentences are equal if their name and value are equal
            if(ReferenceEquals(x, y)) return true;

            // check whether any of the compared objects is null
            if(ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            // check whether sentence properties are equal
            return
                x.Name == y.Name &&
                x.Value == y.Value;
        }

        public int GetHashCode(SentenceElement obj)
        {
            // check whether the object is null
            if(obj == null) return 0;

            int hashSentenceName = obj.Name == null ? 0 : obj.Name.GetHashCode();

            int hashSentenceValue = obj.Value.GetHashCode();

            return hashSentenceName ^ hashSentenceValue;
        }
    }
}
