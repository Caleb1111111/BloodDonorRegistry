using BloodDonorRegistry.Data;

namespace BloodDonorRegistry.Controllers;

public class SettingsController
{
    private readonly ISettingsRepository _settings;

    public SettingsController(ISettingsRepository settings) => _settings = settings;

    public int GetWaitingMonths() => _settings.GetWaitingMonths();

    public OperationResult SaveWaitingMonths(int months)
    {
        if (months < 1 || months > 12)
            return OperationResult.Fail("The waiting period must be between 1 and 12 months.");

        _settings.SetWaitingMonths(months);
        return OperationResult.Ok("Saved. Everyone's readiness is now recalculated.");
    }
}