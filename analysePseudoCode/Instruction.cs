using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace analysePseudoCode
{
    /// <summary>
    /// Where the magic born
    /// Analyse the code
    /// 
    /// Definition: Scope = level of a calcul
    /// (4+5)*(3+1) is a scope
    /// 4+5 and 3+1 are two lower level scope
    /// 4, 5, 3 and 1 are lowerer level scope
    /// </summary>
    internal class Instruction
    {
        /// <summary>
        /// Code line to analyse
        /// </summary>
        internal string Ligne { get; }

        /// <summary>
        /// Id of the instruction line
        /// </summary>
        private int LineId { get; }

        /// <summary>
        /// If the line is an assignation line,
        /// this is the assignate variable
        /// </summary>
        internal Variable AssigneA { get; private set; }

        /// <summary>
        /// If it is a procedure call,
        /// this is the procedure
        /// </summary>
        internal Procedure Proc { get; private set; }

        /// <summary>
        /// Simple constructor, define if the line is
        /// an assignation, a print of variable, or a procedure call
        /// </summary>
        /// <param name="line">Line of code to analyse</param>
        /// <param name="id">Id of the instruction line</param>
        public Instruction(string line, int id)
        {
            Ligne = line;
            LineId = id;
            if (IsAssignation(line)){
                try
                {
                    AnalyseAssignation(line);
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }
                return;
            }
            if (IsAffiche(line)) {
                AnalyseAffiche(line);
                return;
            }
            if (IsAppelProcedure(line)) {
                AnalyseAppelProcedure(line);
                return;
            }
            Console.Error.WriteLine("Error: Unknown error, what happened ?");
            throw new Exception("Unknown error");
        }

        /// <summary>
        /// Get all the scope of a lower level
        /// </summary>
        /// <param name="scope">higher scope to analyse</param>
        /// <returns>List of lower level scope</returns>
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
                    { // Ignore operator if from a >= or a <=
                        debutElem++;
                        continue;
                    }
                    listeElement.Add(scope.Substring(debutElem, i - debutElem));
                    debutElem = i + 1;
                }
            }
            listeElement.Add(scope.Substring(debutElem, scope.Length - debutElem));

            for (int i = 0; i < listeElement.Count; i++) // Remove '(' and ')' arround a scope
            {
                if (listeElement[i][0] == '(')
                    listeElement[i] = listeElement[i].Substring(1, listeElement[i].Length - 2);
            }

            return listeElement;
        }

        /// <summary>
        /// Get the type of a scope by analysing lower level scope
        /// </summary>
        /// <param name="scope">scope to find type</param>
        /// <returns>Type of the scope</returns>
        private TypeElement GetScopeType(string scope)
        {
            List<string> listeElement = GetScopeList(scope);

            TypeElement type = null;
            if (listeElement.Count == 1) // If lowerer level scope, find his type
            {
                if (IsVariable(scope))
                    try
                    {
                        type = Program.GetVariable(scope).Type;
                    }
                    catch (NotImplementedException e)
                    {
                        CatcherError(e.Message);
                    }
                if (IsConstant(scope))
                    try
                    {
                        type = AnalyseConstant(scope).Type;
                    }
                    catch (Exception e)
                    {
                        CatcherError(e.Message);
                    }
                if (IsFunction(scope))
                    type = AnalyseFunction(scope, false).FunctionType;
                return type;
            }

            type = GetScopeType(listeElement[0]);
            try
            {
                GetScopeType(listeElement[0]).Type = type.Type;
            }
            catch (ArgumentException e)
            {
                CatcherError(e.Message);
            }
            for (int i = 1; i < listeElement.Count; i++)
            { // Make all the types to be the same
                try
                {
                    type.Type = GetScopeType(listeElement[i]).Type;
                    GetScopeType(listeElement[i]).Type = type.Type;
                }
                catch (ArgumentException e)
                {
                    CatcherError(e.Message);
                }
            }

            if (listeElement.Count == 2 && (scope.Contains(">") || scope.Contains("<") || scope.Contains("=")))
            { // If boolean operation, it's a boolean
                type = new TypeElement(TypeEnum.Boolean);
            }
            return type;
        }

        /// <summary>
        /// On error, print the error and leave the application
        /// </summary>
        /// <param name="errorMessage">Error message to print</param>
        private void CatcherError(string errorMessage)
        {
            Console.Error.WriteLine($"[Error] At line {LineId}: {errorMessage}\n{Ligne}");
            Console.ReadKey();
            Environment.Exit(-1);
        }

        #region IsSomething
        /// <summary>
        /// Analyse if or not it is an assignation
        /// </summary>
        /// <param name="chaine">line to analyse</param>
        /// <returns>true if assignation</returns>
        internal bool IsAssignation(string chaine)
        {
            Regex assRegex = new Regex(@"^[a-z]+<_");
            return assRegex.IsMatch(chaine);
        }

        /// <summary>
        /// Analyse if or not it is a print variable
        /// </summary>
        /// <param name="chaine">line to analyse</param>
        /// <returns>true if print</returns>
        internal bool IsAffiche(string chaine)
        {
            Regex affRegex = new Regex(@"^affiche\([a-z]+\)donne((\d+)|(\d+\.\d+)|(vrai)|(faux))$");
            return affRegex.IsMatch(chaine);
        }

        /// <summary>
        /// Analyse if or not it is a procedure call
        /// </summary>
        /// <param name="chaine">line to analyse</param>
        /// <returns>true if procedure</returns>
        internal bool IsAppelProcedure(string chaine)
        {
            Regex procRegex = new Regex(@"^[^(affiche)][a-z]+\(.*\)$");
            return procRegex.IsMatch(chaine);
        }

        /// <summary>
        /// Analyse if or not it is a function
        /// </summary>
        /// <param name="function">line to analyse</param>
        /// <returns>true if function</returns>
        internal bool IsFunction(string function)
        {
            Regex isFunctionRegex = new Regex(@"^[a-z]+\(.+\)$");
            
            return isFunctionRegex.IsMatch(function);
        }

        /// <summary>
        /// Analyse if or not it is a constant
        /// </summary>
        /// <param name="constante">line to analyse</param>
        /// <returns>true if constant</returns>
        private bool IsConstant(string constante)
        {
            Regex constRegex = new Regex(@"^((vrai)|(faux)|(\d+)|(\d+\.\d+))$");
            return constRegex.IsMatch(constante);
        }

        /// <summary>
        /// Analyse if or not it is a variable
        /// </summary>
        /// <param name="variable">line to analyse</param>
        /// <returns>true if variable</returns>
        private bool IsVariable(string variable)
        {
            Regex isVarRegex = new Regex(@"^[a-z]+$");
            return isVarRegex.IsMatch(variable);
        }

        /// <summary>
        /// Check if a scope is correct
        /// </summary>
        /// <param name="scope">scope to analyse</param>
        /// <returns>True if correct</returns>
        private bool IsScopeValid(string scope)
        {
            /*int cptOperator = 0;
            for (int i = 0; i < scope.Length; i++)
                if (scope[i] == '-' || scope[i] == '+' || scope[i] == '*' || scope[i] == '/' ||
                    scope[i] == '=' || scope[i] == '>' || scope[i] == '<')
                    if (!((scope[i] == '-' || scope[i] == '=') && (scope[i - 1] == '<' || scope[i - 1] == '>')))
                        cptOperator++;*/ // I keep it, can be usefull
            if ((scope.Length - scope.Replace(">=", "").Length > 2) ||
                (scope.Length - scope.Replace(">=", "").Length > 2) ||
                (scope.Length - scope.Replace(">", "").Length > 1) ||
                (scope.Length - scope.Replace("<", "").Length > 1) ||
                (scope.Length - scope.Replace("=", "").Length > 1))
                return false; // If multiple boolean operator
            List<string> listeElement = GetScopeList(scope);

            try
            {
                if (listeElement.Count == 1)
                    if ((IsVariable(listeElement[0]) && /* Not good, not null */
                         Program.GetVariable(listeElement[0]) != null) ||
                        IsConstant(listeElement[0]) || IsFunction(listeElement[0]))
                        return true; // If element is correct
                    else
                        return false;
            }
            catch (Exception e)
            {
                CatcherError(e.Message);
            }
            foreach (string elem in listeElement)
                if (!IsScopeValid(elem)) // Analyse all lower level element
                    return false;
            return true;
        }
        #endregion

        #region AnalyseBigStuff
        /// <summary>
        /// Analyse an assignation
        /// </summary>
        /// <param name="assignation">assignation to analyse</param>
        private void AnalyseAssignation(string assignation)
        {
            string ppv = Properties.Resources.ppv;
            string varName = assignation.Substring(0, assignation.IndexOf(ppv, StringComparison.CurrentCulture));
            string calcul = assignation.Substring(assignation.IndexOf(ppv,
                StringComparison.CurrentCulture) + ppv.Length);
            try // Try to get the variable if already exist
            {
                AssigneA = Program.GetVariable(varName);
            }
            catch (NotImplementedException)
            {
                AssigneA = null;
            }

            if (AssigneA == null) // If not exist, create it
            {
                Variable rc;
                try
                {
                    rc = AnalyseCalcul(calcul, false);
                }
                catch (ArithmeticException e)
                {
                    CatcherError(e.Message);
                    return;
                }
                if (rc.ContentKnown) // If assignate content is known...
                {
                    if (rc.Type.Type == TypeEnum.Boolean)
                        try
                        {
                            AssigneA = new Variable(varName, rc.Type, rc.GetContentBool());
                        }
                        catch (Exception e)
                        {
                            CatcherError(e.Message);
                        }
                    else if (rc.Type.Type == TypeEnum.Integer)
                        try
                        {
                            AssigneA = new Variable(varName, rc.Type, rc.GetContentInt());
                        }
                        catch (Exception e)
                        {
                            CatcherError(e.Message);
                        }
                    else if (rc.Type.Type == TypeEnum.Real)
                        try
                        {
                            AssigneA = new Variable(varName, rc.Type, rc.GetContentReal());
                        }
                        catch (Exception e)
                        {
                            CatcherError(e.Message);
                        }
                }
                else
                    try
                    {
                        AssigneA = new Variable(varName, rc.Type);
                    }
                    catch (Exception e)
                    {
                        CatcherError(e.Message);
                    }
                try
                {
                    if (Program.InsertVariable(AssigneA) == StatutInsertion.Error)
                    {
                        throw new Exception("An error appeared approxymately here");
                    }
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }
            }
            else
                try
                {
                    AssigneA.SetVariable(AnalyseCalcul(calcul, true)); // If variable exist, just an update
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }
        }

        /// <summary>
        /// Analyse a print message
        /// </summary>
        /// <param name="affiche">print code to parse</param>
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
            catch (NotImplementedException ex) // If the variable doesn't exist...
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine($"{varName} doesn't exist");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            //TODO Affichage ?
            try
            {
                var.SetVariable(AnalyseConstant(content));
            } // Update of the content
            catch (Exception e)
            {
                CatcherError(e.Message);
            }
        }
        
        /// <summary>
        /// Analyse a procedure call
        /// </summary>
        /// <param name="affiche">Procedure code to analyse</param>
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
            } // Get parameter list
            paramList.Add(affiche.Substring(debutParam, affiche.Length - debutParam - 1));
            int j = 0;
            Parameter[] listeParameters = new Parameter[paramList.Count];
            try
            {
                foreach (string param in paramList) listeParameters[j++] = AnalyseParametre(param);
            }
            catch (ArgumentNullException e)
            {
                CatcherError(e.Message);
            }

            try
            {
                Program.InsertProcedure(Proc = new Procedure(procName, listeParameters));
            }
            catch (ArgumentException e)
            {
                CatcherError(e.Message);
            }
        }
        #endregion

        #region AnalyseSmallerStuff
        /// <summary>
        /// Analyse a parameter as string, and return it as Parameter
        /// </summary>
        /// <param name="param">parameter to analyse</param>
        /// <returns>Parameter object</returns>
        private Parameter AnalyseParametre(string param)
        {
            if (param == null) throw new ArgumentNullException(nameof(param));
            TypeElement type;
            TypePassage pass = TypePassage.Unknown;
            
            if (!IsVariable(param))
            { // Only a single variable can be by adress
                pass = TypePassage.Value;
                try
                {
                    type = IsConstant(param) ? AnalyseConstant(param).Type : AnalyseCalcul(param, false).Type;
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                    return null;
                }
            }
            else
            {
                try
                {
                    type = Program.GetVariable(param).Type;
                }
                catch (NotImplementedException e)
                {
                    CatcherError(e.Message);
                    return null;
                }
            }

            return new Parameter(type, pass);
        }

        /// <summary>
        /// Analyse a function string, and return it as an object
        /// </summary>
        /// <param name="param">function to analyse</param>
        /// <param name="fromAssignation">True if directly from assignation, false from a lower level</param>
        /// <returns>Function object</returns>
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
            } // Get parameter list
            paramList.Add(param.Substring(debutParam, param.Length - debutParam - 1));
            int j = 0;
            Parameter[] listeParameters = new Parameter[paramList.Count];
            foreach (string paramContent in paramList) listeParameters[j++] = AnalyseParametre(paramContent);

            try
            {
                Program.InsertFunction(new Function(funcName, type, listeParameters));
            }
            catch (ArgumentException e)
            {
                CatcherError(e.Message);
            }

            try
            {
                return Program.GetFunction(funcName);
            }
            catch (NotImplementedException e)
            {
                CatcherError(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Analyse a calcul, return a variable, for the type and eventually the content
        /// </summary>
        /// <param name="calcul">calcul to analyse</param>
        /// <param name="fromAssignation">True if directly from assignation, false if from a lower level</param>
        /// <returns>A variable containing the result (type+content)</returns>
        private Variable AnalyseCalcul(string calcul, bool fromAssignation)
        {
            Regex isWithFunctionRegex = new Regex(@"[a-z]+\(.+\)");
            // If lowerer level
            if (IsVariable(calcul))
                try
                {
                    return Program.GetVariable(calcul);
                }
                catch (NotImplementedException e)
                {
                    CatcherError(e.Message);
                }
            if (IsConstant(calcul))
                try
                {
                    return AnalyseConstant(calcul);
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }
            if (IsFunction(calcul))
                try
                {
                    return new Variable("function", AnalyseFunction(calcul, fromAssignation).FunctionType);
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }

            if (!IsScopeValid(calcul))
                throw new ArithmeticException("If function addition, remove it, not handled");

            TypeElement type = GetScopeType(calcul);

            //TODO Analyse du calcul
            if (!isWithFunctionRegex.IsMatch(calcul))
            {
                
            }

            return new Variable("calcul", type);
            // Value can be unknown
        }

        /// <summary>
        /// Analyse a constant, return it as a variable
        /// </summary>
        /// <param name="constant">constant to analyse</param>
        /// <returns></returns>
        private Variable AnalyseConstant(string constant)
        {
            if (!IsConstant(constant))
            {
                Console.Error.WriteLine("Error: constant not valid");
                throw new Exception("Invalid constant");
            }

            if (constant == "vrai" || constant == "faux")
                try
                {
                    return new Variable("constant", constant == "vrai");
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }

            if (new Regex(@"^\d+$").IsMatch(constant))
                try
                {
                    return new Variable("constant", Int32.Parse(constant));
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }

            if (new Regex(@"^\d+\.\d+$").IsMatch(constant))
                try
                {
                    return new Variable("constant", Single.Parse(constant));
                }
                catch (Exception e)
                {
                    CatcherError(e.Message);
                }

            throw new Exception("Not understandable error");
        }
        #endregion

        /// <summary>
        /// Just a ToString, nothing exceptionnal
        /// </summary>
        /// <returns>Line code</returns>
        public override string ToString()
        {
            return Ligne;
        }

        internal string ToPascal()
        {
            if (IsAssignation(Ligne))
                return Ligne.Replace(Properties.Resources.ppv, ":=") + ";";
            if (IsAppelProcedure(Ligne))
                return Ligne + ";";
            if (IsAffiche(Ligne))
            {
                string affKeyWord = "affiche(", donneKeyWord = ")donne";
                string varName = Ligne.Substring(affKeyWord.Length,
                    Ligne.IndexOf(donneKeyWord, StringComparison.CurrentCulture) - affKeyWord.Length);
                string rc = "WriteLn(";
                rc += varName + ");";
                return rc;
            }
            throw new NotImplementedException();
        }
    }
}