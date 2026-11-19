using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Data;
using BloodDonorRegistry.Views;

namespace BloodDonorRegistry;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var auth = new AuthController(new UserRepository());
        try
        {
            auth.EnsureDefaultOfficer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("LifeDrop can't reach the database.\n\n" + ex.Message,
                            "LifeDrop", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var login = new LoginForm(auth);
        if (login.ShowDialog() == DialogResult.OK)
            Application.Run(new DashboardForm(login.LoggedInName));
    }
}