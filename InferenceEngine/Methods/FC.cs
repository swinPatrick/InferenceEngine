using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine.Algorithms
{
    internal class FC
    {
        private List<SentenceElement> KB;
        private Queue<SentenceElement> Agenda;
        private List<SentenceElement> Query;
        private List<SentenceElement> Inferred = new List<SentenceElement>();

        public FC()
        {
            KB = new List<SentenceElement>();
            Agenda = new Queue<SentenceElement>();
            Query = new List<SentenceElement>();
        }

        public void Tell(List<SentenceElement> aKB, SentenceElement aQuery)
        {
            Agenda.Clear();
            KB = aKB;
            foreach (SentenceElement knowledge in aKB)
            {
                if (!knowledge.Name.Contains("~") && !knowledge.Name.Contains("||") &&
                    !knowledge.Name.Contains("=>") && !knowledge.Name.Contains("&"))
                {
                    Agenda.Enqueue(knowledge);
                }
            }
            Query.Add(aQuery);

        }

        public string Ask()
        {

            Inferred.Clear();
            /*
             * while (Agenda.Count != 0)
            {
                SentenceElement s = Agenda.Dequeue();
                foreach (SentenceElement q in Query)
                {
                    if (s.Name == q.Name)
                    {
                        return "Completed s already in knowledge base at start";
                    }
                }
            }
            */

            SentenceElement result = null;
            while (Agenda.Count() != 0)
            {
                SentenceElement a_item = Agenda.Dequeue();
                foreach (SentenceElement knowledge in KB)
                {
                    result = knowledge.Apply(a_item);
                    if (result != null)
                    {
                        foreach (SentenceElement q in Query)
                        {
                            if (result.Name == q.Name)
                            {
                                string output = "";
                                Inferred.Add(result);
                                foreach (SentenceElement s in Inferred)
                                {
                                    if (output == "")
                                    {
                                        output = output + s.Name;
                                    }
                                    else { output = output + ", " + s.Name; }
                                }
                                return "YES: " + output;
                            }
                        }
                        if (!Inferred.Contains(result))
                        {   
                            Inferred.Add(result);
                            Agenda.Enqueue(result);
                        }
                    }
                }
            }
            return "NO";

        }
    }
}

