namespace BloodDonorRegistry.Models;

public class Donation
{
    public int DonationId { get; set; }
    public int DonorId { get; set; }
    public DateTime DonationDate { get; set; }
    public string Place { get; set; } = "";
    public int AmountMl { get; set; }
}