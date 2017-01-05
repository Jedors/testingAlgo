using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace projTut
{
    class Program
    {
        private static List<Variable> listeVariable = new List<Variable>();     // Liste des variables dans le code
        private static List<Procedure> listeProcedure = new List<Procedure>();  // Liste des procédures et fonctions


        /// <summary>
        /// Try to insert a Variable into listeParametre.
        /// If already existing -> Updating the one into the list.
        /// </summary>
        /// <param name="var">Variable to insert</param>
        /// <returns>1 if inserted, 2 if updated, -1 if Type error</returns>
        public static int InsertVariable(Variable var)
        {
            foreach(Variable v in listeVariable)
            {
                if (v.getNom() == var.getNom())
                {
                    if (v.getType().type == Type.types.UNKNOWN)
                    {
                        v.getType().type = var.getType().type;
                    }
                    else if (v.getType().type != v.getType().type)
                    {
                        return -1;
                    }
                    var.setVar(v);
                    return 2;
                }
            }
            listeVariable.Add(new Variable(var));
            return 1;
        }


        /// <summary>
        /// Fonction principale lol
        /// </summary>
        /// <param name="args">Les arguments/noms de fichier à analyser</param>
        /// <returns>-1 si erreur</returns>
        static int Main(string[] args)
        {
            string fileName = "", appName = Properties.Resources.appName; // appName alias pour ressources

            // Vérification nombre d'arguments
            if(args.Length != 1)
            {
                Console.Error.WriteLine("Un et un seul argument requis");
                Console.Error.WriteLine("Consultez {0} --help", appName);
                return -1;
            }

            fileName = args[0]; // Assignarion nom de fichier
            string[] pseudocode; // Pseudo-code à analyser

            if (!File.Exists(fileName)) // Si le fichier n'existe pas...
            {
                Console.Error.WriteLine("{0}: Fichier inexistant", fileName);
                return -1;
            }
            else
            {
                FileIOPermission f = null;

                try // Essaye de voir si il y a les permissions pour lire le fichier
                {
                    f = new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(fileName));
                    f.Demand();
                    pseudocode = File.ReadAllLines(fileName); // Lis le fichier
                }
                catch (SecurityException e)
                {
                    Console.Error.WriteLine("{0}: {1}", appName, e.Message);
                    return -1;
                }
            }

            // Mise en forme du code (Minuscule + suppresion espaces)
            for (int i = 0; i < pseudocode.Length; i++)
            {
                pseudocode[i] = pseudocode[i].ToLower();
                pseudocode[i] = Regex.Replace(pseudocode[i], @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();
                pseudocode[i] = pseudocode[i].Replace(" ", string.Empty);
            }

            Parser pars = new Parser(pseudocode);
            if (!pars.isValid()) // Si code non valide -> Erreur
            {
                Console.Error.WriteLine("Analyse abbandonnée");
                Console.ReadKey();
                return -1;
            }

            Console.WriteLine(args[0]);
            foreach (string line in pseudocode)
            {
                Console.WriteLine(line);
            }

            foreach (string line in pseudocode)
            {
                Instruction ins = new Instruction(line);
      


            }
            

            Console.ReadKey();
            Console.WriteLine();
            return 0;
        }
    }
}
