# JobPortal — easiest free Render deployment

This deployment package uses SQLite so the app can run without a paid SQL Server.
It is intended for a demo/academic live URL.

## 1. Push this folder to GitHub

Create a repository and upload the contents of `JobPortal-main` (not the outer zip folder).

## 2. Deploy on Render

Open Render and choose:

New → Web Service → Public Git Repository

Select your GitHub repository.

Render should detect the `Dockerfile`.

Use:
- Runtime: Docker
- Plan: Free
- Dockerfile: `./Dockerfile`

Then deploy.

## 3. Important database limitation

The free Render web service has an ephemeral filesystem. The SQLite database is therefore suitable for a demo, but users/jobs can be reset after a redeploy or restart.

For permanent production data, switch the EF Core provider to PostgreSQL and use a persistent PostgreSQL database such as Supabase.

## 4. Email

SMTP credentials are intentionally NOT stored in `appsettings.json`.

If email sending is needed, add these Render environment variables:
- `Smtp__Email`
- `Smtp__Password`

Use a Gmail App Password, not your normal Gmail password.

## 5. Admin login

The existing application currently contains:
- Email: `admin@gmail.com`
- Password: `admin123`

Change this before using the application publicly.
