# Copilot Instructions for Practica1

Purpose: Make AI agents productive quickly in this .NET Framework 4.8 solution by documenting project structure, workflows, and codebase-specific patterns.

Big picture
- Solution is split into: `Logica` (domain models + validation/hash utils), `Database` (in?memory data store), `I18n` (translation loader), `www` (ASP.NET WebForms web site), and MSTest projects `LogicaTests`, `DatabaseTests`, `I18nTests`. A `wwwTest` project exists for simple web testing.
- Data flow:
  - WebForms pages under `www` consume compiled libraries in `www/Bin` (e.g., `I18n.dll`). Shared data access is via `Database.DataStore.Instance` (singleton `CapaDatos`).
  - `Database.CapaDatos` holds in?memory lists for `User` and `Activity`, seeds demo data in its constructor, and implements operations defined in `Database.ICapaDatos` plus extras (status/role changes, cascading deletes, etc.).
  - `Logica.Models.User` and `Logica.Models.Activity` define IDs via per?type static auto?increment counters and set `CreatedAt` at construction. Passwords are hashed with `Logica.Utils.PasswordHasher`.
  - `I18n.TranslationService` loads JSON translation files per language and serves string lookup by key.

Build, test, debug
- Target: .NET Framework 4.8, C# 7.3. Build the solution in Visual Studio. Tests use MSTest and are configured to not run in parallel with `[assembly: DoNotParallelize]`.
- Run tests in Test Explorer. Tests depend on data files under `LogicaTests/DatosTest` (JSON/CSV/XML) and load them via `AppDomain.CurrentDomain.BaseDirectory/../../DatosTest`.
- To run the web site, use IIS/IIS Express pointing to the `www` folder. Ensure the app pool targets .NET 4.8 and `www/Bin` contains the compiled assemblies (e.g., `I18n.dll`, and any others you reference from `www`).

Internationalization (I18n)
- Translations live at `www/translations/<lang>/` with JSON files like `dashboard.json`. Keys are looked up as `filename.key` (e.g., `"dashboard.Title"`).
- Call `TranslationService.SetLanguage("ES_es")` (default if null/empty) or `"EN_en"` then `TranslationService.Get("dashboard.Title")`.
- Path resolution prefers `HttpContext.Current.Server.MapPath("~/translations/<lang>")`; in non?web contexts it searches upward for `translations/<lang>` or `www/translations/<lang>`.
- Thread safety: `SetLanguage` is guarded by a lock; returns false when setting the same language or when the language folder is missing.

Data layer patterns
- `CapaDatos` enforces uniqueness on `User.Email` and `User.NIF`; validating credentials uses `PasswordHasher.VerifyPassword(password, user.PasswordHash)`.
- Deleting a user cascades: removes their activities. Status and role changes are explicit methods (`InactivaUsuario`, `ReactivaUsuario`, `PromueveUsuario`, `DegradaUsuario`).
- Activities are user?scoped via `Activity.UserId`. Counting and lookups are simple list queries.

Conventions and gotchas
- IDs for `User`/`Activity` auto?increment via static counters. Tests reset them in `[TestInitialize]` using `ResetIdCounter()`; new tests that depend on IDs should do the same.
- Validations are Spain?centric: `Validate.NIF`, `Validate.IBAN`, `Validate.Email`, `Validate.Telf`. Prefer these helpers for input checks.
- Do not store plaintext passwords. Always hash via `PasswordHasher.HashPassword`. The `User` constructor already hashes the provided `pwd` argument.
- `ICapaDatos` is `internal`; `CapaDatos` provides additional methods not declared in the interface. Use `DataStore.Instance` for a shared `CapaDatos` within the web site.

Examples
- I18n: `TranslationService.SetLanguage("EN_en"); var title = TranslationService.Get("dashboard.Title");`
- User creation: `var u = new User(nif, name, surname, email, pwd, age, telf, gender, weight);` then `CapaDatos.GuardaUsuario(u)`.
- Auth: `CapaDatos.ValidaUsuario(email, password)`; never compare hashes manually.
