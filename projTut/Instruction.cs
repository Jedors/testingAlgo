﻿using System;
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

        public void ListePara(string line)
        {

            int debutpara = line.IndexOf("(");
           
            if ( debutpara != -1)
            {
                
                int finpara = line.LastIndexOf(")");
                int length = finpara - debutpara;
                string para = line.Substring(debutpara+1, length-1);

                string para1 = "";
                Boolean inproc = false;
                int i= 0;
                int j = 1;
                List<Parametre> listeParametre = new List<Parametre>();
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
                proc.affiche();
            }           
        }



        private Parametre determinePara(string para)
        {
            Parametre parametre=null;
            if (para.IndexOf(">(")!=-1 || para.IndexOf("<(") != -1 || para.IndexOf("=(") != -1 || para.IndexOf(")>") != -1 || para.IndexOf(")<") != -1 || para.IndexOf(")=") != -1)
            {
                parametre = new Parametre(new Type(Type.types.BOOLEAN),Parametre.PassagePar.VALEUR,para);
                ListePara(para);
            }

            else if (para.IndexOf('(') != -1)
            {
                parametre = new Parametre(new Type(Type.types.UNKNOWN), Parametre.PassagePar.VALEUR, para);
                ListePara(para);
            }
            else
            {
                parametre = new Parametre(new Type(Type.types.UNKNOWN), Parametre.PassagePar.UNKNOWN, para);
                ListePara(para);
            }



            return parametre;
        }

        private string determineNom(string proc,int debutpara)
        {
            int debutnom = proc.IndexOf(Properties.Resources.ppv);
            
            if (debutnom != -1){
                return proc.Substring(debutnom + Properties.Resources.ppv.Length, debutpara - debutnom - Properties.Resources.ppv.Length);

            }
            else
            {
                return proc.Substring(0, debutpara);
            }
        }
    }
}
