using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projTut
{
    class Procedure
    {
        protected List<Parametre> listeParametre;
        protected List<Instruction> listeInstruction;
        protected string nom;

        public Procedure(List<Parametre> listeParametre, List<Instruction> listeInstruction, string nom)
        {
            this.listeParametre = new List<Parametre>(listeParametre);
            this.listeInstruction = new List<Instruction>(listeInstruction);
            this.nom = nom;
        }



        public Procedure(List<Parametre> listeParametre, string nom) : this(listeParametre, new List<Instruction>(), nom)
        {

        }

        public void insertInstruction(Instruction instruction)
        {
            listeInstruction.Add(new Instruction(instruction));
        }

        public Procedure(string line)
        {
            AnalyseProcedure(line);
        }
        
        public override string ToString()
        {
            int i = 1;
            string retour;
            retour = "Nom de la procédure : " + this.nom;
            
            if(listeParametre != null)
            {
                foreach (Parametre Para in listeParametre)
                {
                    retour += "\nParametre n°" + i + " :";
                    i++;
                    retour += Para.ToString();
                    retour += "\n";
                } 
            }
            return retour;
        }

        private void AnalyseProcedure(string line)
        {
            int debutpara = line.IndexOf("(");
            int finpara = line.LastIndexOf(")");
            int length = finpara - debutpara;
            string para = line.Substring(debutpara + 1, length - 1);
            string para1 = "";
            int inproc = 0;
            int i = 0;
            int j = 1;
            listeParametre = new List<Parametre>();
            for (i = 0; i < para.Length; i++)
            {
                if (para[i] == '(')
                {
                    inproc++;
                }

                else if (para[i] == ')')
                {
                    inproc--;
                }

                if ((para[i] == ',') && (inproc == 0))
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
            Procedure proc= new Procedure(listeParametre, determineNom(line, debutpara));
            Console.WriteLine(proc);
        }


        protected Parametre determinePara(string para)
        {
            Parametre parametre = null;
            if (para.IndexOf(">(") != -1 || para.IndexOf("<(") != -1 || para.IndexOf("=(") != -1 || para.IndexOf(")>") != -1 || para.IndexOf(")<") != -1 || para.IndexOf(")=") != -1)
            {
                parametre = new Parametre(new Type(Type.types.BOOLEAN), Parametre.PassagePar.VALEUR, para);
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

        protected string determineNom(string proc, int debutpara)
        {

            return proc.Substring(0, debutpara);

        }
    }
}
