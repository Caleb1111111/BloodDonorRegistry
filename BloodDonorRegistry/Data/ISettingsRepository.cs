namespace BloodDonorRegistry.Data;

public interface ISettingsRepository
{
    int GetWaitingMonths();
    void SetWaitingMonths(int months);
}