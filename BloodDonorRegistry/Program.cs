using BloodDonorRegistry.Views;

namespace BloodDonorRegistry;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new AppServices();
        try
        {
            services.Auth.EnsureDefaultOfficer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("LifeDrop can't reach the database.\n\n" + ex.Message,
                            "LifeDrop", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        while (true)
        {
            using var login = new LoginForm(services.Auth);
            if (login.ShowDialog() != DialogResult.OK) break;

            using var dashboard = new DashboardForm(login.LoggedInName, services);
            // Retry = the officer clicked "Sign out"; anything else closes the app
            if (dashboard.ShowDialog() != DialogResult.Retry) break;
        }
    }
}