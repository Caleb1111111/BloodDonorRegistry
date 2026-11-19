namespace BloodDonorRegistry.Data;

public interface IReportRepository
{
    int CountDonors();
    int CountDonations();
    int CountDonationsThisMonth();
    Dictionary<string, int> DonorsPerBloodGroup();
    // key = first day of the month
    Dictionary<DateTime, int> DonationsPerMonth(DateTime fromMonth);
}