using BloodDonorRegistry.Data;
using BloodDonorRegistry.Models;

namespace BloodDonorRegistry.Controllers;

public class ReadinessController
{
    private readonly IReadinessRepository _repository;
    private readonly ISettingsRepository _settings;

    public ReadinessController(IReadinessRepository repository, ISettingsRepository settings)
    {
        _repository = repository;
        _settings = settings;
    }

    // status: "All", "Ready" or "Waiting"
    public List<DonorReadiness> Check(string text, string? bloodGroup, string status)
    {
        int months = _settings.GetWaitingMonths();
        var list = _repository.GetDonorsWithLastDonation(text.Trim(), bloodGroup);
        foreach (var item in list) Evaluate(item, months);

        return status switch
        {
            "Ready" => list.Where(x => x.IsReady).ToList(),
            "Waiting" => list.Where(x => !x.IsReady).ToList(),
            _ => list
        };
    }

    // Donors of one blood group who can give today. Never-donated first,
    // then the ones who have rested the longest.
    public List<DonorReadiness> Emergency(string bloodGroup)
        => Check("", bloodGroup, "Ready")
            .OrderBy(x => x.LastDonation ?? DateTime.MinValue)
            .ToList();

    // Compatible donors: exact match first, then compatible groups, all Ready.
    // Returns (donor, isExactMatch) so the UI can distinguish them.
    public List<(DonorReadiness Item, bool IsExact)> EmergencyCompatible(string bloodGroup)
    {
        var exact = Emergency(bloodGroup).Select(x => (x, true));

        var compatible = CompatibleDonorGroups(bloodGroup)
            .SelectMany(g => Emergency(g))
            .Select(x => (x, false));

        return exact
            .Concat(compatible)
            .DistinctBy(t => t.x.Donor.DonorId)
            .ToList();
    }

    // Returns the blood groups whose donors can donate to the given recipient group.
    public static IEnumerable<string> CompatibleDonorGroups(string recipientGroup)
        => recipientGroup switch
        {
            "A+"  => new[] { "A-", "O+", "O-" },
            "A-"  => new[] { "O-" },
            "B+"  => new[] { "B-", "O+", "O-" },
            "B-"  => new[] { "O-" },
            "AB+" => new[] { "A-", "B-", "O+", "O-", "A+", "B+", "AB-" },
            "AB-" => new[] { "A-", "B-", "O-" },
            "O+"  => new[] { "O-" },
            "O-"  => Array.Empty<string>(),
            _     => Array.Empty<string>()
        };

    private static void Evaluate(DonorReadiness item, int months)
    {
        if (item.LastDonation is null)
        {
            item.ReadyOn = null;
            item.DaysLeft = 0;
            item.IsReady = true;
            return;
        }

        item.ReadyOn = item.LastDonation.Value.Date.AddMonths(months);
        item.DaysLeft = Math.Max(0, (item.ReadyOn.Value - DateTime.Today).Days);
        item.IsReady = item.DaysLeft == 0;
    }
}