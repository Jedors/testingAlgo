using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projTut
{
    class Instruction
    {
        string Line;
        Variable AssigneA;

        public Instruction(Instruction instruction)
        {
            //TODO
        }

        public Instruction(string line)
        {
            this.Line = line;
            AnalyseInstruction();
        }

        public void AnalyseInstruction()
        {       
            string ppv = Properties.Resources.ppv;
            int debutpara = Line.IndexOf("(");
            if ( debutpara != -1)
            {
                Boolean fonc = false;
                int debutfonc= Line.IndexOf(ppv);
                if (debutfonc != -1)
                {
                    string variablenom = Line.Substring(0, debutfonc);
                    Line = Line.Substring(debutfonc + ppv.Length);
                    fonc = true;
                    new Fonction(Line);
                }



                else
                {
                    Console.WriteLine(Line);
                    new Procedure(Line);
                }
            }

                  
        }


    }
}
