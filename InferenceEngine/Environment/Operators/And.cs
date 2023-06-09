﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class And : Operator
    {
        public And() 
        {
            Symbol = "&";
        }

        public override bool Check(SentenceElement aSentence)
        {
            // if any child is null, return false
            if (aSentence.LeftElement == null ||
                aSentence.RightElement == null)
                return false;

            // Both left and right elements must be true
            return (aSentence.LeftElement.Check() && aSentence.RightElement.Check());
            
            //return (aSentence.LeftElement.Check() && aSentence.RightElement.Check());
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            List<SentenceElement> lRequires = new List<SentenceElement>();

            // search both children for the required agenda (null will pass along if appropirate).
            
            lRequires.AddRange(aSentenceThis.LeftElement.Requires(aSentenceAgenda));
            lRequires.AddRange(aSentenceThis.RightElement.Requires(aSentenceAgenda));
            

            return lRequires;
        }

        /// <summary>
        /// Passes Apply along to children. updates itself.
        /// </summary>
        /// <param name="aSentenceAgenda">The node being searched for</param>
        /// <param name="aSentenceThis">the respective node</param>
        /// <returns></returns>
        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            bool lLeftChanged = aSentenceThis.LeftElement.Apply(aSentenceAgenda) != null;
            bool lRightChanged = aSentenceThis.RightElement.Apply(aSentenceAgenda) != null;

            if (lLeftChanged && lRightChanged)
            {
                    return aSentenceThis;
            }
            else return null;
        }
    }
}
