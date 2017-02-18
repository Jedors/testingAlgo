using System;
using System.Collections.Generic;

namespace projTut
{
    internal class Instruction
    {
        private readonly string _line;
        Variable AssigneA;

        public Instruction(Instruction instruction)
        {
            //TODO
        }

        public Instruction(string line)
        {
            _line = line;
            AnalyseInstruction();
        }

        public void AnalyseInstruction()
        {       
            string ppv = Properties.Resources.ppv;
            int inproc = 0;
            Boolean proc = false;
            string outil="";
            List<string> listeOutil=new List<string>();
            for (int i = 0; i < _line.Length; i++)
            {
                if (_line[i] == '(')
                {
                    inproc++;
                    if(Char.IsLetter(_line[i-1]))
                    {
                        proc = true;
                    }
                    
                }

                else if (_line[i] == ')')
                {
                    inproc--;
                }

                if ((_line[i] == '+'|| _line[i] == '-' || _line[i] == '*' || _line[i] =='/' || i+1==_line.Length) && (inproc == 0) && proc )
                {
                    
                    outil += _line[i];
                    int debutpara = _line.IndexOf("(");

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
                    outil += _line[i];
                }
            }



          

                  
        }


    }
}
