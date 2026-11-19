namespace BloodDonorRegistry.Models;

public class ReportSummary
{
    public int TotalDonors { get; set; }
    public int TotalDonations { get; set; }
    public int DonationsThisMonth { get; set; }
    public int ReadyToday { get; set; }
}