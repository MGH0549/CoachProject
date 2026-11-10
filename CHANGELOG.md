# Changelog

## 2026-11-10

- Fixed registration on a newly migrated database by seeding Identity roles independently of coach initialization; added a controlled unavailable response for missing role configuration.

- Fixed Development startup without external service credentials, added local SQL defaults and a temporary JWT signing key, and retained strict production configuration validation.

- Implemented the coaching backend with ASP.NET Core controllers and separate class/use-case/action files.
- Added Identity, JWT rotation/reuse detection, database account access enforcement, and ownership filters.
- Added client lifecycle, exercises, training/diet plans, template copies, selections and coach locks.
- Added workout/check-in history, feedback, private progress photos, privacy export/deletion, and Arabic recovery/reminder emails.
- Added Cairo/DST handling, concurrency checks, migrations, daily retention jobs, tests, and verification/load-test scripts.
