Business Requirements Document (BRD)
System Name

Clean Architecture Solution Generator

1. Purpose

The purpose of this system is to provide an interactive command-line tool that generates a ready-to-use .NET solution following Clean Architecture principles with Domain-Driven Design (DDD) alignment.

The tool aims to:

Eliminate repetitive project setup

Enforce architectural boundaries by default

Provide a consistent, professional baseline for freelance and enterprise projects

Reduce decision fatigue during project initialization

2. Business Objectives
Primary Objectives

Generate a .NET solution with predefined Clean Architecture layers

Ensure correct project references and dependencies

Enforce domain isolation from infrastructure and frameworks

Provide a repeatable and customizable solution scaffold

Secondary Objectives

Support different project configurations (API-only, modular, CQRS-ready)

Be extensible for future enhancements

Enable AI-assisted code generation workflows

3. Target Users

Freelance .NET developers

Full-stack developers transitioning toward software architecture

Small-to-medium development teams

Consultants building multiple client systems

4. Scope
In Scope

CLI-based interactive system

Generation of .NET solutions and projects

Folder structure creation

Project reference wiring

Optional feature toggles

Out of Scope (Initial Version)

GUI interface

Code generation beyond basic placeholders

Authentication scaffolding

Deployment pipelines

5. Functional Requirements
FR-1: Interactive CLI Interface

The system SHALL provide an interactive command-line interface that prompts the user for required inputs.

Required Inputs

Solution Name

Base Namespace

Target .NET version (default: latest LTS)

Selected architecture layers

FR-2: Layer Selection

The system SHALL allow the user to select which architectural layers to include.

Supported Layers

Domain

Application

Infrastructure

API

Each layer SHALL be optional, except Domain, which is mandatory.

FR-3: Project Generation

For each selected layer, the system SHALL:

Create a corresponding .NET project

Apply standardized naming conventions

Remove default boilerplate files (e.g., Class1.cs)

Project Types
Layer	Project Type
Domain	Class Library
Application	Class Library
Infrastructure	Class Library
API	ASP.NET Core Web API
FR-4: Folder Structure Generation

The system SHALL generate a predefined folder structure per layer.

Domain Project Structure
Domain
 ├── Entities
 ├── ValueObjects
 ├── Aggregates
 ├── Specifications
 ├── DomainServices
 ├── DomainEvents
 ├── Exceptions
 └── Common

Application Project Structure
Application
 ├── UseCases
 ├── Interfaces
 ├── DTOs
 ├── Validators
 ├── Mappings
 └── Common

Infrastructure Project Structure
Infrastructure
 ├── Persistence
 │    ├── DbContext
 │    ├── Configurations
 │    └── Migrations
 ├── Repositories
 ├── Services
 └── Messaging

API Project Structure
API
 ├── Controllers
 ├── Filters
 ├── Middleware
 ├── Contracts
 └── Extensions

FR-5: Project Reference Enforcement

The system SHALL automatically add project references according to Clean Architecture dependency rules.

Dependency Rules

Application → Domain

Infrastructure → Application

API → Application

Domain MUST NOT reference any other project

Any violation of inward dependency SHALL be prevented.

FR-6: Optional Feature Flags

The system SHALL optionally ask the user whether to include:

CQRS structure (Commands / Queries folders)

EF Core infrastructure setup

Test projects (Unit / Integration)

README with architecture guidelines

FR-7: Configuration Artifacts

The system SHALL generate:

.editorconfig

Directory.Build.props

.gitignore

README.md

These artifacts SHALL enforce consistency and architectural discipline.

6. Non-Functional Requirements
NFR-1: Performance

The system SHALL complete generation within 5 seconds for a standard solution.

NFR-2: Usability

CLI prompts SHALL be clear and minimal

Default values SHALL be provided where possible

User SHALL not be required to understand Clean Architecture terminology to proceed

NFR-3: Portability

The system SHALL be executable on:

Windows (PowerShell)

macOS (Bash/Zsh)

Linux (Bash)

NFR-4: Maintainability

Script logic SHALL be modular

Layer definitions SHALL be configurable

Future layers SHALL be easy to add

7. Naming Conventions
Solution Naming
Company.Product

Project Naming
Company.Product.Domain
Company.Product.Application
Company.Product.Infrastructure
Company.Product.API

Namespace Consistency

Namespaces SHALL match project names exactly.

8. Architectural Constraints

Domain layer SHALL contain no framework dependencies

Infrastructure SHALL implement interfaces defined in Application

API SHALL contain no business logic

Specifications SHALL not reference EF Core in Domain

9. Error Handling

The system SHALL:

Validate required inputs

Prevent invalid names or empty values

Abort generation gracefully with meaningful error messages

10. Success Criteria

The system is considered successful if:

A developer can generate a Clean Architecture solution in under 1 minute

Generated solutions compile without errors

Architecture boundaries are enforced by default

The structure is reusable across multiple real projects

11. Future Enhancements (Not Required Now)

Convert script into dotnet new template

Package as .NET global tool

Support modular monolith configuration

Add OpenAPI, logging, and health checks options

12. One-Sentence Summary

This system standardizes the creation of Clean Architecture .NET solutions by automating structure, naming, and dependency rules while remaining lightweight and extensible.

If you want next, I can:

Convert this BRD into technical design specs

Generate prompt-optimized instructions for Cursor / Copilot

Write the first full implementation script

Review an AI-generated version critically

Just tell me what the next step is.

i want it as md formate  file 

Below is the same BRD rewritten cleanly as a Markdown (.md) file.
You can copy-paste it directly into a file like:

clean-architecture-solution-generator.brd.md

# Business Requirements Document (BRD)
## Clean Architecture Solution Generator

---

## 1. System Name

**Clean Architecture Solution Generator**

---

## 2. Purpose

The purpose of this system is to provide an **interactive command-line tool** that generates a **ready-to-use .NET solution** following **Clean Architecture principles** aligned with **Domain-Driven Design (DDD)**.

The system aims to:

- Eliminate repetitive project setup
- Enforce architectural boundaries by default
- Provide a consistent and professional baseline for freelance and enterprise projects
- Reduce decision fatigue during project initialization
- Enable AI-assisted code generation workflows

---

## 3. Business Objectives

### 3.1 Primary Objectives

- Generate a .NET solution with predefined Clean Architecture layers
- Enforce correct project references and dependency direction
- Isolate the Domain layer from infrastructure and frameworks
- Provide a reusable and extensible solution scaffold

### 3.2 Secondary Objectives

- Support different architectural configurations (API-only, CQRS-ready, modular)
- Be extensible for future enhancements
- Serve as a foundation for `dotnet new` templates or global CLI tools

---

## 4. Target Users

- Freelance .NET developers
- Full-stack developers transitioning toward software architecture
- Small-to-medium development teams
- Consultants working on multiple client systems

---

## 5. Scope

### 5.1 In Scope

- Interactive CLI-based system
- .NET solution and project generation
- Folder structure creation
- Project reference wiring
- Optional feature toggles

### 5.2 Out of Scope (Initial Version)

- GUI interface
- Authentication and authorization scaffolding
- CI/CD or deployment pipelines
- Advanced code generation beyond placeholders

---

## 6. Functional Requirements

### FR-1: Interactive CLI Interface

The system SHALL provide an interactive command-line interface that prompts the user for required inputs.

#### Required Inputs
1. Solution name
2. Base namespace
3. Target .NET version (default: latest LTS)
4. Selected architectural layers

---

### FR-2: Layer Selection

The system SHALL allow the user to select which architectural layers to include.

#### Supported Layers
- Domain (mandatory)
- Application
- Infrastructure
- API

Each layer SHALL be optional except the Domain layer.

---

### FR-3: Project Generation

For each selected layer, the system SHALL:

- Create a corresponding .NET project
- Apply standardized naming conventions
- Remove default boilerplate files (e.g., `Class1.cs`)

#### Project Types

| Layer          | Project Type                |
|---------------|-----------------------------|
| Domain         | Class Library               |
| Application    | Class Library               |
| Infrastructure | Class Library               |
| API            | ASP.NET Core Web API        |

---

### FR-4: Folder Structure Generation

The system SHALL generate a predefined folder structure per layer.

#### Domain Project Structure



Domain
├── Entities
├── ValueObjects
├── Aggregates
├── Specifications
├── DomainServices
├── DomainEvents
├── Exceptions
└── Common


#### Application Project Structure



Application
├── UseCases
├── Interfaces
├── DTOs
├── Validators
├── Mappings
└── Common


#### Infrastructure Project Structure



Infrastructure
├── Persistence
│ ├── DbContext
│ ├── Configurations
│ └── Migrations
├── Repositories
├── Services
└── Messaging


#### API Project Structure



API
├── Controllers
├── Filters
├── Middleware
├── Contracts
└── Extensions


---

### FR-5: Project Reference Enforcement

The system SHALL automatically add project references according to Clean Architecture dependency rules.

#### Dependency Rules

- Application → Domain
- Infrastructure → Application
- API → Application
- Domain MUST NOT reference any other project

Inward dependency violations SHALL be prevented.

---

### FR-6: Optional Feature Flags

The system SHALL optionally ask the user whether to include:

- CQRS structure (Commands / Queries)
- EF Core infrastructure setup
- Test projects (Unit / Integration)
- README with architecture guidelines

---

### FR-7: Configuration Artifacts

The system SHALL generate the following artifacts:

- `.editorconfig`
- `Directory.Build.props`
- `.gitignore`
- `README.md`

These artifacts SHALL enforce consistency and architectural discipline.

---

## 7. Non-Functional Requirements

### NFR-1: Performance

The system SHALL complete solution generation within **5 seconds** for a standard configuration.

---

### NFR-2: Usability

- CLI prompts SHALL be minimal and clear
- Default values SHALL be provided where possible
- Users SHALL not need deep architectural knowledge to proceed

---

### NFR-3: Portability

The system SHALL run on:

- Windows (PowerShell)
- macOS (Bash / Zsh)
- Linux (Bash)

---

### NFR-4: Maintainability

- Script logic SHALL be modular
- Layer definitions SHALL be configurable
- New layers SHALL be easy to add

---

## 8. Naming Conventions

### Solution Naming



Company.Product


### Project Naming



Company.Product.Domain
Company.Product.Application
Company.Product.Infrastructure
Company.Product.API


### Namespace Rules

Namespaces SHALL match project names exactly.

---

## 9. Architectural Constraints

- Domain layer SHALL contain no framework dependencies
- Infrastructure SHALL implement interfaces defined in Application
- API SHALL contain no business logic
- Domain Specifications SHALL not reference EF Core

---

## 10. Error Handling

The system SHALL:

- Validate all required inputs
- Prevent invalid or empty names
- Abort gracefully with meaningful error messages

---

## 11. Success Criteria

The system is considered successful if:

- A Clean Architecture solution can be generated in under 1 minute
- The generated solution builds without errors
- Architectural boundaries are enforced by default
- The structure is reusable across real projects

---

## 12. Implementation Plan

### 12.1 Technical Approach

The system SHALL be implemented as a **cross-platform CLI tool** using one of the following approaches:

**Option A: PowerShell Script (Windows-first, cross-platform with PowerShell Core)**
- Pros: Native Windows support, easy to maintain
- Cons: Requires PowerShell Core for macOS/Linux

**Option B: Bash Script with PowerShell fallback**
- Pros: Native Linux/macOS support, Windows via Git Bash/WSL
- Cons: More complex cross-platform logic

**Option C: .NET Console Application (Recommended)**
- Pros: Native cross-platform, can leverage .NET SDK APIs directly
- Cons: Requires .NET SDK installation

**Recommended: Hybrid Approach**
- Primary: .NET Console Application (C#) for core logic
- Alternative: PowerShell script wrapper for quick execution
- Both approaches share the same layer definitions and templates

---

### 12.2 Implementation Phases

#### Phase 1: Foundation & Core CLI (Week 1)

**Deliverables:**
- Interactive CLI interface with input prompts
- Solution and project creation logic
- Basic validation and error handling
- Cross-platform compatibility testing

**Tasks:**
1. Set up project structure for the generator tool
2. Implement interactive prompt system (solution name, namespace, .NET version)
3. Create solution file generation
4. Implement basic project creation (Domain layer only)
5. Add input validation (name format, empty checks)
6. Cross-platform path handling (Windows/macOS/Linux)

**Acceptance Criteria:**
- Can generate a solution with Domain project
- Prompts are clear and user-friendly
- Handles invalid inputs gracefully

---

#### Phase 2: Layer Generation & Structure (Week 1-2)

**Deliverables:**
- Complete layer selection system
- Folder structure generation for all layers
- Project type configuration per layer
- Boilerplate file removal

**Tasks:**
1. Implement layer selection (Domain mandatory, others optional)
2. Create folder structure templates for each layer:
   - Domain (8 folders)
   - Application (6 folders)
   - Infrastructure (4 main folders with subfolders)
   - API (5 folders)
3. Generate projects with correct project types
4. Remove default `Class1.cs` files
5. Create placeholder `.gitkeep` files in empty folders

**Acceptance Criteria:**
- All 4 layers can be generated with correct structure
- Folder hierarchies match BRD specifications
- Projects compile without errors

---

#### Phase 3: Dependency Management (Week 2)

**Deliverables:**
- Automatic project reference wiring
- Dependency rule enforcement
- Validation of dependency direction

**Tasks:**
1. Implement project reference logic:
   - Application → Domain
   - Infrastructure → Application
   - API → Application
2. Add dependency validation (prevent Domain → anything)
3. Handle conditional references (only if layer exists)
4. Test dependency graph correctness

**Acceptance Criteria:**
- References are added automatically
- Dependency violations are prevented
- Solution builds with correct dependency chain

---

#### Phase 4: Optional Features & Configuration (Week 2-3)

**Deliverables:**
- Feature flag system (CQRS, EF Core, Tests, README)
- Configuration artifact generation
- CQRS folder structure (when enabled)

**Tasks:**
1. Implement optional feature prompts:
   - CQRS structure (Commands/Queries in Application)
   - EF Core setup (DbContext, Configurations in Infrastructure)
   - Test projects (Unit/Integration)
   - Architecture README generation
2. Generate configuration files:
   - `.editorconfig` (C# standards)
   - `Directory.Build.props` (common properties)
   - `.gitignore` (.NET specific)
   - `README.md` (architecture guidelines)
3. Create CQRS folder structure when enabled
4. Generate EF Core placeholder files when enabled

**Acceptance Criteria:**
- All optional features work independently
- Configuration files enforce standards
- Generated solutions are production-ready

---

#### Phase 5: Testing & Refinement (Week 3)

**Deliverables:**
- Comprehensive testing across platforms
- Performance optimization
- Documentation and examples

**Tasks:**
1. Test on Windows (PowerShell), macOS (Zsh), Linux (Bash)
2. Performance testing (ensure <5 seconds generation)
3. Generate test solutions and verify compilation
4. Create usage documentation
5. Add example outputs and screenshots
6. Error handling refinement

**Acceptance Criteria:**
- Works on all target platforms
- Generation completes in <5 seconds
- Generated solutions build successfully
- Clear error messages for all failure scenarios

---

### 12.3 Technical Architecture

#### Core Components

1. **CLI Interface Module**
   - Interactive prompt handler
   - Input validation
   - User experience flow

2. **Solution Generator**
   - Solution file creation
   - Project creation and configuration
   - Project reference management

3. **Structure Generator**
   - Folder hierarchy creation
   - Template file generation
   - Placeholder management

4. **Configuration Generator**
   - Artifact template engine
   - Feature flag handler
   - Customization logic

5. **Validation Engine**
   - Dependency rule checker
   - Naming convention validator
   - Cross-platform compatibility verifier

---

### 12.4 Dependencies & Prerequisites

**Required:**
- .NET SDK (latest LTS version)
- Access to `dotnet` CLI commands
- File system write permissions

**Optional:**
- PowerShell Core (for PowerShell script variant)
- Git (for `.gitignore` and version control)

---

### 12.5 Risk Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| Cross-platform path issues | High | Use .NET `Path` APIs, test on all platforms early |
| Invalid project references | High | Validate dependency graph before generation |
| Performance degradation | Medium | Optimize file I/O, batch operations |
| User input errors | Medium | Comprehensive validation with clear error messages |
| .NET SDK version conflicts | Low | Detect and validate .NET version availability |

---

### 12.6 Development Timeline

**Estimated Duration: 3 weeks**

- **Week 1:** Phases 1-2 (Foundation, Layer Generation)
- **Week 2:** Phases 3-4 (Dependencies, Optional Features)
- **Week 3:** Phase 5 (Testing, Refinement, Documentation)

**Milestones:**
- ✅ M1: Basic CLI with Domain layer generation
- ✅ M2: All layers with folder structures
- ✅ M3: Complete feature set with optional flags
- ✅ M4: Production-ready tool with documentation

---

### 12.7 Testing Strategy

**Unit Testing:**
- Input validation logic
- Naming convention enforcement
- Dependency rule validation

**Integration Testing:**
- End-to-end solution generation
- Cross-platform compatibility
- Generated solution compilation

**User Acceptance Testing:**
- Real-world scenario testing
- Developer feedback collection
- Usability validation

---

### 12.8 Success Metrics

Track the following metrics post-implementation:

- Generation success rate (target: >95%)
- Average generation time (target: <5 seconds)
- Cross-platform compatibility (target: 100%)
- User satisfaction (target: positive feedback)
- Generated solution build success (target: 100%)

---

## 13. Future Enhancements (Out of Scope)

- Convert to `dotnet new` template
- Package as a .NET global tool
- Support modular monolith configuration
- Add OpenAPI, logging, and health checks

---

## 14. One-Sentence Summary

> This system standardizes the creation of Clean Architecture .NET solutions by automating structure, naming, and dependency rules while remaining lightweight and extensible.