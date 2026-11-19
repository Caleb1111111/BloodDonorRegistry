using BloodDonorRegistry.Views;

namespace BloodDonorRegistry;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var login = new LoginForm();
        if (login.ShowDialog() == DialogResult.OK)
            Application.Run(new DashboardForm(login.LoggedInName));
    }
}