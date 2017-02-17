namespace analysePseudoCode
{
    internal class Function : Procedure
    {
        internal TypeElement TypeFunction { get; }

        public Function(string nom, TypeElement type, params Parameter[] listeParam) : base(nom, listeParam)
        {
            TypeFunction = type;
        }

        public override string ToString()
        {
            string param = "";
            foreach (Parameter parameter in ListeParametres)
            {
                param += "  " + parameter + "\n";
            }
            return $"Function {Nom} {{Type: {TypeFunction}, ListeParam: \n{param}}}";
        }
    }
}