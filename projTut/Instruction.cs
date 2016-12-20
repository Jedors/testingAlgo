using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Instruction
    {

        string line;
        Fonction fonc;
        Procedure proc;


        public Instruction(Instruction instruction)
        {
            //TODO
        }

        public Instruction(string line)
        {
            this.line = line;
        }

        public void DeterminPara()
        {
            int debutpara = line.IndexOf("(");

            if ( debutpara != -1)
            {
                int finpara = line.LastIndexOf(")");
                int length = finpara - debutpara;
                string para = line.Substring(debutpara+1, length-1);
                Console.WriteLine(para);
                string para1 = "";
                Boolean inproc = false;
                Boolean multipara = false;
                int i= 0;
                for (i = 0; i < para.Length; i++)
                {   
                    if (para[i] == '(')
                    {
                        inproc = true;
                    }
                    else if (para[i] == ')')
                    {
                        inproc = false;
                    }


                    if ((para[i] == ',') && (inproc==false))
                    {
                        Console.WriteLine(para1);
                        para1 = "";
                        multipara = true;
                    }
                    else
                    {
                        para1 += para[i];
                    }

                }
                if(multipara)
                {
                    Console.WriteLine(para1);
                }
 
                    
            }
            
        }
    }
}
