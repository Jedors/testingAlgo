using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace analysePseudoCode
{
    internal class Program
    {
        #region Elements lists
        private static readonly List<Procedure> ListeProcedure = new List<Procedure>();
        private static readonly List<Variable> ListeVariable = new List<Variable>();
        private static readonly List<Instruction> ListeInstruction = new List<Instruction>();

        internal static StatutInsertion InsertProcedure(Procedure procedure)
        {
            if (procedure.Nom == "")
                return StatutInsertion.Error;
            foreach (Procedure proc in ListeProcedure)
            {
                if (proc.Nom == procedure.Nom)
                {
                    if (proc.GetType() == typeof(Function))
                    {
                        Console.Error.WriteLine("Error: Atempt to instanciate a procedure named like a function.");
                        return StatutInsertion.Error;
                    }

                    if (!procedure.IsListMatch(proc.ListeParametres))
                    {
                        Console.Error.WriteLine("Error: Procedure signature incoherent.");
                        return StatutInsertion.Error;
                    }

                    bool isUpdated = false;
                    for (int i = 0; i < procedure.ListeParametres.Count; i++)
                    {
                        if (procedure.ListeParametres[i].TypeParam.Type == TypeEnum.Unknown &&
                            proc.ListeParametres[i].TypeParam.Type != TypeEnum.Unknown)
                        {
                            procedure.ListeParametres[i].TypeParam.Type = proc.ListeParametres[i].TypeParam.Type;
                            isUpdated = true;
                        }

                        if (procedure.ListeParametres[i].TypePass == TypePassage.Unknown &&
                            proc.ListeParametres[i].TypePass != TypePassage.Unknown)
                        {
                            procedure.ListeParametres[i].TypePass = proc.ListeParametres[i].TypePass;
                            isUpdated = true;
                        }
                    }

                    if (isUpdated)
                    {
                        return StatutInsertion.Updated;
                    }
                    return StatutInsertion.Nothing;
                }
            }
            ListeProcedure.Add(procedure);
            return StatutInsertion.Inserted;
        }

        internal static StatutInsertion InsertFunction(Function function)
        {
            if (function.Nom == "")
                return StatutInsertion.Error;
            foreach (Procedure proc in ListeProcedure)
            {
                if (proc.GetType() == typeof(Function))
                {
                    Function func = (Function) proc;
                    if (func.Nom == function.Nom)
                    {
                        if (!function.IsListMatch(func.ListeParametres))
                        {
                            Console.Error.WriteLine("Error: Function signature incoherent.");
                            return StatutInsertion.Error;
                        }

                        bool isUpdated = false;

                        if (function.TypeFunction.Type != func.TypeFunction.Type &&
                            function.TypeFunction.Type != TypeEnum.Unknown)
                        {
                            return StatutInsertion.Error;
                        }
                        if (function.TypeFunction.Type != TypeEnum.Unknown)
                        {
                            function.TypeFunction.Type = func.TypeFunction.Type;
                            isUpdated = true;
                        }

                        for (int i = 0; i < function.ListeParametres.Count; i++)
                        {
                            if (function.ListeParametres[i].TypeParam.Type == TypeEnum.Unknown &&
                                func.ListeParametres[i].TypeParam.Type != TypeEnum.Unknown)
                            {
                                function.ListeParametres[i].TypeParam.Type = func.ListeParametres[i].TypeParam.Type;
                                isUpdated = true;
                            }

                            if (function.ListeParametres[i].TypePass == TypePassage.Unknown &&
                                func.ListeParametres[i].TypePass != TypePassage.Unknown)
                            {
                                function.ListeParametres[i].TypePass = func.ListeParametres[i].TypePass;
                                isUpdated = true;
                            }
                        }

                        if (isUpdated)
                        {
                            return StatutInsertion.Updated;
                        }
                        return StatutInsertion.Nothing;
                    }
                }
                
            }
            ListeProcedure.Add(function);
            return StatutInsertion.Inserted;
        }

        internal static StatutInsertion InsertVariable(Variable variable)
        {
            if (variable.Nom == "")
                return StatutInsertion.Error;
            foreach (Variable var in ListeVariable)
            {
                if (variable.Nom == var.Nom)
                {
                    if (variable.Type.Type != var.Type.Type)
                    {
                        Console.Error.WriteLine("Error: A variable cannot have two types");
                        throw new Exception("Type error, a variable with the same name and a different" +
                                            " type already exist.");
                    }

                    try
                    {
                        if (var.SetVariable(variable))
                        {
                            return StatutInsertion.Updated;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        return StatutInsertion.Error;
                    }
                    return StatutInsertion.Nothing;
                }
            }
            ListeVariable.Add(variable);
            return StatutInsertion.Inserted;
        }

        internal static Variable GetVariable(string nom)
        {
            foreach (Variable var in ListeVariable)
                if (var.Nom == nom)
                    return var;
            throw new NotImplementedException("Variable doesn't exist.");
        }

        internal static Procedure GetProcedure(string nom)
        {
            foreach (Procedure var in ListeProcedure)
                if (var.Nom == nom)
                    return var;
            throw new NotImplementedException("Procedure doesn't exist.");
        }

        internal static Function GetFunction(string nom)
        {
            foreach (Procedure var in ListeProcedure)
                if (var.Nom == nom)
                    if (var.GetType() == typeof(Function))
                        return (Function) var;
                    else
                        throw new Exception("Not a function, it's a procedure");
            throw new NotImplementedException("Variable doesn't exist.");
        }
        #endregion

        internal static int Main(string[] args)
        {
            string appName = Properties.Resources.appName; // appName alias pour ressources

            // Vérification nombre d'arguments
            if (args.Length != 1)
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

            foreach (string line in pseudocode)
            {
                ListeInstruction.Add(new Instruction(line));
            }

            foreach (Variable var in ListeVariable)
                Console.WriteLine(var.ToString());
            foreach (Procedure proc in ListeProcedure)
                Console.WriteLine(proc.ToString());
            foreach (Instruction ins in ListeInstruction)
                Console.WriteLine(ins.ToString());

            Console.ReadKey();
            Console.WriteLine();
            return 0;
        }
    }
}
