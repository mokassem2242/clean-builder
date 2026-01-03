# Shared Kernel Implementation (DDD Pattern)

## ✅ Implementation Complete

The generator now supports **Shared Kernel** following Domain-Driven Design (DDD) principles.

## What is Shared Kernel?

In DDD, **Shared Kernel** is a pattern where two or more bounded contexts share some common domain model. It contains:

- Shared entities and value objects
- Common enums and constants
- Shared interfaces and abstractions
- Reusable domain concepts

## Implementation Details

### Layer Definition

**SharedKernel** is added as an optional layer with:

- **Project Type**: Class Library
- **Dependencies**: None (innermost layer)
- **Folder Structure**:
  ```
  SharedKernel/
  ├── Entities/          # Shared entities
  ├── ValueObjects/      # Shared value objects
  ├── Enums/             # Shared enumerations
  ├── Constants/         # Shared constants
  ├── Exceptions/        # Shared exceptions
  ├── Interfaces/        # Shared interfaces
  └── Common/            # Common utilities
  ```

### Dependency Rules

With SharedKernel, the dependency structure becomes:

```
┌─────────────┐
│     API     │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│ Application │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│   Domain    │
└──────┬──────┘
       │
       ↓
┌─────────────┐
│SharedKernel │ ← Innermost (no dependencies)
└─────────────┘
```

**Dependency Rules:**
- ✅ **SharedKernel → Nothing**: SharedKernel has no dependencies
- ✅ **Domain → SharedKernel**: Domain can depend on SharedKernel (if selected)
- ✅ **Application → Domain + SharedKernel**: Application can depend on both
- ✅ **Infrastructure → Application**: Infrastructure depends on Application
- ✅ **API → Application**: API depends on Application

### Usage

When generating a solution:

1. **Select SharedKernel** (optional):
   ```
   Select layers to include (Domain is mandatory):
     Include SharedKernel layer? (Y/n, default: N): Y
     Include Domain layer? (mandatory) (Y/n, default: Y): [Enter]
     ...
   ```

2. **Dependencies are automatically configured**:
   - If SharedKernel is selected, Domain will reference it
   - If SharedKernel is selected, Application will reference it
   - All dependency rules are validated

### Generated Structure

When SharedKernel is included:

```
SolutionName/
├── src/
│   ├── Company.Product.SharedKernel/    # Shared Kernel (if selected)
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   ├── Constants/
│   │   ├── Exceptions/
│   │   ├── Interfaces/
│   │   └── Common/
│   ├── Company.Product.Domain/         # References SharedKernel
│   ├── Company.Product.Application/     # References Domain + SharedKernel
│   ├── Company.Product.Infrastructure/
│   └── Company.Product.API/
└── tests/
```

### Project References

When SharedKernel is selected:

**Domain.csproj:**
```xml
<ItemGroup>
  <ProjectReference Include="../SharedKernel/Company.Product.SharedKernel.csproj" />
</ItemGroup>
```

**Application.csproj:**
```xml
<ItemGroup>
  <ProjectReference Include="../Domain/Company.Product.Domain.csproj" />
  <ProjectReference Include="../SharedKernel/Company.Product.SharedKernel.csproj" />
</ItemGroup>
```

## Benefits

1. **DDD Alignment**: Follows Domain-Driven Design Shared Kernel pattern
2. **Reusability**: Common domain concepts can be shared across contexts
3. **Separation**: Clear separation between shared and context-specific domain logic
4. **Flexibility**: Optional - only include when needed

## When to Use SharedKernel

Use SharedKernel when:
- You have multiple bounded contexts that share domain concepts
- Common entities, value objects, or enums are used across contexts
- You want to avoid duplication of shared domain logic
- You're building a modular monolith or microservices architecture

## Best Practices

1. **Keep it Minimal**: Only include truly shared concepts
2. **No Framework Dependencies**: SharedKernel should remain pure
3. **Version Carefully**: Changes to SharedKernel affect all contexts
4. **Document Shared Elements**: Clearly document what's shared and why

## Validation

The generator validates:
- ✅ SharedKernel has no dependencies
- ✅ Domain can only depend on SharedKernel (if selected)
- ✅ Application can depend on Domain and SharedKernel
- ✅ No circular dependencies

## Example Use Cases

- **Shared Enums**: Status enums used across multiple contexts
- **Common Value Objects**: Money, Address, Email patterns
- **Shared Entities**: User, Tenant (in multi-tenant systems)
- **Common Interfaces**: Domain event interfaces, repository contracts

---

**Note**: SharedKernel is optional. Only include it when you have shared domain concepts that need to be reused across bounded contexts.

