# Coaching platform backend

Backend implementation of the supplied technical plan: ASP.NET Core 10 controllers, SQL Server, EF Core migrations, Identity, JWT sessions, Cloudinary, and Resend. The Angular application is a separate deliverable.

## Code arrangement

- `src/Coaching.Domain`: individual entities, enums, and business rules.
- `src/Coaching.Application`: feature folders for Auth, Clients, Exercises, Training, Diet, Progress, and Privacy. Handlers, request/response records, validators, and mapping classes have separate files.
- `src/Coaching.Infrastructure`: EF mappings and migrations, Identity/token operations, image processing, Cloudinary, Resend outbox, and scheduled jobs.
- `src/Coaching.Api/Controllers`: controller routes and actions. Each action has its own file using a partial controller; the controller's route and authorization attributes live in its main file. There are no minimal API feature endpoints.
- `tests/Coaching.Tests`: domain, time-zone, image, architecture, and SQL Server integration tests. Each test method has its own file.

Classes with several operations use partial declarations to keep each method in its own file. Generated EF migrations keep EF's standard layout.

## Local setup

Install the .NET SDK selected by `global.json` and SQL Server. Run commands from the solution root.

For local Development startup, run `dotnet run --project src/Coaching.Api --launch-profile https`. Development settings default to the local `Coaching` SQL Server database and generate an in-memory JWT signing key when none is configured. That temporary key changes on restart; set `Jwt:Key` in user secrets for persistent sessions. Create the HTTPS development certificate with `dotnet dev-certs https`; `dotnet dev-certs https --trust` additionally lets your browser trust it.

Cloudinary, Resend, coach contact, and privacy-policy settings may be omitted during Development startup. Missing photo storage returns a service-unavailable response, missing Resend credentials prevent sends, and an unconfigured coach number produces an empty WhatsApp link. Set the actual values to use those features. Daily jobs are disabled by default in Development; set `Jobs:Enabled=true` after configuring the integrations. Production still requires all settings below and never generates a signing key.

```powershell
dotnet restore
dotnet tool restore
dotnet dev-certs https --trust
```

Set values with `dotnet user-secrets set --project src/Coaching.Api <key> <value>`. The full configuration requires the following keys; the Development-only exceptions above let the API start before integration setup:

- `ConnectionStrings:SqlServer`: the application database connection. For a local Windows SQL Server, use `Server=localhost;Database=Coaching;Integrated Security=true;Encrypt=true;TrustServerCertificate=true`. Trust the server certificate only in development.
- `Jwt:Key`: a randomly generated secret of at least 32 characters.
- `Platform:AngularOrigin`: the Angular HTTPS origin, such as `https://localhost:4200`.
- `Platform:CoachWhatsApp`: international phone number, digits only.
- `Platform:PrivacyPolicyUrl`: the published privacy policy's HTTPS URL.
- `Cloudinary:CloudName`, `Cloudinary:ApiKey`, `Cloudinary:ApiSecret`.
- `Resend:ApiKey`, `Resend:From`: a sender on your verified Resend domain.

No real credentials are checked into source control. Resend domain verification, SPF/DKIM, Cloudinary account limits/region, and the policy text must be supplied by the owner.

Apply the reviewed migrations to create the database and required roles, then create the one coach:

```powershell
dotnet ef database update --project src/Coaching.Infrastructure --startup-project src/Coaching.Api
dotnet user-secrets set --project src/Coaching.Api Bootstrap:Email '<coach-email>'
dotnet user-secrets set --project src/Coaching.Api Bootstrap:Password '<strong-password>'
dotnet run --project src/Coaching.Api --launch-profile https -- --initialize
dotnet user-secrets remove --project src/Coaching.Api Bootstrap:Password
dotnet run --project src/Coaching.Api --launch-profile https
```

The initialization command is idempotent for the existing coach's email and refuses to add a second coach. Client registration works after migrations even before a coach account is initialized. The role migration preserves existing role IDs and assignments. If the Client role is missing, registration returns `503` with `code=registration_unavailable` without creating a partial account. Normal startup never applies migrations or creates a coach. The migration command uses the development configuration/user secrets when run through the startup project; alternatively set `ConnectionStrings__SqlServer` explicitly.

OpenAPI: `https://localhost:7443/openapi/v1.json`. It is public in Development and requires the Coach policy in other environments. Health: `GET /health` checks database connectivity without returning database details.

## API usage

All business routes start with `/api/v1`. OpenAPI describes the controller request and response schemas.

For manual testing, open `src/Coaching.Api/Coaching.Api.http`. Set the variables at the top, then send individual requests in order. Named responses supply tokens, IDs, and row versions to later requests. Photo upload, password recovery, refresh cookies, and destructive actions include setup notes. Do not use Run All: the optional deletion requests remove test data.

- Auth: `POST /auth/register`, `/auth/login`, `/auth/refresh`, `/auth/logout`, `/auth/forgot-password`, `/auth/reset-password`.
- Client: `GET /me`, `/me/training`, `/me/diet`, `/me/progress`; `POST /me/workouts`, `/me/check-ins`; selections under `/me/training/exercises/{id}/selection` and `/me/diet/meals/{id}/selection`.
- Photos: send the image bytes directly to `POST /me/check-ins/{id}/photos`, with `Content-Type: image/jpeg`, `image/png`, or `image/webp`. Maximum 8 MiB; static images only. Upload separately after the check-in saves so storage outages never discard measurements. Photo links returned by progress/export expire after five minutes; reload the data for fresh links.
- Coach: clients and access actions under `/coach/clients`; library under `/coach/exercises`; create/update plans under `/coach/training-plans` and `/coach/diet-plans`; template lists, details, and copying under `/coach/templates/training` and `/coach/templates/diet`.
- Locks and coach selections: `/coach/clients/{clientId}/training/lock`, `/training/exercises/{id}/lock`, `/training/exercises/{id}/selection`, and the corresponding diet `/diet/lock`, `/diet/meals/{id}/lock`, `/diet/meals/{id}/selection`.
- Progress and feedback: `/coach/clients/{clientId}/progress`, `/check-ins/{id}/feedback`.
- Privacy: `GET /me/export`, `DELETE /me`, and coach equivalents `/coach/clients/{clientId}/export` and `DELETE /coach/clients/{clientId}`. These client privacy routes remain available after access ends. Deletion requires the account row version in a JSON body.

Auth requests must include `X-Coaching-Client: Angular`. Browser requests must originate from the configured Angular origin. Send cookies with `withCredentials: true` and keep the short-lived access token in memory. Refresh tokens are returned only as HttpOnly, Secure, SameSite=Strict cookies scoped to `/api/v1/auth`. Serialize browser refresh calls: reuse revokes the token family. A family has an absolute 30-day lifetime. Password reset also invalidates existing access tokens and refresh sessions.

Authorization uses roles, a database access-status check on every active-client request, and request-bound EF ownership filters on every client-owned table. Clients register Pending with explicit consent. Their account response includes the Arabic WhatsApp contact link.

Lists accept `page=1&size=20` with a maximum size of 100. Client lists also accept `status` and `search`; exercises accept `search` and `archived`. Progress accepts Cairo `from`/`to` calendar dates, defaults to the last 90 days, and limits a query to 366 days. Export returns all the client's personal records.

Dates of moments are UTC ISO 8601 strings ending in `Z`; calendar dates use `yyyy-MM-dd`. All display/grouping is Africa/Cairo, including DST. Access ends at the final instant of the coach's selected Cairo calendar date. Numeric fields are JSON numbers; errors, validation, WhatsApp messages, and emails are Arabic.

Send the latest base64 `rowVersion` for edits, locks, selections, feedback, and deletes. A `409` with `code=concurrency_conflict` means reload before editing again. Plan creation uses `rowVersion: null`; updates require the current version. Plan updates replace their day/meal structure and regenerate child IDs, so always use the returned plan. Workout history is preserved when old plan items are removed. Template copying creates independent child records, and replacing a client's existing plan requires `existingPlanVersion`.

Every failure has a problem-details response with a stable `code`. Clients receive dedicated response records; EF entities are never returned.

## Scheduled work

The worker runs while the API is running. Reminder scheduling runs once daily after 09:00 Cairo; photo/refresh-token/audit housekeeping runs once daily after 03:00 Cairo. Database job state and SQL application locks avoid duplicate daily execution across instances. A restart later that day catches up that day's work. Keep the host always on.

Reminders are queued three days before the end date, sent through Resend, and marked sent for that specific end date. Extension creates a new reminder opportunity. The durable email outbox encrypts its HTML with ASP.NET Data Protection, retries failures up to five times, and uses Resend idempotency keys. Old payloads are removed after one day. Monitor failed attempts in structured logs. Preserve and protect the Data Protection key ring across restarts and share it if adding instances; otherwise queued emails and password-reset links cannot be decrypted/validated.

Photo cleanup rechecks access under a database lock and removes Cloudinary assets plus photo rows more than seven days after expiry/deactivation. Reactivated clients retain photos. Measurements remain. `Platform:ReminderLeadDays` is configurable; photo retention stays seven days. Audit retention defaults to 365 days and is configurable.

## Verification and release artifacts

Quick unit/architecture/image tests:

```powershell
dotnet test
```

SQL tests are explicitly skipped unless `TestSqlServer` is set. Each creates a random `CoachingTests_*` database, applies the migrations, runs the real controllers with real Identity/SQL Server, and deletes only its own database. Cloudinary and Resend HTTP calls use test doubles. The SQL account needs create/drop database rights for tests; the runtime account does not.

```powershell
$env:TestSqlServer = 'Server=localhost;Integrated Security=true;Encrypt=true;TrustServerCertificate=true'
./scripts/verify.ps1
```

The verification script restores dependencies, builds Release, runs all tests, checks transitive dependency vulnerabilities and missing migrations, generates reviewed idempotent migration SQL, and publishes the API to `artifacts/api`. It exits on a failed check. Use it as the build/test/security/artifact stages of your hosting pipeline. Artifacts are ignored by source control.

For the pre-launch load test, prepare an active staging client with a representative training plan and progress history. Set `LOAD_TEST_ACCESS_TOKEN` to that client's JWT and run:

```powershell
python scripts/load_test.py https://api.your-staging-domain.example
```

The default run uses five workers and 200 measured requests across plan/progress, verifies 200 responses, records median/p95 latency in `artifacts/load-test-summary.json`, and fails if p95 exceeds 200 ms. HTTPS certificate verification stays enabled; `--ca-file` supports a local test CA. Rate limits cap runs to avoid benchmarking throttling. The SQL test suite also runs 200 concurrent controller reads against a seeded three-day training plan and twelve weeks of progress, recording `artifacts/sql-load-test-summary.json`. That in-process check excludes network/TLS and records performance without a hardware-dependent assertion; it does not replace the staging check.

## Hosting handoff

Local, staging, and production must use different databases and secrets. Production environment variables use double underscores, for example `ConnectionStrings__SqlServer` and `Cloudinary__ApiSecret`. Set `AllowedHosts` to the actual API hostname.

Hosting is intentionally unselected because the plan leaves Azure versus VPS open. No cloud resources have been created and nothing has been deployed. After hosting is chosen, deploy the published API with HTTPS, configured secrets, and a persistent protected Data Protection key ring. Apply `artifacts/migrations.sql` as a controlled pipeline step using a migration account; give the API login only the required CRUD and application-lock permissions. The API does not require schema-change rights.

If a reverse proxy terminates HTTPS, configure that host's trusted forwarded headers and known proxy addresses before enabling traffic. The application does not trust arbitrary forwarded IP headers. Route structured error logs to the host's alerting system, schedule daily encrypted SQL backups, and periodically restore to a separate recovery database to verify them. Those infrastructure operations need the selected host and database administrator access. Live Cloudinary/Resend delivery and the staging load test also require the owner's configured service accounts.

The implementation remains within the backend plan: one coach, external payment agreement, Arabic first, no native mobile application or additional product features.
