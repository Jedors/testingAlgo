namespace analysePseudoCode
{
    /// <summary>
    /// A function is a procedure, with a return type
    /// </summary>
    internal class Function : Procedure
    {
        /// <summary>
        /// Return type of the function
        /// </summary>
        internal TypeElement FunctionType { get; }

        /// <summary>
        /// Constructor of the function, like a procedure, with a type added
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="type">Function type</param>
        /// <param name="listeParam">Parameter list of the function</param>
        public Function(string name, TypeElement type, params Parameter[] listeParam) : base(name, listeParam)
        {
            FunctionType = type;
        }

        /// <summary>
        /// Basic ToString, simply format properly the thing
        /// </summary>
        /// <returns>Beautiful formating of the function</returns>
        public override string ToString()
        {

            return $"Function {Name} {{Type: {FunctionType}, {base.ListParameter()}}}";
        }
    }
}