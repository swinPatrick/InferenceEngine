using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public abstract class Operator
    {   
        public abstract bool Check(SentenceElement aSentence);

        public abstract List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis, List<SentenceElement> aSentenceRequirements);

        /// <summary>
        /// Apply will apply the value contained in aSearchForSentence to aTargetSentence.
        /// </summary>
        /// <param name="aSearchForSentence">The parameters of the sentence being searched for, where .Name matches</param>
        /// <param name="aTargetSentence">The sentence which is being searched.</param>
        /// <returns></returns>
        public abstract SentenceElement Apply(SentenceElement aSearchForSentence, SentenceElement aTargetSentence);
    }
}
