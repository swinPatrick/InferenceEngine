﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Itself : Operator
    {
        public override bool Check(SentenceElement aSentence)
        {
            // returns the value of the sentence/node
            return aSentence.Value > 0;
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List<SentenceElement> aSentenceRequires)
        { 
            if (aSentenceAgenda == aSentenceThis)
            {
                aSentenceThis.Value = 1;
                return null;
            }
            else
            {
                aSentenceRequires.Add(aSentenceThis);
                return aSentenceRequires;
            }
            
        }

        public override SentenceElement Apply(SentenceElement aSearchFor, SentenceElement aTarget)
        {
            if (aTarget.Name == aSearchFor.Name)
            {
                //aTarget.Value = 1;
                aTarget.Value = aSearchFor.Value; // might want to change value to 0.
            }
            if (aTarget.Value == 1)
            {
                return (aTarget);
            }
            else return null;
        }
    }
}