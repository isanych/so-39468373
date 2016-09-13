using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Semantics;
using System.Linq;

namespace so39468373
{
    internal static class Program
    {
        private static void Main()
        {
            var tree = CSharpSyntaxTree.ParseText(@"
public class c
{
    public static c operator ++(c o) { return o; }
    static c pre(c o) => ++o;
    static c post(c o) => o++;
    public static void Main() {}
}");
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create(null, new[] { tree }, new[] { mscorlib });
            var model = compilation.GetSemanticModel(tree);
            foreach (var node in tree.GetRoot().DescendantNodes().OfType<ArrowExpressionClauseSyntax>())
            {
                var operation = model.GetOperation(node);
                var block = (IBlockStatement)operation;
                var statement = (IReturnStatement)block.Statements.First();
                var increment = (IIncrementExpression)statement.ReturnedValue;
                // How to get lowered operations for increment here?
            }
        }
    }
}
