using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Not : Operator
    {
        public Not()
        {
            Symbol = "~";
        }

        public override bool Check(SentenceElement aSentence)
        {
            // Returns the opposite of the value of the sentence/node
            return aSentence.Value == 0;
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        {
            List<SentenceElement> lRequired = new List<SentenceElement>();

            // if agenda is null, or this, return itself.
            if (aSentenceAgenda == null ||
                (aSentenceAgenda.Name == aSentenceThis.Name
                && aSentenceAgenda.Operator == aSentenceThis.Operator
                && aSentenceThis.ParentElement != aSentenceThis)) //make sure it's not the root node
                lRequired.Add(aSentenceThis);

            return lRequired;
        }

        public override SentenceElement Apply(SentenceElement aSearchFor, SentenceElement aTarget)
        {
            if (aTarget.Name == aSearchFor.Name)
            {
                aTarget.Value = aSearchFor.Value; // might want to change value to 0.
            }
            if (aTarget.Value == 0)
            {
                return aTarget;
            }

            else return null;
        }
    }
}
