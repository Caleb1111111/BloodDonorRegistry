namespace BloodDonorRegistry.Models;

public class DonorReadiness
{
    public Donor Donor { get; set; } = new();
    public DateTime? LastDonation { get; set; }
    public DateTime? ReadyOn { get; set; }
    public bool IsReady { get; set; }
    public int DaysLeft { get; set; }

    public string StatusText => IsReady ? "Ready" : "Waiting";
}