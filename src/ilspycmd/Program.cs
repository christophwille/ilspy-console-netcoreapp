using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;
using Mono.Cecil;

namespace ilspycmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string assemblyFileName = typeof(Program).Assembly.Location;
            assemblyFileName = Path.Combine(Path.GetDirectoryName(assemblyFileName), "owin.dll");

            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyFileName));
            resolver.RemoveSearchDirectory(".");

            var module = ModuleDefinition.ReadModule(assemblyFileName, new ReaderParameters {
                AssemblyResolver = resolver,
                InMemory = true
            });

            var typeSystem = new DecompilerTypeSystem(module);
            CSharpDecompiler decompiler = new CSharpDecompiler(typeSystem, new DecompilerSettings());

            decompiler.AstTransforms.Add(new EscapeInvalidIdentifiers());
            var syntaxTree = decompiler.DecompileWholeModuleAsSingleFile();

            StringWriter output = new StringWriter();
            var visitor = new CSharpOutputVisitor(output, FormattingOptionsFactory.CreateSharpDevelop());
            syntaxTree.AcceptVisitor(visitor);

            Console.WriteLine(output);
            Console.Read();
        }
    }
}
