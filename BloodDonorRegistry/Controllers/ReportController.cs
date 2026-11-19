using BloodDonorRegistry.Data;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class ReportController
{
    private readonly IReportRepository _repository;
    private readonly ReadinessController _readiness;

    public ReportController(IReportRepository repository, ReadinessController readiness)
    {
        _repository = repository;
        _readiness = readiness;
    }

    public ReportSummary GetSummary() => new()
    {
        TotalDonors = _repository.CountDonors(),
        TotalDonations = _repository.CountDonations(),
        DonationsThisMonth = _repository.CountDonationsThisMonth(),
        ReadyToday = _readiness.Check("", null, "Ready").Count
    };

    public List<(string Label, int Value)> DonorsPerBloodGroup()
    {
        var counts = _repository.DonorsPerBloodGroup();
        return BloodGroups.All
            .Select(g => (g, counts.TryGetValue(g, out int n) ? n : 0))
            .ToList();
    }

    public List<(string Label, int Value)> DonationsPerMonth(int months = 6)
    {
        var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var first = thisMonth.AddMonths(-(months - 1));
        var counts = _repository.DonationsPerMonth(first);

        var result = new List<(string Label, int Value)>();
        for (int i = 0; i < months; i++)
        {
            var month = first.AddMonths(i);
            result.Add((month.ToString("MMM yyyy"), counts.TryGetValue(month, out int n) ? n : 0));
        }
        return result;
    }
}