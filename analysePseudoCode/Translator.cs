using System;
using System.Collections.Generic;
using System.IO;

namespace analysePseudoCode
{
    /// <summary>
    /// Translate the pseudo-code in pascal
    /// </summary>
    class Translator
    {
        /// <summary>
        /// Basic string function to change first letter of a string to upper case
        /// </summary>
        /// <param name="source">String to change</param>
        /// <returns>source with first letter up-cased</returns>
        private static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }

        /// <summary>
        /// Name of the program, to create the pascal code file
        /// </summary>
        internal string ProgName { get; }

        /// <summary>
        /// List of instruction to convert
        /// </summary>
        private readonly List<Instruction> _instructionList;

        /// <summary>
        /// List of variable to print
        /// </summary>
        private readonly List<Variable> _variableList;

        /// <summary>
        /// List of procedure and function to print
        /// </summary>
        private readonly List<Procedure> _procedureList;

        /// <summary>
        /// Full constructor to fill the attribute
        /// </summary>
        /// <param name="progName">Name of the program</param>
        /// <param name="instructionList">List of instruction</param>
        /// <param name="variableList">List of variable</param>
        /// <param name="procedureList">List of procedure and function</param>
        internal Translator(string progName, List<Instruction> instructionList,
            List<Variable> variableList, List<Procedure> procedureList)
        {
            ProgName = progName;
            _instructionList = instructionList;
            _variableList = variableList;
            _procedureList = procedureList;
        }

        /// <summary>
        /// Convert a parameter object to string
        /// </summary>
        /// <param name="param">Parameter to convert</param>
        /// <param name="id">Id of the parameter in the procedure/function call</param>
        /// <returns>Parameter analysed</returns>
        private string ParamToPascal(Parameter param, int id)
        {
            string rc = "";
            if (param.TypePass == TypePassage.Adress)
                rc += "var ";
            else if (param.TypePass == TypePassage.Unknown)
                rc += "{var} ";

            rc += $"param{id} : ";
            rc += param.TypeParam.Type == TypeEnum.Unknown ? "{To define}" : param.TypeParam.Type.ToString();

            return rc;
        }

        /// <summary>
        /// Do the conversion
        /// </summary>
        internal void Work()
        {
            string fileName = ProgName + ".pas";
            if (File.Exists(fileName))
                try
                {
                    File.Delete(fileName); // Remove the file, if it exist, to recreate from skratch
                }
                catch (Exception)
                {
                    // ignored
                }

            // Initialize the beggining of the pascal code
            List<string> pascalCode = new List<string> {$"PROGRAM {ProgName};", "", "    VAR"};

            string toDefine = "{To define}";
            foreach (Variable var in _variableList)
            { // Print all the variable
                string type = var.Type.Type == TypeEnum.Unknown ? toDefine : var.Type.Type.ToString();
                pascalCode.Add($"        {var.Name} : {type};");
            }

            pascalCode.Add("");

            // Print the analyse of the procedure and the function
            foreach (Procedure proc in _procedureList)
            {
                string firstLine = "    ";
                if (proc.GetType() == typeof(Function))
                    firstLine += "FUNCTION";
                else
                    firstLine += "PROCEDURE";
                firstLine += $" {ToUpperFirstLetter(proc.Name)}(";

                if (proc.ParameterList.Count > 0)
                {
                    firstLine += ParamToPascal(proc.ParameterList[0], 1);
                    if (proc.ParameterList.Count > 1)
                    {
                        for (int i = 1; i < proc.ParameterList.Count; i++)
                        {
                            firstLine += "; ";
                            firstLine += ParamToPascal(proc.ParameterList[i], i + 1);
                        }
                    }
                }

                firstLine += ")";
                if (proc.GetType() == typeof(Function))
                {
                    Function func = (Function) proc;
                    string typeFunction = func.FunctionType.Type == TypeEnum.Unknown ? toDefine : func.FunctionType.Type.ToString();
                    firstLine += $" : {typeFunction}";
                }
                
                firstLine += ";";

                pascalCode.Add(firstLine);
                pascalCode.Add("        BEGIN");
                pascalCode.Add("        //TODO");
                pascalCode.Add("    END;");
                pascalCode.Add("");
            }

            // Add the main code
            pascalCode.Add("    BEGIN");
            foreach (Instruction ins in _instructionList)
            {
                try
                {
                    pascalCode.Add("    " + ins.ToPascal());
                }
                catch (Exception e)
                {
                    Program.PrintError(e.Message);
                }
            }

            pascalCode.Add("    WriteLn();");
            pascalCode.Add("    ReadLn();");
            pascalCode.Add("END.");
            File.WriteAllLines(fileName, pascalCode); // Write the file
        }
    }
}
