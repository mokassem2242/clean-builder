using System.Text.RegularExpressions;

namespace CleanArchitectureGenerator.Services;

public static class InputValidator
{
    private static readonly Regex ValidNameRegex = new(@"^[A-Za-z][A-Za-z0-9_]*(\.[A-Za-z][A-Za-z0-9_]*)*$", RegexOptions.Compiled);

    public static ValidationResult ValidateSolutionName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationResult.Failure("Solution name cannot be empty.");
        }

        if (!ValidNameRegex.IsMatch(name))
        {
            return ValidationResult.Failure("Solution name must be a valid identifier (e.g., Company.Product).");
        }

        return ValidationResult.Success();
    }

    public static ValidationResult ValidateNamespace(string? namespaceValue)
    {
        if (string.IsNullOrWhiteSpace(namespaceValue))
        {
            return ValidationResult.Failure("Namespace cannot be empty.");
        }

        if (!ValidNameRegex.IsMatch(namespaceValue))
        {
            return ValidationResult.Failure("Namespace must be a valid .NET namespace (e.g., Company.Product).");
        }

        return ValidationResult.Success();
    }

    public static ValidationResult ValidateDotNetVersion(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            return ValidationResult.Failure(".NET version cannot be empty.");
        }

        // Basic validation for .NET version format (netX.Y)
        if (!Regex.IsMatch(version, @"^net\d+\.\d+$"))
        {
            return ValidationResult.Failure(".NET version must be in format 'netX.Y' (e.g., net9.0, net8.0).");
        }

        return ValidationResult.Success();
    }
}

public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string? ErrorMessage { get; private set; }

    private ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Success() => new(true);
    public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
}

