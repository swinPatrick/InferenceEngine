using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Or : Operator
    {
        public override bool Check(SentenceElement aSentence)
        {
            // returns true if either left or right element is true
            return (aSentence.LeftElement.Check() || aSentence.RightElement.Check());
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List <SentenceElement> aSentenceRequires)
        {
            List<SentenceElement> result = new List<SentenceElement>();
            if (aSentenceThis.LeftElement.Requires(aSentenceAgenda, aSentenceRequires) != null)
            {
                result.AddRange(aSentenceThis.LeftElement.Requires(aSentenceAgenda, aSentenceRequires));
            }
            if (aSentenceThis.RightElement.Requires(aSentenceAgenda, aSentenceRequires) != null)
            {
                result.AddRange(aSentenceThis.RightElement.Requires(aSentenceAgenda, aSentenceRequires));                       
            }
            if (result.Count > 0)
            {
                return result;
            }
            else return null;
        }

        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
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
