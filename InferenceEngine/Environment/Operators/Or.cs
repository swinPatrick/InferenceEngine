using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Or : Operator
    {
        public Or()
        {
            Symbol = "|";
        }

        public override bool Check(SentenceElement aSentence)
        {
            // returns true if either left or right element is true

            return (aSentence.LeftElement.Check() || aSentence.RightElement.Check());
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            List<SentenceElement> lRequires = new List<SentenceElement>();

            // search both children for the required agenda (null will pass along if appropirate).
            // if null, forward to both children
            if(aSentenceAgenda ==  null)
            {
                lRequires.AddRange(aSentenceThis.LeftElement.Requires());
                lRequires.AddRange(aSentenceThis.RightElement.Requires());
            }
            // otherwise, forward individually
            if(
                aSentenceThis.LeftElement.Requires(aSentenceAgenda).Count() > 0 ||
                aSentenceThis.RightElement.Requires(aSentenceAgenda).Count > 0)
            {
                // becuase of OR in IF, only one will be populated.
                lRequires.AddRange(aSentenceThis.LeftElement.Requires(aSentenceAgenda));
                lRequires.AddRange(aSentenceThis.RightElement.Requires(aSentenceAgenda));
            }

            return lRequires;
        }

        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            // searches both children for agenda that is being searched for.
            bool lLeftChanged = aSentenceThis.LeftElement.Apply(aSentenceAgenda) != null;
            bool lRightChanged = aSentenceThis.RightElement.Apply(aSentenceAgenda) != null;

            if ((lLeftChanged || lRightChanged))
            {
                return aSentenceThis;
            }
            else return null;
        }
    }
}
