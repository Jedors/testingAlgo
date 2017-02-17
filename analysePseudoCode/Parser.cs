using System;
using System.Text.RegularExpressions;

namespace analysePseudoCode
{
    /// <summary>
    /// Parser to check the code syntax
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Pseudo-code to analyse
        /// </summary>
        private readonly string[] _pseudocode;
        /// <summary>
        /// Alias of ppv in the ressource
        /// </summary>
        private readonly string _ppv = Properties.Resources.ppv;

        /// <summary>
        /// Print an error
        /// </summary>
        /// <param name="lineerror">Line ID containing the error</param>
        /// <param name="message">Error message to print</param>
        private void PrintError(int lineerror, string message)
        {
            Console.Error.WriteLine("Error at line {0}: {1}", lineerror + 1, _pseudocode[lineerror]);
            Console.Error.WriteLine(message);
        }

        /// <summary>
        /// Default contructor, fill with the code
        /// </summary>
        /// <param name="pseudocode">Code to analyse</param>
        public Parser(string[] pseudocode)
        {
            _pseudocode = pseudocode;
        }

        // To factorise later
        /// <summary>
        /// Return a boolean if the code is or not valid
        /// </summary>
        /// <returns>True if valid</returns>
        public bool IsValid()
        {
            //Verification assignation symbole
            foreach (string line in _pseudocode)
            {
                //If affiche next ite;
                Regex affRegex = new Regex(@"^affiche\([a-z]+\)donne((\d+)|(\d+\.\d+)|(vrai)|(faux))$");
                Regex keywordRegex = new Regex(@"(([^a-z]vrai[^a-z])|([^a-z]faux[^a-z])|([^a-z]affiche[^a-z])|([^a-z]donne[^a-z]))");
                if (affRegex.IsMatch(line))
                    continue;
                if (keywordRegex.IsMatch(line))
                {
                    PrintError(Array.IndexOf(_pseudocode, line), "Vrai, faux, affiche, and donne are reservated keyword");
                    return false;
                }

                
                int needleCount = (line.Length - line.Replace(_ppv, "").Length) / _ppv.Length;
                if (needleCount == 1)
                {
                    if (line.IndexOf(_ppv, StringComparison.Ordinal) == 0 ||
                        line.IndexOf(_ppv, StringComparison.Ordinal) == line.Length - 2)
                    {
                        PrintError(Array.IndexOf(_pseudocode, line), "Assignation symbole at beginning or end of string");
                        return false;
                    }
                }
                else if (needleCount > 1)
                {
                    PrintError(Array.IndexOf(_pseudocode, line), "Double assignation symbole");
                    return false;
                }
            }

            //"Vérification parenthésage"
            int nbParOpen = 0; //number of "parenthèses" open
            foreach (string line in _pseudocode)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if (c == '(')
                    {
                        if (i == line.Length - 1)
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "'(' in end of line");
                            return false;
                        }
                        nbParOpen++;
                        char prec = line[i - 1], suiv = line[i + 1];
                        if (((prec < 'a') || (prec > 'z')) && (prec != '-') && (prec != '+') && (prec != '*') && (prec != '/') && (prec != '=')
                            && (prec != '>') && (prec != '<'))
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "Forbidden character before '('");
                            return false;
                        }
                        if ((suiv < 'a') && (suiv > 'z') && (suiv < '0') && (suiv > '9') && (suiv != '-') && (suiv != '('))
                        {
                            PrintError(Array.IndexOf(_pseudocode, line), "Forbidden character after '('");
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
                                    PrintError(Array.IndexOf(_pseudocode, line), "Forbidden character after ')'");
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

            //Verification variable name
            foreach (string line in _pseudocode)
            {
                if (line.Contains(_ppv))
                {
                    string nomVar = line.Substring(0, line.IndexOf(_ppv, StringComparison.Ordinal));
                    if (nomVar[0] < 'a' || nomVar[0] > 'z')
                    {
                        PrintError(Array.IndexOf(_pseudocode, line), "Variable name not correct");
                        return false;
                    }
                }
            }

            //Verification parameter not empty
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
                                    PrintError(Array.IndexOf(_pseudocode, line), "Invalid parameter");
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
                        PrintError(Array.IndexOf(_pseudocode, line), "Variable name invalid");
                        return false;
                    }
                }
            }
            

            return true;
        }
    }
}