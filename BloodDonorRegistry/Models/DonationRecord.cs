namespace BloodDonorRegistry.Models;

public class DonationRecord
{
    public int DonationId { get; set; }
    public int DonorId { get; set; }
    public string DonorName { get; set; } = "";
    public string BloodGroup { get; set; } = "";
    public DateTime DonationDate { get; set; }
    public string Place { get; set; } = "";
    public int AmountMl { get; set; }
}