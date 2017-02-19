using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace analysePseudoCode
{
    class Translator
    {
        private static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }

        internal string ProgName { get; }
        private readonly List<Instruction> _instructionList;
        private readonly List<Variable> _variableList;
        private readonly List<Procedure> _procedureList;

        internal Translator(string progName, List<Instruction> instructionList,
            List<Variable> variableList, List<Procedure> procedureList)
        {
            ProgName = progName;
            _instructionList = instructionList;
            _variableList = variableList;
            _procedureList = procedureList;
        }

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

        internal void Work()
        {
            string fileName = ProgName + ".pas";
            if (File.Exists(fileName))
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception)
                {
                    // ignored
                }

            List<string> pascalCode = new List<string> {$"PROGRAM {ProgName};", "", "    VAR"};

            string toDefine = "{To define}";
            foreach (Variable var in _variableList)
            {
                string type = var.Type.Type == TypeEnum.Unknown ? toDefine : var.Type.Type.ToString();
                pascalCode.Add($"        {var.Name} : {type};");
            }

            pascalCode.Add("");

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

            pascalCode.Add("    BEGIN");
            foreach (Instruction ins in _instructionList)
            {
                pascalCode.Add("    " + ins.ToPascal());
            }

            pascalCode.Add("    WriteLn();");
            pascalCode.Add("    ReadLn();");
            pascalCode.Add("END.");
            File.WriteAllLines(fileName, pascalCode);
        }
    }
}
