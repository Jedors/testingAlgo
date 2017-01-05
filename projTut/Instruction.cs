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
            int inproc = 0;
            Boolean proc = false;
            string outil="";
            List<string> listeOutil=new List<string>();
            for (int i = 0; i < Line.Length; i++)
            {
                if (Line[i] == '(')
                {
                    inproc++;
                    if(Char.IsLetter(Line[i-1]))
                    {
                        proc = true;
                    }
                    
                }

                else if (Line[i] == ')')
                {
                    inproc--;
                }

                if ((Line[i] == '+'|| Line[i] == '-' || Line[i] == '*' || Line[i] =='/' || i+1==Line.Length) && (inproc == 0) && proc )
                {
                    
                    outil += Line[i];
                    int debutpara = Line.IndexOf("(");

                    if (debutpara != -1)
                    {

                        int debutfonc = outil.IndexOf(ppv);
                        if (debutfonc != -1)
                        {
                            string variablenom = outil.Substring(0, debutfonc);
                            outil = outil.Substring(debutfonc + ppv.Length);
                            new Procedure(outil);
                        }



                        else
                        {
                            Console.WriteLine(outil);
                            new Procedure(outil);
                        }
                    }
                    outil = "";
                    proc = false;

                }

                else
                {
                    outil += Line[i];
                }
            }



          

                  
        }


    }
}
