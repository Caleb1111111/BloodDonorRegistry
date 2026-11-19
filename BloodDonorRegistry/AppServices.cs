using BloodDonorRegistry.Controllers;
using BloodDonorRegistry.Data;

namespace BloodDonorRegistry;

// Creates the controllers once and hands them to the screens.
public class AppServices
{
    public AuthController Auth { get; }
    public DonorController Donors { get; }
    public DonationController Donations { get; }
    public ReadinessController Readiness { get; }
    public SettingsController Settings { get; }
    public ReportController Reports { get; }

    public AppServices()
    {
        var settings = new SettingsRepository();

        Auth = new AuthController(new UserRepository());
        Donors = new DonorController(new DonorRepository());
        Donations = new DonationController(new DonationRepository());
        Readiness = new ReadinessController(new ReadinessRepository(), settings);
        Reports = new ReportController(new ReportRepository(), Readiness);
        Settings = new SettingsController(settings);
    }
}