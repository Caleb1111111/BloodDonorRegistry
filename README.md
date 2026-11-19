# LifeDrop – Blood Donor Readiness Tracker

A Windows desktop app that helps a blood-drive registry officer keep donor
records, log donations, and see at a glance **who is ready to give again**.
Built with C# (.NET 10), Windows Forms and PostgreSQL, using the MVC pattern.

> Academic project for the Dot Net (C#) course, Monday Group A.
> Not an official health-organisation system. All sample data is fictional.

**Author:** Joseph Caleb Gisubizo

## Features
| Area | What it does |
|---|---|
| Login | Officer signs in with a username and password (stored as a salted hash). |
| Donors | Register, view, edit, delete, search by name or national ID, filter by blood group. Duplicate national IDs and donors under 18 are rejected. A donor who has donations cannot be deleted. |
| Donations | Record a donation (donor, date, place, amount). The date cannot be in the future. View every donation, or one donor's full history. |
| Readiness | Each donor shows **Ready** or **Waiting**, with the days left. |
| Emergency finder | Pick a blood group and get the donors who are Ready today. |
| Settings | The officer sets the waiting period in months. |
| Reports | Total donors, donors per blood group, donations per month. |

### Business rule
A donor is **Ready** when `last donation + waiting period (months) <= today`,
or when they have never donated. The default waiting period is 3 months.
LifeDrop is not a medical check: staff still decide whether a person may donate.

## Requirements
- Windows 10/11
- Visual Studio 2022 or later with the **.NET desktop development** workload
- .NET 10 SDK
- PostgreSQL 16 or newer (with pgAdmin)

## Setup
1. In pgAdmin, create a database named `blood_donor_registry`.
2. Open the Query Tool on it and run `database/schema.sql`.
3. (Optional demo data) Run `database/sample_data.sql` **once**.
4. In the `BloodDonorRegistry` project folder, copy `appsettings.sample.json`
   to `appsettings.json` and put your PostgreSQL password in it.
   `appsettings.json` is git-ignored so your password is never committed.
5. Open `BloodDonorRegistry.slnx` in Visual Studio and press **F5**.
6. Sign in with the demo account created on first run:
   **username** `officer` · **password** `LifeDrop2026`

## Architecture (MVC)
```
View  ->  Controller  ->  Repository  ->  PostgreSQL
(forms)   (rules)         (SQL only)
```
BloodDonorRegistry/
Models/ Donor, Donation, User, DonorReadiness, ...
Views/ LoginForm, DashboardForm, DonorsPage, DonationsPage, ...
Controllers/ Auth, Donor, Donation, Readiness, Report, Settings controllers
Data/ Repository interfaces + PostgreSQL (Npgsql) implementations
Helpers/ UI theme, password hashing, chart and grid helpers
database/ schema.sql, sample_data.sql

Views never run SQL; controllers hold the rules; repositories use
parameterised queries only.

## Known limitations
- Donations cannot be edited or deleted.
- The Emergency finder matches the exact blood group only.
- One officer role; no change-password screen yet.
