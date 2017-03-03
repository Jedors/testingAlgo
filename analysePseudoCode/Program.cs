using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;

//TODO More exception catch, status insertion to check

namespace analysePseudoCode
{
    /// <summary>
    /// Main class, contain the global data list, shared in all
    /// the project architecture
    /// </summary>
    internal class Program
    {
        #region Elements lists
        /// <summary>
        /// List of the function and procedure found in the pseudo-code
        /// Access and insertion only possible by the methods made for that
        /// </summary>
        private static readonly List<Procedure> ProcedureList = new List<Procedure>();
        /// <summary>
        /// List of the variables found in the pseudo-code
        /// Access and insertion only possible by the methods made for that
        /// </summary>
        private static readonly List<Variable> VariableList = new List<Variable>();
        /// <summary>
        /// List of the instruction of the pseudo-code
        /// </summary>
        private static readonly List<Instruction> InstructionList = new List<Instruction>();

        /// <summary>
        /// Insert a procedure in the procedure list, or update an already existing one
        /// </summary>
        /// <param name="procedure">Procedure to insert, identify by it name</param>
        /// <returns>The Insertion status (Inserted, updated, ...)</returns>
        internal static StatutInsertion InsertProcedure(Procedure procedure)
        {
            if (procedure.Name == "")
                return StatutInsertion.Error;
            foreach (Procedure proc in ProcedureList)
            {
                if (proc.Name == procedure.Name) // If the procedure already exist...
                {
                    if (proc.GetType() == typeof(Function)) // If a function already exist => Error
                    {
                        Console.Error.WriteLine("Error: Atempt to instanciate a procedure named like a function.");
                        return StatutInsertion.Error;
                    }

                    if (!procedure.IsListMatch(proc.ParameterList)) // Signature verification
                    {
                        Console.Error.WriteLine("Error: Procedure signature incoherent.");
                        return StatutInsertion.Error;
                    }

                    bool isUpdated = false; // For the return code
                    for (int i = 0; i < procedure.ParameterList.Count; i++) // For each param, try to update it if different
                    {
                        if (procedure.ParameterList[i].TypeParam.Type == TypeEnum.Unknown &&
                            proc.ParameterList[i].TypeParam.Type != TypeEnum.Unknown) // if type now known, update it
                        {
                            try
                            {
                                procedure.ParameterList[i].TypeParam.Type = proc.ParameterList[i].TypeParam.Type;
                            }
                            catch (ArgumentException e)
                            {
                                PrintError(e.Message);
                            }
                            isUpdated = true;
                        }

                        if (procedure.ParameterList[i].TypePass == TypePassage.Unknown &&
                            proc.ParameterList[i].TypePass != TypePassage.Unknown) // if passage know known, update it
                        {
                            try
                            {
                                procedure.ParameterList[i].TypePass = proc.ParameterList[i].TypePass;
                            }
                            catch (ArgumentException e)
                            {
                                PrintError(e.Message);
                            }
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
            ProcedureList.Add(procedure); // If not found, simply insert
            return StatutInsertion.Inserted;
        }

        /// <summary>
        /// Insert a function in the procedure list, or update an already existing one
        /// Same as InsertProcedure, just check the function type too
        /// </summary>
        /// <param name="function">Function to insert, identify by it name</param>
        /// <returns>The Insertion status (Inserted, updated, ...)</returns>
        internal static StatutInsertion InsertFunction(Function function) // To factorize with insertprocedure
        {
            if (function.Name == "")
                return StatutInsertion.Error;
            foreach (Procedure proc in ProcedureList)
            {
                if (proc.Name == function.Name && proc.GetType() != typeof(Function)) // Check the name and type
                {
                    Console.Error.WriteLine("Error: A procedure with the same name already exist");
                    return StatutInsertion.Error;
                }
                if (proc.GetType() == typeof(Function)) // To check only the function
                {
                    Function func = (Function) proc;
                    if (func.Name == function.Name) // If the function already exist...
                    {
                        if (!function.IsListMatch(func.ParameterList)) // Signature verification
                        {
                            Console.Error.WriteLine("Error: Function signature incoherent.");
                            return StatutInsertion.Error;
                        }

                        bool isUpdated = false; // For the return code

                        if (function.FunctionType.Type != func.FunctionType.Type &&
                            function.FunctionType.Type != TypeEnum.Unknown) // If type different => Error
                        {
                            return StatutInsertion.Error;
                        }
                        if (function.FunctionType.Type != TypeEnum.Unknown) // Update the type if you know it
                        {
                            try
                            {
                                function.FunctionType.Type = func.FunctionType.Type;
                            }
                            catch (ArgumentException e)
                            {
                                PrintError(e.Message);
                            }
                            isUpdated = true;
                        }

                        for (int i = 0; i < function.ParameterList.Count; i++) // For each param, try to update it if different
                        {
                            if (function.ParameterList[i].TypeParam.Type == TypeEnum.Unknown &&
                                func.ParameterList[i].TypeParam.Type != TypeEnum.Unknown) // if type now known, update it
                            {
                                try
                                {
                                    function.ParameterList[i].TypeParam.Type = func.ParameterList[i].TypeParam.Type;
                                }
                                catch (ArgumentException e)
                                {
                                    PrintError(e.Message);
                                }
                                isUpdated = true;
                            }

                            if (function.ParameterList[i].TypePass == TypePassage.Unknown &&
                                func.ParameterList[i].TypePass != TypePassage.Unknown) // if passage now known, update it
                            {
                                try
                                {
                                    function.ParameterList[i].TypePass = func.ParameterList[i].TypePass;
                                }
                                catch (ArgumentException e)
                                {
                                    PrintError(e.Message);
                                }
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
            ProcedureList.Add(function); // If not found, simply insert
            return StatutInsertion.Inserted;
        }

        /// <summary>
        /// Insert a variable in the variable list, or update an already existing one
        /// </summary>
        /// <param name="variable">Variable to insert, identify by it name</param>
        /// <returns>The Insertion status (Inserted, updated, ...)</returns>
        internal static StatutInsertion InsertVariable(Variable variable)
        {
            if (variable.Name == "")
                return StatutInsertion.Error;
            foreach (Variable var in VariableList) // Check if already exist
            {
                if (variable.Name == var.Name)
                {
                    if (var.Type.Type != TypeEnum.Unknown &&
                        variable.Type.Type != TypeEnum.Unknown &&
                        variable.Type.Type != var.Type.Type) // If type different & not unknown...
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
            VariableList.Add(variable); // If not found, simply insert
            return StatutInsertion.Inserted;
        }

        /// <summary>
        /// Get a variable from the list
        /// Error if doesn't exist
        /// </summary>
        /// <param name="name">name of the variable to return</param>
        /// <returns>return the good variable</returns>
        internal static Variable GetVariable(string name)
        {
            foreach (Variable var in VariableList)
                if (var.Name == name)
                    return var;
            throw new NotImplementedException("Variable doesn't exist.");
        }

        /// <summary>
        /// Get the procedure name like name parameter
        /// Error if doesn't exist or is function
        /// </summary>
        /// <param name="name">name of the procedure to found</param>
        /// <returns>the procedure found</returns>
        internal static Procedure GetProcedure(string name)
        {
            foreach (Procedure proc in ProcedureList)
                if (proc.Name == name && proc.GetType() != typeof(Function))
                    return proc;
            throw new NotImplementedException("Procedure doesn't exist.");
        }

        /// <summary>
        /// Get the function name like name parameter
        /// Error if doesn't exist or is not a function
        /// </summary>
        /// <param name="name">name of the function to found</param>
        /// <returns>the function found</returns>
        internal static Function GetFunction(string name)
        {
            foreach (Procedure proc in ProcedureList)
                if (proc.Name == name)
                    if (proc.GetType() == typeof(Function))
                        return (Function) proc;
                    else
                        throw new Exception("Not a function, it's a procedure");
            throw new NotImplementedException("Function doesn't exist.");
        }
        #endregion

        /// <summary>
        /// Main method, format the code and start the analyse
        /// </summary>
        /// <param name="args">Name of the code file</param>
        /// <returns>I don't know, 0 if OK</returns>
        internal static int Main(string[] args)
        {
            string appName = Properties.Resources.appName; // appName alias for the ressources

            // Verification arguments number
            if (args.Length != 1)
            {
                Console.Error.WriteLine("One and only one argument required");
                Console.Error.WriteLine("Check {0} --help", appName);
                Console.Error.WriteLine("(Not implemented yet.)");
                return -1;
            }

            string fileName = args[0]; // Assignation file name
            string[] pseudocode; // Pseudo-code to analyse

            if (!File.Exists(fileName)) // If the file doesn't exist...
            {
                Console.Error.WriteLine("{0}: Unexisting file", fileName);
                return -1;
            }
            try // Try to see if their is permission to read the file
            {
                FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(fileName));
                f.Demand();
                pseudocode = File.ReadAllLines(fileName); // Read the file
            }
            catch (SecurityException e)
            {
                Console.Error.WriteLine("{0}: {1}", appName, e.Message);
                return -1;
            }

            // Optimisation of the code (All to lower + remove space)
            for (int i = 0; i < pseudocode.Length; i++)
            {
                pseudocode[i] = pseudocode[i].ToLower();
                pseudocode[i] = Regex.Replace(pseudocode[i], @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();
                pseudocode[i] = pseudocode[i].Replace(" ", string.Empty);
            }

            Parser pars = new Parser(pseudocode);
            if (!pars.IsValid()) // If code not valid -> Error
            {
                Console.Error.WriteLine("Analyse abort");
                Console.ReadKey();
                return -1;
            }

            int id = 1;
            foreach (string line in pseudocode) // Line by line analyse
            {
                try
                {
                    InstructionList.Add(new Instruction(line, id++));
                }
                catch (Exception e)
                {
                    PrintError(e.Message);
                }
            }

            // Print the result
            foreach (Variable var in VariableList)
                Console.WriteLine(var.ToString());
            Console.WriteLine();
            foreach (Procedure proc in ProcedureList)
                Console.WriteLine(proc.ToString());
            Console.WriteLine();
            foreach (Instruction ins in InstructionList)
                Console.WriteLine(ins.ToString());

            List<string> listNamePart = new List<string>(args[0].Split('.'));
            if (listNamePart.Count > 1)
                listNamePart.RemoveAt(listNamePart.Count - 1);
            string progName = String.Join(".", listNamePart);

            Translator trans = new Translator(progName, InstructionList, VariableList, ProcedureList);
            trans.Work();

            Console.WriteLine();
            Console.ReadKey();
            return 0;
        }

        /// <summary>
        /// On error, print the error and leave the application
        /// </summary>
        /// <param name="errorMessage">Error message to print</param>
        internal static void PrintError(string errorMessage)
        {
            Console.Error.WriteLine($"[Error]{errorMessage}");
            Console.ReadKey();
            Environment.Exit(-1);
        }
    }
}

