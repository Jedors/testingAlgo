using System;
using System.Collections.Generic;

namespace analysePseudoCode
{
    /// <summary>
    /// Procedure
    /// </summary>
    internal class Procedure
    {
        /// <summary>
        /// Parameter list of the procedure
        /// </summary>
        internal List<Parameter> ParameterList { get; }
        /// <summary>
        /// Name of the procedure
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Check if liste seem to came from the same procedure
        /// </summary>
        /// <param name="liste">list to compare</param>
        /// <returns>True if Is list match</returns>
        internal bool IsListMatch(List<Parameter> liste)
        {
            if (ParameterList.Count == liste.Count)
            {
                for (int i = 0; i < ParameterList.Count; i++)
                {
                    if (ParameterList[i].TypeParam.Type != liste[i].TypeParam.Type &&
                        ParameterList[i].TypeParam.Type != TypeEnum.Unknown &&
                        liste[i].TypeParam.Type != TypeEnum.Unknown) // If type isn't legit
                    {
                        return false;
                    }

                    if (ParameterList[i].TypePass != liste[i].TypePass &&
                        ParameterList[i].TypePass != TypePassage.Unknown &&
                        liste[i].TypePass != TypePassage.Unknown) // If type passage isn't legit 
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Procedure constructor
        /// </summary>
        /// <param name="name">Name of the procedure</param>
        /// <param name="listeParam">List of the parameter</param>
        public Procedure(string name, params Parameter[] listeParam)
        {
            Name = name;
            ParameterList = new List<Parameter>();
            if (listeParam.Length > 0) // Parameter verification
            {
                HashSet<Parameter> paramHashSet = new HashSet<Parameter>();

                foreach (Parameter param in listeParam)
                {
                    paramHashSet.Add(param);
                }

                if (paramHashSet.Count != listeParam.Length)
                {
                    Console.Error.WriteLine("Error: Some parameters are duplicated in object creation");
                    throw new ArgumentException("Number of parameters different from effective number of params.");
                }

                foreach (Parameter param in listeParam)
                {
                    ParameterList.Add(param);
                }
            }
        }

        /// <summary>
        /// Basic ToString, simply format properly the thing
        /// </summary>
        /// <returns>Beautiful formating of the procedure</returns>
        public override string ToString()
        {
            return $"Procedure {Name} {{{ListParameter()}}}";
        }

        public string ListParameter()
        {
            string param = "";
            for (int i = 0; i < ParameterList.Count; i++)
            {
                param += "  " + ParameterList[i].ToString(i + 1) + "\n";
            }

            return "ListeParam: \n" + param;
        }
    }
}