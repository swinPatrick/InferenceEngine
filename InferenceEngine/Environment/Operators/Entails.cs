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
        }


        public override SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            // search both sides fo whether they are the sentence item being looked for, if left has been updated, return tight side.
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
