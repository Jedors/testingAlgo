using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace projTut
{
    internal class Program
    {
        private static readonly List<Variable> ListeVariable = new List<Variable>();     // Liste des variables dans le code
        private static List<Procedure> _listeProcedure = new List<Procedure>();  // Liste des procédures et fonctions


        /// <summary>
        /// Try to insert a Variable into listeParametre.
        /// If already existing -> Updating the one into the list.
        /// </summary>
        /// <param name="var">Variable to insert</param>
        /// <returns>1 if inserted, 2 if updated, -1 if Type error</returns>
        public static int InsertVariable(Variable var)
        {
            foreach(Variable v in ListeVariable)
            {
                if (v.Nom == var.Nom)
                {
                    if (v.Type.Type == TypeEnum.Unknown)
                    {
                        v.Type.Type = var.Type.Type;
                    }
                    else if (v.Type.Type != var.Type.Type)
                    {
                        return -1;
                    }
                    var.SetVar(v);
                    return 2;
                }
            }
            ListeVariable.Add(new Variable(var));
            return 1;
        }


        /// <summary>
        /// Fonction principale lol
        /// </summary>
        /// <param name="args">Les arguments/noms de fichier à analyser</param>
        /// <returns>-1 si erreur</returns>
        static int Main(string[] args)
        {
            string appName = Properties.Resources.appName; // appName alias pour ressources

            // Vérification nombre d'arguments
            if(args.Length != 1)
            {
                Console.Error.WriteLine("Un et un seul argument requis");
                Console.Error.WriteLine("Consultez {0} --help", appName);
                return -1;
            }

            string fileName = args[0]; // Assignarion nom de fichier
            string[] pseudocode; // Pseudo-code à analyser

            if (!File.Exists(fileName)) // Si le fichier n'existe pas...
            {
                Console.Error.WriteLine("{0}: Fichier inexistant", fileName);
                return -1;
            }
            try // Essaye de voir si il y a les permissions pour lire le fichier
            {
                FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(fileName));
                f.Demand();
                pseudocode = File.ReadAllLines(fileName); // Lis le fichier
            }
            catch (SecurityException e)
            {
                Console.Error.WriteLine("{0}: {1}", appName, e.Message);
                return -1;
            }

            // Mise en forme du code (Minuscule + suppresion espaces)
            for (int i = 0; i < pseudocode.Length; i++)
            {
                pseudocode[i] = pseudocode[i].ToLower();
                pseudocode[i] = Regex.Replace(pseudocode[i], @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();
                pseudocode[i] = pseudocode[i].Replace(" ", string.Empty);
            }

            Parser pars = new Parser(pseudocode);
            if (!pars.IsValid()) // Si code non valide -> Erreur
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
