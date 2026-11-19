using BloodDonorRegistry.Data; 

namespace BloodDonorRegistry
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using var conn = DbConnectionFactory.Create();
                conn.Open();
                MessageBox.Show("Connected to PostgreSQL!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed: " + ex.Message);
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new DashboardForm());
        }
    }
}