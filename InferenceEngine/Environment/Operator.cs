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

        public abstract SentenceElement Apply(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis);
    }
}
