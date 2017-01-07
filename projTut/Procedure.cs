using System;
using System.Collections.Generic;

namespace projTut
{
    internal class Procedure
    {
        protected List<Parametre> ListeParametre;
        protected List<Instruction> ListeInstruction;
        protected string Nom;

        public Procedure(List<Parametre> listeParametre, List<Instruction> listeInstruction, string nom)
        {
            ListeParametre = new List<Parametre>(listeParametre);
            ListeInstruction = new List<Instruction>(listeInstruction);
            Nom = nom;
        }



        public Procedure(List<Parametre> listeParametre, string nom) : this(listeParametre, new List<Instruction>(), nom)
        {

        }

        public void InsertInstruction(Instruction instruction)
        {
            ListeInstruction.Add(new Instruction(instruction));
        }

        public Procedure(string line)
        {
            AnalyseProcedure(line);
        }
        
        public override string ToString()
        {
            int i = 1;
            string retour = "Nom de la procédure : " + Nom;
            
            if(ListeParametre != null)
            {
                foreach (Parametre para in ListeParametre)
                {
                    retour += "\nParametre n°" + i + " :";
                    i++;
                    retour += para.ToString();
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
            int i;
            int j = 1;
            ListeParametre = new List<Parametre>();
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
                    ListeParametre.Add(DeterminePara(para1));
                    para1 = "";
                    j++;
                }

                else
                {
                    para1 += para[i];
                }
            }

            ListeParametre.Add(DeterminePara(para1));
            Procedure proc= new Procedure(ListeParametre, DetermineNom(line, debutpara));
            Console.WriteLine(proc);
        }


        protected Parametre DeterminePara(string para)
        {
            Parametre parametre;
            if (para.IndexOf(">(") != -1 || para.IndexOf("<(") != -1 || para.IndexOf("=(") != -1 || para.IndexOf(")>") != -1 || para.IndexOf(")<") != -1 || para.IndexOf(")=") != -1)
            {
                parametre = new Parametre(new TypeElement(TypeEnum.Boolean), Parametre.PassagePar.Valeur, para);
                Instruction ins = new Instruction(para);
                ins.AnalyseInstruction();
            }

            else if (para.IndexOf('(') != -1)
            {
                parametre = new Parametre(new TypeElement(TypeEnum.Unknown), Parametre.PassagePar.Valeur, para);
                Instruction ins = new Instruction(para);
                ins.AnalyseInstruction();
            }

            else
            {
                parametre = new Parametre(new TypeElement(TypeEnum.Unknown), Parametre.PassagePar.Unknown, para);

            }

            return parametre;
        }

        protected string DetermineNom(string proc, int debutpara)
        {

            return proc.Substring(0, debutpara);

        }
    }
}
