using System;
using System.Text.RegularExpressions;

namespace analysePseudoCode
{
    internal class Parser
    {
        private readonly string[] _pseudocode; // Pseudo-code à analyser
        private readonly string _ppv = Properties.Resources.ppv; // Alias de ppv dans les ressources

        /// <summary>
        /// Affiche une erreur
        /// </summary>
        /// <param name="ligneerreur">Id de la ligne contenant l'erreur</param>
        /// <param name="message">Message d'erreur à afficher</param>
        private void PrintError(int ligneerreur, string message)
        {
            Console.Error.WriteLine("Erreur à la ligne {0}: {1}", ligneerreur + 1, _pseudocode[ligneerreur]);
            Console.Error.WriteLine(message);
        }

        /// <summary>
        /// Contructeur par défaut, rempli avec le code
        /// </summary>
        /// <param name="pseudocode">Code à analyser</param>
        public Parser(string[] pseudocode)
        {
            _pseudocode = pseudocode;
        }

        // A factoriser plus tard + Documenter
        /// <summary>
        /// Renvoie un booléen si le code est valide ou non
        /// </summary>
        /// <returns>True si valide</returns>
        public bool IsValid()
        {
            //Vérification symbole assignation
            foreach (string line in _pseudocode)
            {
                //Si affiche next ite;
                Regex affRegex = new Regex(@"^affiche\([a-z]+\)donne((\d+)|(\d+\.\d+)|(vrai)|(faux))$");
                Regex keywordRegex = new Regex(@"(([^a-z]vrai[^a-z])|([^a-z]faux[^a-z])|([^a-z]affiche[^a-z])|([^a-z]donne[^a-z]))");
                if (affRegex.IsMatch(line))
                    continue;
                if (keywordRegex.IsMatch(line))
                {
                    PrintError(Array.IndexOf(_pseudocode, line), "Vrai, faux, affiche, et donne sont des mots-clés interdit");
                    return false;
                }

                
                int needleCount = (line.Length - line.Replace(_ppv, "").Length) / _ppv.Length;
                if (needleCount == 1)
                {
                    if (line.IndexOf(_ppv, StringComparison.Ordinal) == 0 ||
                        line.IndexOf(_ppv, StringComparison.Ordinal) == line.Length - 2)
                    {
                        PrintError(Array.IndexOf(_pseudocode, line), "Symbole d'assignation en début ou fin de chaîne");
                        return false;
                    }
                }
                else if (needleCount > 1)
                {
                    PrintError(Array.IndexOf(_pseudocode, line), "Double symbole d'assignation");
                    return false;
                }
            }

            //Vérification parenthésage
            int nbParOpen = 0; //nombre de parenthèses ouvertes
            foreach (string line in _pseudocode)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if (c == '(')
                    {
                        if (i == line.Length - 1)
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "'(' en fin de ligne");
                            return false;
                        }
                        nbParOpen++;
                        char prec = line[i - 1], suiv = line[i + 1];
                        if (((prec < 'a') || (prec > 'z')) && (prec != '-') && (prec != '+') && (prec != '*') && (prec != '/') && (prec != '=')
                            && (prec != '>') && (prec != '<'))
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "Caractère interdit avant un '('");
                            return false;
                        }
                        if ((suiv < 'a') && (suiv > 'z') && (suiv < '0') && (suiv > '9') && (suiv != '-') && (suiv != '('))
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "Caractère interdit après un '('");
                            return false;
                        }
                    }
                    if (c == ')')
                    {
                        nbParOpen--;
                        if (nbParOpen < 0)
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "Erreur de parenthesage");
                            return false;
                        }
                        if (i != line.Length - 1)
                        {
                            char suiv = line[i + 1];
                            if ((suiv != '*') && (suiv != '/') && (suiv != '-') && (suiv != '+') && (suiv != ')')
                                && (suiv != ',') && (suiv != '<') && (suiv != '>') && (suiv != '='))
                            {
                                if (!line.Substring(i + 1).Contains("donne") || line.Substring(i + 1)
                                    .IndexOf("donne", StringComparison.Ordinal) != 0)
                                {
                                    PrintError(Array.IndexOf(_pseudocode, line), "Caractère interdit après un ')'");
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (nbParOpen != 0)
                {
                    PrintError(Array.IndexOf(_pseudocode, line), "Erreur de parenthesage");
                    return false;
                }
            }

            //Vérif nom variable
            foreach (string line in _pseudocode)
            {
                if (line.Contains(_ppv))
                {
                    string nomVar = line.Substring(0, line.IndexOf(_ppv, StringComparison.Ordinal));
                    if (nomVar[0] < 'a' || nomVar[0] > 'z')
                    {
                        PrintError(Array.IndexOf(_pseudocode, line), "Nom de variable invalide");
                        return false;
                    }
                }
            }

            //Vérification paramètre non vide
            foreach (string line in _pseudocode)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if (c == ',')
                    {
                        for (int j = i - 1; j < i + 2; j += 2)
                        {
                            char ch = line[j];
                            if ((ch < 'a') && (ch > 'z') && (ch < '0') && (ch > '9') && (ch != '('))
                            {
                                if ((j != i + 1) || (ch != '-'))
                                {
                                    PrintError(Array.IndexOf(_pseudocode, line), "Paramètre invalide");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            // Verif if only var name before ppv
            foreach (string line in _pseudocode)
            {
                int index = line.IndexOf(_ppv, StringComparison.Ordinal);
                if (index != -1)
                {
                    string firstElement = line.Substring(0, index);
                    Regex varValid = new Regex(@"^[a-z]+$");
                    if (!varValid.IsMatch(firstElement))
                    {
                        PrintError(Array.IndexOf(_pseudocode, line), "Nom de variable invalide");
                        return false;
                    }
                }
            }
            
            

            return true;
        }
    }
}