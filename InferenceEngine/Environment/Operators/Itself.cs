using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Itself : Operator
    {
        public Itself()
        {
            Symbol = "";
        }

        public override bool Check(SentenceElement aSentence)
        {
            // returns the value of the sentence/node
            return aSentence.Value == 1;
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis)
        { 
            List<SentenceElement> lRequired = new List<SentenceElement>();
            
            // if agenda is null, or this, return itself.
            if (aSentenceAgenda == null || 
                (aSentenceAgenda.Name == aSentenceThis.Name 
                && aSentenceAgenda.Operator.GetType() == aSentenceThis.Operator.GetType()
                //&& aSentenceThis.ParentElement != aSentenceThis //make sure it's not the root node
                ))
                lRequired.Add(aSentenceThis);
            
            return lRequired;

        }

        public override SentenceElement Apply(SentenceElement aSearchFor, SentenceElement aTarget)
        {
            // if this is the node being searched for, updae the value. if the value is 1, return itself.
            if (aTarget.Name == aSearchFor.Name)
            {
                aTarget.Value = aSearchFor.Value; // might want to change value to 0.
            }
            if(aTarget.Value == 1)
            {
                return aTarget;
            }

            else return null;
        }
    }
}
