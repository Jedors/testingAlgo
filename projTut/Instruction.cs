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

        public void AnalyseInstruction()
        {

            
            string ppv = Properties.Resources.ppv;
            int debutpara = line.IndexOf("(");
            if ( debutpara != -1)
            {
                Boolean fonc = false;
                int debutfonc= line.IndexOf(ppv);   
                if (debutfonc != -1)
                {
                    string variablenom = line.Substring(0,debutfonc);
                    line = line.Substring(debutfonc+ppv.Length);
                    
                    fonc = true;
                }


                debutpara = line.IndexOf("(");
                int finpara = line.LastIndexOf(")");
                int length = finpara - debutpara;
                Console.WriteLine(line);
                string para = line.Substring(debutpara+1, length-1);
                Console.WriteLine(para);
                string para1 = "";
                int inproc = 0;
                int i= 0;
                int j = 1;
                List<Parametre> listeParametre = new List<Parametre>();
                for (i = 0; i < para.Length; i++)
                {   
                    if (para[i] == '(')
                    {
                        inproc ++;
                    }

                    else if (para[i] == ')')
                    {
                        inproc --;
                    }

                    if ((para[i] == ',') && (inproc==0))
                    {    
                        listeParametre.Add(determinePara(para1));
                        para1 = "";
                        j++;
                    }

                    else
                    {
                        para1 += para[i];
                    }
                }
                
                listeParametre.Add(determinePara(para1));
                proc = new Procedure(listeParametre, determineNom(line, debutpara));
                Console.WriteLine(proc);
            }           
        }



        private Parametre determinePara(string para)
        {
            Parametre parametre=null;
            if (para.IndexOf(">(")!=-1 || para.IndexOf("<(") != -1 || para.IndexOf("=(") != -1 || para.IndexOf(")>") != -1 || para.IndexOf(")<") != -1 || para.IndexOf(")=") != -1)
            {
                parametre = new Parametre(new Type(Type.types.BOOLEAN),Parametre.PassagePar.VALEUR,para);
                Instruction ins = new Instruction(para);
                ins.AnalyseInstruction();
            }

            else if (para.IndexOf('(') != -1)
            {
                parametre = new Parametre(new Type(Type.types.UNKNOWN), Parametre.PassagePar.VALEUR, para);
                Instruction ins = new Instruction(para);
                ins.AnalyseInstruction();
            }

            else
            {
                parametre = new Parametre(new Type(Type.types.UNKNOWN), Parametre.PassagePar.UNKNOWN, para);
 
            }



            return parametre;
        }

        private string determineNom(string proc,int debutpara)
        {

                return proc.Substring(0, debutpara);
            
        }
    }
}
