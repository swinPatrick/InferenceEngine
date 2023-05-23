using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Entails : Operator
    {
        public Entails()
        {
            Symbol = "=>";
        }

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

        // If the right side is being searched for, return left side.
        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            List<SentenceElement> lRequirements = new List<SentenceElement>();

            // if agenda is null, return all elements on both sides
            if ( aSentenceAgenda ==  null )
            {
                lRequirements.AddRange(aSentenceThis.LeftElement.Requires());
                lRequirements.AddRange(aSentenceThis.RightElement.Requires());
            }
            // otherwise, check if right side is the agenda. if it is, return the left side elements.
            else
            {
                lRequirements = aSentenceThis.RightElement.Requires(aSentenceAgenda);
                if (lRequirements.Count > 0)
                    lRequirements = aSentenceThis.LeftElement.Requires();
            }

            return lRequirements;

            /*
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
            */
        }


        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            bool lLeftChanged = aSentenceThis.LeftElement.Apply(aSentenceAgenda) != null;
            bool lRightChanged = aSentenceThis.RightElement.Apply(aSentenceAgenda) != null;

            if (lLeftChanged)
            {
                return aSentenceThis.RightElement;
            }
            else return null;
        }
    }
}
