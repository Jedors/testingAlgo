using System;
using System.Collections.Generic;

namespace analysePseudoCode
{
    internal class Procedure
    {
        internal List<Parameter> ListeParametres { get; }
        internal string Nom { get; }

        internal bool IsListMatch(List<Parameter> liste)
        {
            if (ListeParametres.Count == liste.Count)
            {
                for (int i = 0; i < ListeParametres.Count; i++)
                {
                    if (ListeParametres[i].TypeParam.Type != liste[i].TypeParam.Type &&
                        ListeParametres[i].TypeParam.Type != TypeEnum.Unknown &&
                        liste[i].TypeParam.Type != TypeEnum.Unknown)
                    {
                        return false;
                    }

                    if (ListeParametres[i].TypePass != liste[i].TypePass &&
                        ListeParametres[i].TypePass != TypePassage.Unknown &&
                        liste[i].TypePass != TypePassage.Unknown)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public Procedure(string nom, params Parameter[] listeParam)
        {
            Nom = nom;
            ListeParametres = new List<Parameter>();
            if (listeParam.Length > 0)
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
                    ListeParametres.Add(param);
                }
            }
        }

        public override string ToString()
        {
            string param = "";
            foreach (Parameter parameter in ListeParametres)
            {
                param += "  " + parameter + "\n";
            }
            return $"Procedure {Nom} {{ListeParam: \n{param}}}";
        }
    }
}