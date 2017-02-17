using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace analysePseudoCode
{
    internal class Instruction
    {
        internal string Ligne { get; }

        internal Variable AssigneA { get; private set; }

        internal Procedure Proc { get; private set; }
        //private readonly Operation _operation;

        public Instruction(string chaine)
        {
            Ligne = chaine;

            if (IsAssignation(chaine)){
                AnalyseAssignation(chaine);
                return;
            }
            if (IsAffiche(chaine)) {
                AnalyseAffiche(chaine);
                return;
            }
            if (IsAppelProcedure(chaine)) {
                AnalyseAppelProcedure(chaine);
                return;
            }
            Console.Error.WriteLine("Error: Unknown error, what happened ?");
            throw new Exception("Unknown error");
        }

        private List<string> GetScopeList(string scope)
        {
            List<string> listeElement = new List<string>();
            int debutElem = 0, nbParentheseOpen = 0;
            for (int i = debutElem; i < scope.Length; i++)
            {
                if (scope[i] == '(') nbParentheseOpen++;
                if (scope[i] == ')') nbParentheseOpen--;
                if (nbParentheseOpen == 0 && (scope[i] == '+' || scope[i] == '-' ||
                    scope[i] == '*' || scope[i] == '/' || scope[i] == '>' || scope[i] == '<' || scope[i] == '='))
                {
                    if (scope[i] == '=' && (scope[i] == '>' || scope[i] == '<'))
                    {
                        debutElem++;
                        continue;
                    }
                    listeElement.Add(scope.Substring(debutElem, i - debutElem));
                    debutElem = i + 1;
                }
            }
            listeElement.Add(scope.Substring(debutElem, scope.Length - debutElem));

            return listeElement;
        }

        private TypeElement GetScopeType(string scope)
        {
            List<string> listeElement = GetScopeList(scope);

            TypeElement type = null;
            if (listeElement.Count == 1)
            {
                if (IsVariable(scope))
                    type = Program.GetVariable(scope).Type;
                if (IsConstant(scope))
                    type = AnalyseConstant(scope).Type;
                if (IsFunction(scope))
                    type = AnalyseFunction(scope, false).FunctionType;
                return type;
            }

            type = GetScopeType(listeElement[0]);
            GetScopeType(listeElement[0]).Type = type.Type;
            for (int i = 1; i < listeElement.Count; i++)
            {
                type.Type = GetScopeType(listeElement[i]).Type;
                GetScopeType(listeElement[i]).Type = type.Type;
            }

            if (listeElement.Count == 2 && (scope.Contains(">") || scope.Contains("<") || scope.Contains("=")))
            {
                type = new TypeElement(TypeEnum.Boolean);
            }
            return type;
        }

        #region IsSomething
        private bool IsAssignation(string chaine)
        {
            Regex assRegex = new Regex(@"^[a-z]+<_");
            return assRegex.IsMatch(chaine);
        }

        private bool IsAffiche(string chaine)
        {
            Regex affRegex = new Regex(@"^affiche\([a-z]+\)donne((\d+)|(\d+\.\d+)|(vrai)|(faux))$");
            return affRegex.IsMatch(chaine);
        }

        private bool IsAppelProcedure(string chaine)
        {
            Regex procRegex = new Regex(@"^[^(affiche)][a-z]+\(.*\)$");
            return procRegex.IsMatch(chaine);
        }

        private bool IsFunction(string function)
        {
            Regex isFunctionRegex = new Regex(@"^[a-z]+\(.+\)$");
            return isFunctionRegex.IsMatch(function);
        }

        private bool IsConstant(string constante)
        {
            Regex constRegex = new Regex(@"^((vrai)|(faux)|(\d+)|(\d+\.\d+))$");
            return constRegex.IsMatch(constante);
        }

        private bool IsVariable(string variable)
        {
            Regex isVarRegex = new Regex(@"^[a-z]+$");
            return isVarRegex.IsMatch(variable);
        }

        private bool IsScopeValid(string scope)
        {
            /*int cptOperator = 0;
            for (int i = 0; i < scope.Length; i++)
                if (scope[i] == '-' || scope[i] == '+' || scope[i] == '*' || scope[i] == '/' ||
                    scope[i] == '=' || scope[i] == '>' || scope[i] == '<')
                    if (!((scope[i] == '-' || scope[i] == '=') && (scope[i - 1] == '<' || scope[i - 1] == '>')))
                        cptOperator++;*/
            if ((scope.Length - scope.Replace(">=", "").Length > 2) ||
                (scope.Length - scope.Replace(">=", "").Length > 2) ||
                (scope.Length - scope.Replace(">", "").Length > 1) ||
                (scope.Length - scope.Replace("<", "").Length > 1) ||
                (scope.Length - scope.Replace("=", "").Length > 1))
                return false;
            List<string> listeElement = GetScopeList(scope);

            if (listeElement.Count == 1)
                if ((IsVariable(listeElement[0]) && Program.GetVariable(listeElement[0]) != null) ||
                    IsConstant(listeElement[0]) || IsFunction(listeElement[0]))
                    return true;
                else
                    return false;
            foreach (string elem in listeElement)
                if (!IsScopeValid(elem))
                    return false;
            return true;
        }
        #endregion

        #region AnalyseBigStuff
        private void AnalyseAssignation(string assignation)
        {
            string ppv = Properties.Resources.ppv;
            string varName = assignation.Substring(0, assignation.IndexOf(ppv, StringComparison.CurrentCulture));
            string calcul = assignation.Substring(assignation.IndexOf(ppv,
                StringComparison.CurrentCulture) + ppv.Length);
            try
            {
                AssigneA = Program.GetVariable(varName);
            }
            catch (NotImplementedException)
            {
                AssigneA = null;
            }

            if (AssigneA == null)
            {
                Variable rc = AnalyseCalcul(calcul, false);
                if (rc.ContentKnown)
                {
                    if (rc.Type.Type == TypeEnum.Boolean)
                        AssigneA = new Variable(varName, rc.Type, rc.GetContentBool());
                    else if (rc.Type.Type == TypeEnum.Integer)
                        AssigneA = new Variable(varName, rc.Type, rc.GetContentInt());
                    else if (rc.Type.Type == TypeEnum.Real)
                        AssigneA = new Variable(varName, rc.Type, rc.GetContentReal());
                }
                else
                    AssigneA = new Variable(varName, rc.Type);
                if (Program.InsertVariable(AssigneA) == StatutInsertion.Error)
                {
                    throw new Exception("An error appeared approxymately here");
                }
            }
            else
                AssigneA.SetVariable(AnalyseCalcul(calcul, true));
        }

        private void AnalyseAffiche(string affiche)
        {
            string affKeyWord = "affiche(", donneKeyWord = ")donne";
            string varName = affiche.Substring(affKeyWord.Length,
                affiche.IndexOf(donneKeyWord, StringComparison.CurrentCulture) - affKeyWord.Length);
            string content = affiche.Substring(affiche.IndexOf(donneKeyWord,
                StringComparison.CurrentCulture) + donneKeyWord.Length);
            Variable var = null;
            try
            {
                var = Program.GetVariable(varName);
            }
            catch (NotImplementedException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine($"{varName} doesn't exist");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            //TODO Affichage ?
            var.SetVariable(AnalyseConstant(content));
        }
        
        private void AnalyseAppelProcedure(string affiche)
        {
            string procName = affiche.Substring(0, affiche.IndexOf("(", StringComparison.CurrentCulture));
            int debutParam = procName.Length + 1, nbParentheseOpen = 0;
            List<string> paramList = new List<string>();
            for (int i = debutParam; i < affiche.Length - 1; i++)
            {
                if (affiche[i] == '(') nbParentheseOpen++;
                if (affiche[i] == ')') nbParentheseOpen--;
                if (nbParentheseOpen == 0 && affiche[i] == ',')
                {
                    paramList.Add(affiche.Substring(debutParam, i - debutParam));
                    debutParam = i + 1;
                }
            }
            paramList.Add(affiche.Substring(debutParam, affiche.Length - debutParam - 1));
            int j = 0;
            Parameter[] listeParameters = new Parameter[paramList.Count];
            foreach (string param in paramList) listeParameters[j++] = AnalyseParametre(param);

            Program.InsertProcedure(Proc = new Procedure(procName, listeParameters));
        }
        #endregion

        #region AnalyseSmallerStuff
        private Parameter AnalyseParametre(string param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            TypeElement type;
            TypePassage pass = TypePassage.Unknown;
            
            if (!IsVariable(param))
            {
                pass = TypePassage.Value;
                type = IsConstant(param) ? AnalyseConstant(param).Type : AnalyseCalcul(param, false).Type;
            }
            else
            {
                type = Program.GetVariable(param).Type;
            }

            return new Parameter(type, pass);
        }

        private Function AnalyseFunction(string param, bool fromAssignation)
        {
            TypeElement type = fromAssignation ? AssigneA.Type : new TypeElement(TypeEnum.Unknown);
            string funcName = param.Substring(0, param.IndexOf("(", StringComparison.CurrentCulture));
            int debutParam = funcName.Length + 1, nbParentheseOpen = 0;
            List<string> paramList = new List<string>();
            for (int i = debutParam; i < param.Length - 1; i++)
            {
                if (param[i] == '(') nbParentheseOpen++;
                if (param[i] == ')') nbParentheseOpen--;
                if (nbParentheseOpen == 0 && param[i] == ',')
                {
                    paramList.Add(param.Substring(debutParam, i - debutParam));
                    debutParam = i + 1;
                }
            }
            paramList.Add(param.Substring(debutParam, param.Length - debutParam - 1));
            int j = 0;
            Parameter[] listeParameters = new Parameter[paramList.Count];
            foreach (string paramContent in paramList) listeParameters[j++] = AnalyseParametre(paramContent);

            Program.InsertFunction(new Function(funcName, type, listeParameters));

            return Program.GetFunction(funcName);
        }

        private Variable AnalyseCalcul(string calcul, bool fromAssignation)
        {
            //TODO
            Regex isWithFunctionRegex = new Regex(@"[a-z]+\(.+\)");
            if (IsVariable(calcul))
                return Program.GetVariable(calcul);
            if (IsConstant(calcul))
                return AnalyseConstant(calcul);
            if (IsFunction(calcul))
                return new Variable("function", AnalyseFunction(calcul, fromAssignation).FunctionType);

            if (!IsScopeValid(calcul))
                throw new ArithmeticException();

            TypeElement type = GetScopeType(calcul);

            //TODO Analyse du calcul
            if (!isWithFunctionRegex.IsMatch(calcul))
            {
                
            }

            return new Variable("calcul", type);
            // Valeur peut être inconnu
        }

        private Variable AnalyseConstant(string constant)
        {
            if (!IsConstant(constant))
            {
                Console.Error.WriteLine("Error: constant not valid");
                throw new Exception("Invalid constant");
            }

            if (constant == "vrai" || constant == "faux")
                return new Variable("constant", constant == "vrai");

            if (new Regex(@"^\d+$").IsMatch(constant))
                return new Variable("constant", Int32.Parse(constant));

            if (new Regex(@"^\d+\.\d+$").IsMatch(constant))
                return new Variable("constant", Single.Parse(constant));

            throw new Exception("Not understandable error");
        }
        #endregion

        public override string ToString()
        {
            return Ligne;
        }
    }
}