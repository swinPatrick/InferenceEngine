using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Not : Operator
    {
        public override bool Check(SentenceElement aSentence)
        {
            // Returns the opposite of the value of the sentence/node
            return aSentence.Value == 0;
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List<SentenceElement> aSentenceRequires)
        {
            List<SentenceElement> output = new List<SentenceElement>();
            if (aSentenceThis.LeftElement.Value == 0)
            {
                output.AddRange(aSentenceThis.Requires(aSentenceAgenda, aSentenceRequires));
            }
            //return aSentenceRequires.AddRange(output);
            return null;
        }

        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            if (aSentenceThis.LeftElement.Apply(aSentenceAgenda) == null)
            {
                return aSentenceThis;
            }
            else return null;
        }
    }
}
