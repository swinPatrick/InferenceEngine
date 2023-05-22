using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class And : Operator
    {
        public override bool Check(SentenceElement aSentence)
        {
            // Both left and right elements must be true
            return (aSentence.LeftElement.Check() && aSentence.RightElement.Check());
        }

        public override List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List<SentenceElement> aSentenceRequirements)
        {

            if ((aSentenceThis.LeftElement.Value == 1)&&(aSentenceThis.RightElement.Value == 1))
            {
                aSentenceThis.Value = 1;
            }
            if (aSentenceThis.LeftElement.Value == 0)
            {
                aSentenceRequirements.AddRange(aSentenceThis.LeftElement.Requires(aSentenceAgenda, aSentenceRequirements));
            }
            if (aSentenceThis.RightElement.Value == 0)
            {
                aSentenceRequirements.AddRange(aSentenceThis.RightElement.Requires(aSentenceAgenda, aSentenceRequirements));
            }

            // TODO: Implement And.Requires
            return aSentenceRequirements;
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
