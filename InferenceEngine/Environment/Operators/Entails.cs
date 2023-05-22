using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Entails : Operator
    {
        public override bool Check(SentenceElement aSentence)
        {
            // SenteceElement passes in itself. returns value.
            // value on left is true, then right must be true
            if (aSentence.LeftElement.Check() == true
                && aSentence.RightElement.Check() == false)
            {
                return false;
            }
            else { return true; }
        }
        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List<SentenceElement> aSentenceRequirements)
        {
            if (aSentenceThis.RightElement == aSentenceAgenda)
            {
                if (aSentenceThis.Value == 1)
                {
                    return null;
                }
                List <SentenceElement> result = new List<SentenceElement>();
                if (aSentenceThis.LeftElement.Value == 1)
                {
                    aSentenceThis.Value = 1;
                    return null;
                }
                else
                {
                    aSentenceRequirements.AddRange(aSentenceThis.LeftElement.Requires(aSentenceAgenda, aSentenceRequirements));
                    return aSentenceRequirements;
                }
            }
            return null;
        }
        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            if ((aSentenceThis.LeftElement.Apply(aSentenceAgenda) != null))
            {
                return aSentenceThis.RightElement;
            }
            else return null;
        }
    }
}
