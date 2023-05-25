using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    // This abstract class defines how different node types can be interacted with in a similar way.
    public abstract class Operator
    {
        // Symbol is how the node type is recognised as a astring.
        public string Symbol { get; internal set; }
        
        /// <summary>
        /// This function checked if the node requirements have been met and returns true or false.
        /// </summary>
        /// 
        /// <param name="aSentence">The reference to the node that is being checked.</param>
        /// <returns>TRUE or FALSE based on whether requirements are met.</returns>
        public abstract bool Check(SentenceElement aSentence);

        /// <summary>
        /// Requires function reads the contents of aSentenceThis searching for aSentenceAgenda.
        /// If Agenda is null, return all elements.
        /// Otherwise, return required elements.
        /// </summary>
        /// 
        /// <param name="aSentenceAgenda">The node being searched for</param>
        /// <param name="aSentenceThis">reference to the node being searched</param>
        /// <returns>returns the requirements/preconditions for the sentence/node</returns>
        public abstract List<SentenceElement> Requires(SentenceElement aSentenceAgenda, SentenceElement aSentenceThis);

        /// <summary>
        /// Apply will apply the value contained in aSearchForSentence to aTargetSentence.
        /// </summary>
        /// <param name="aSearchForSentence">The parameters of the sentence being searched for, where .Name matches</param>
        /// <param name="aTargetSentence">The sentence which is being searched.</param>
        /// <returns>returns the updated reference to the target sentence.</returns>
        public abstract SentenceElement Apply(SentenceElement aSearchForSentence, SentenceElement aTargetSentence);
    }
}
