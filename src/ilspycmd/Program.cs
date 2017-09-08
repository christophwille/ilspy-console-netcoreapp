using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.CSharp.OutputVisitor;
using ICSharpCode.Decompiler.CSharp.Transforms;
using ICSharpCode.Decompiler.TypeSystem;
using ICSharpCode.Decompiler.TypeSystem.Implementation;
using McMaster.Extensions.CommandLineUtils;
using Mono.Cecil;

namespace ilspycmd
{
    class Program
    {
        static int Main(string[] args)
        {
            // https://github.com/natemcmaster/CommandLineUtils/
            // Older cmd line clients (for options reference): https://github.com/aerror2/ILSpy-For-MacOSX and https://github.com/andreif/ILSpyMono
            var app = new CommandLineApplication();

            app.HelpOption("-h|--help");
            var argAssembly = app.Argument("Assembly name", "The assembly that is being parsed");

            app.OnExecute(() => {
                // HACK : the CommandLineUtils package does not allow us to specify an argument as mandatory.
                // Therefore we're implementing it as simple as possible.
                if (argAssembly.Value == null) {
                    app.ShowHelp();
                    return -1;
                }
                DecompileToScreen(argAssembly.Value);
                return 0;
            });

            return app.Execute(args);
        }

        static void DecompileToScreen(string assemblyFileName)
        {
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
        }
    }
}
