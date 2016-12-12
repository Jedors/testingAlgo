using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projTut
{
    class Parser
    {
        private string[] pseudocode;
        string ppv = Properties.Resources.ppv;

        private void PrintError(int ligneerreur, string message)
        {
            Console.Error.WriteLine("Erreur à la ligne {0}: {1}", ligneerreur + 1, pseudocode[ligneerreur]);
            Console.Error.WriteLine(message);
        }

        public Parser(string[] pseudocode)
        {
            this.pseudocode = pseudocode;
        }

        // A factoriser plus tard
        public bool isValid()
        {
            //Vérification symbole assignation
            foreach (string line in pseudocode)
            {
                int needleCount = (line.Length - line.Replace(ppv, "").Length) / ppv.Length;
                if (needleCount == 1)
                {
                    if (line.IndexOf(ppv) == 0 || line.IndexOf(ppv) == line.Length - 2)
                    {
                        PrintError(Array.IndexOf(pseudocode, line), "Symbole d'assignation en début ou fin de chaîne");
                        return false;
                    }
                }
                else if (needleCount > 1)
                {
                    PrintError(Array.IndexOf(pseudocode, line), "Double symbole d'assignation");
                    return false;
                }
            }

            //Vérification parenthésage
            int nb_par_open = 0; //nombre de parenthèses ouvertes
            foreach (string line in pseudocode)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if (c == '(') nb_par_open++;
                    if (c == ')')
                    {
                        nb_par_open--;
                        if (nb_par_open < 0)
                        {
                            PrintError(Array.IndexOf(pseudocode, line), "Erreur de parenthesage");
                            return false;
                        }
                        if (i != line.Length - 1)
                        {
                            char suiv = line[i + 1];
                            if (suiv != '*' && suiv != '/' && suiv != '-' && suiv != '+' && suiv != ')'
                                && suiv != ',' && suiv != '<' && suiv != '>' && suiv != '=')
                            {
                                if (!line.Substring(i+1).Contains("donne") || line.Substring(i+1).IndexOf("donne") != 0)
                                {
                                    Console.WriteLine(line.Substring(i+1));
                                    PrintError(Array.IndexOf(pseudocode, line), "Caractère interdit après un ')'");
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (nb_par_open != 0)
                {
                    PrintError(Array.IndexOf(pseudocode, line), "Erreur de parenthesage");
                    return false;
                }
            }

            //Vérif nom variable
            foreach (string line in pseudocode)
            {
                if (line.Contains(ppv))
                {
                    string nom_var = line.Substring(0, line.IndexOf(ppv));
                    if (nom_var[0] < 'a' || nom_var[0] > 'z')
                    {
                        PrintError(Array.IndexOf(pseudocode, line), "Nom de variable invalide");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
