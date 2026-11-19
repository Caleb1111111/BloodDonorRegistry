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

        using var login = new LoginForm(services.Auth);
        if (login.ShowDialog() == DialogResult.OK)
            Application.Run(new DashboardForm(login.LoggedInName, services));
    }
}