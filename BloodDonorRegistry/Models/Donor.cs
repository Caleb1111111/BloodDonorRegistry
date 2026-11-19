namespace BloodDonorRegistry.Models;

public class Donor
{
    public int DonorId { get; set; }
    public string NationalId { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public string District { get; set; } = "";
    public string BloodGroup { get; set; } = "";

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}