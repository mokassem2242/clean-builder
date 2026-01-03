# Duplicate Project Fix

## Problem

When opening a generated solution in Visual Studio, users were getting the error:
> "The project 'Masar.Test.Infrastructure' cannot be added to the solution because a project with the same project filename already exists in the solution."

## Root Cause

The issue was in the solution file generation logic:

1. **Main Projects**: When adding main projects (Domain, Application, Infrastructure, API), the code used `Replace("Global", ...)` which could cause issues if "Global" appeared multiple times.

2. **Test Projects**: When adding test projects, the code:
   - Used `Replace("Global", ...)` which could replace the wrong occurrence
   - Did not check if the project already existed in the solution
   - Could add the same project multiple times

3. **No Duplicate Checking**: Neither method checked if a project with the same name or path already existed before adding it.

## Solution

### 1. Added Duplicate Checking

Both `AddProjectsToSolutionAsync` and `AddTestProjectToSolutionAsync` now check if a project already exists before adding it:

```csharp
// Check if project already exists in solution
if (solutionContent.Contains($"= \"{projectName}\"") || 
    solutionContent.Contains($"\"{relativePathNormalized}\""))
{
    Console.WriteLine($"  ⚠️  Project {projectName} already exists in solution, skipping...");
    return;
}
```

### 2. Improved Project Insertion

Instead of using `Replace("Global", ...)`, the code now:
- Finds the exact position of "Global" keyword
- Inserts the project entry at that position
- Ensures projects are added in the correct location

```csharp
// Find the position right before "Global" keyword
var globalIndex = solutionContent.IndexOf("Global", StringComparison.Ordinal);
if (globalIndex == -1)
{
    throw new InvalidOperationException("Solution file is missing 'Global' section");
}

// Insert the project entry right before "Global"
solutionContent = solutionContent.Insert(globalIndex, $"{projectsSection}\r\n");
```

### 3. Better Configuration Section Handling

The configuration entries are now inserted at the correct position before `EndGlobalSection`:

```csharp
var endGlobalSectionIndex = solutionContent.LastIndexOf("	EndGlobalSection", StringComparison.Ordinal);
if (endGlobalSectionIndex == -1)
{
    throw new InvalidOperationException("Solution file is missing 'EndGlobalSection'");
}

var testConfigEntries = $"		{{{testProjectGuid}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\r\n...";
solutionContent = solutionContent.Insert(endGlobalSectionIndex, testConfigEntries);
```

## Changes Made

### `AddProjectsToSolutionAsync` Method
- ✅ Added duplicate checking before adding projects
- ✅ Changed from `Replace` to `Insert` for more precise placement
- ✅ Added validation for missing "Global" section
- ✅ Skip projects that already exist

### `AddTestProjectToSolutionAsync` Method (New)
- ✅ Extracted test project addition into separate method
- ✅ Added duplicate checking
- ✅ Uses `Insert` instead of `Replace` for precise placement
- ✅ Validates solution file structure
- ✅ Properly handles configuration entries

## Benefits

1. **No More Duplicates**: Projects are checked before being added
2. **Better Error Handling**: Validates solution file structure
3. **More Reliable**: Uses precise insertion instead of string replacement
4. **Clearer Code**: Separate method for test projects improves maintainability

## Testing

To verify the fix:

1. Generate a new solution with all layers and tests
2. Open the solution in Visual Studio
3. Verify no duplicate project errors
4. Check that all projects appear correctly in Solution Explorer

## Note

If you have an existing solution with duplicate projects, you may need to:
1. Remove the duplicate entries manually from the .sln file, or
2. Regenerate the solution

The fix prevents future duplicates but doesn't automatically fix existing ones.

