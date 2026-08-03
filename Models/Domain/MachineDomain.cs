using Models.Entities;

namespace Models.Domain;

public class MachineDomain
{
    public VendingMachine Machine { get; set; }
    
    public string StatusPath => $"../../../Assets/General/{Machine.WorkStatusId}.png";
    
    public string ProviderPath =>$"../../../Assets/Providers/{Machine.MachineProvider.CompanyName}.png";
    
    public string PingPath => $"../../../Assets/Signals/signal{Machine.MachineProvider.Ping}.png";
    
    public string LastCashCollection
    {
        get
        {
            try
            {
                var last = Machine.CashCollections
                    .OrderByDescending(x => x.DateTime).FirstOrDefault();
                var now = DateTime.Now;
                var diff = now - last.DateTime;
                if (diff.TotalDays >= 1)
                {
                    return $"{diff.Days} д. назад";
                }
                if (diff.TotalHours >= 1)
                {
                    return $"{diff.Hours} ч. назад";
                }
                return $"{diff.Minutes} мин. назад";
            }
            catch
            {
                return "";
            }
        }
    }

    public string LastSell
    {
        get
        {
            try
            {
                var last = Machine.Sells.OrderByDescending(x => x.DateTime).FirstOrDefault();
                var now = DateTime.Now;
                var diff = now - last.DateTime;
                if (diff.TotalDays >= 1)
                {
                    return $"{diff.Days} д. назад";
                }
                if (diff.TotalHours >= 1)
                {
                    return $"{diff.Hours} ч. назад";
                }
                return $"{diff.Minutes} мин. назад";
            }
            catch
            {
                return "";
            }
        }
    }
}