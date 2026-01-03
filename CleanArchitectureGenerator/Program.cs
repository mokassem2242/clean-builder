using CleanArchitectureGenerator.Services;

namespace CleanArchitectureGenerator;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var cli = new CLIInterface();
            var config = cli.PromptForConfiguration();
            
            cli.DisplaySummary(config);

            if (!cli.ConfirmGeneration())
            {
                Console.WriteLine("\n❌ Generation cancelled by user.");
                return;
            }

            // Determine output directory (current directory or specified)
            var outputDirectory = args.Length > 0 ? args[0] : Environment.CurrentDirectory;
            
            var generator = new SolutionGenerator(outputDirectory);
            var success = await generator.GenerateSolutionAsync(config);

            if (success)
            {
                Console.WriteLine("🎉 Done! Your Clean Architecture solution is ready.");
                Console.WriteLine("\nNext steps:");
                Console.WriteLine("  1. Navigate to the solution directory");
                Console.WriteLine("  2. Run: dotnet restore");
                Console.WriteLine("  3. Run: dotnet build");
                Console.WriteLine("  4. Start coding! 🚀\n");
            }
            else
            {
                Environment.ExitCode = 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Fatal error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Environment.ExitCode = 1;
        }
    }
}
