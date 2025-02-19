using LibraryGenerator.SyntaxRewriter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace LibraryGenerator
{
    public class CSharpAnalyzer
    {
        private CSharpCompilation _compilation;
        private readonly IReadOnlyCollection<ISyntaxRewriter> _syntaxRewriter = new List<ISyntaxRewriter>
        {
            new PragmaRemover(),
            new DuplicateTypeRemover(),
            new NativeTypeNameRewriter(),
            new DllImportRewriter(),
            new UnsafeRewriter(),
            new OperatorRewriter()
        };

        public CSharpAnalyzer(string projectDirectory)
        {
            _compilation = CSharpCompilation.Create(Path.GetDirectoryName(projectDirectory),
                options:new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            foreach (string filePath in Directory.EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories))
            {
                Console.WriteLine($"\tAdding {Path.GetFileName(filePath)} to syntax trees.");
                SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath), path: filePath);
                _compilation = _compilation.AddSyntaxTrees(tree);
            }
        }

        public void FixFiles()
        {
            CSharpCompilation newCompilation = _compilation;

            foreach (ISyntaxRewriter rewriter in _syntaxRewriter)
            {
                Console.WriteLine($"Running SyntaxRewriter: {rewriter}");

                foreach (SyntaxTree sourceTree in _compilation.SyntaxTrees)
                {
                    var newSourceRoot = rewriter.Visit(sourceTree.GetRoot());

                    if (rewriter.NeedsFixupVisit)
                    {
                        newSourceRoot = rewriter.FixupVisit(newSourceRoot)!;
                    }

                    if (newSourceRoot != sourceTree.GetRoot())
                    {
                        File.WriteAllText(sourceTree.FilePath, newSourceRoot.ToFullString());

                        newCompilation = newCompilation.ReplaceSyntaxTree(sourceTree, sourceTree.WithChangedText(newSourceRoot.GetText()));
                    }
                }

                _compilation = newCompilation;
            }
        }
    }
}