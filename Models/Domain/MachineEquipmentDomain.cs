using Models.Entities;

namespace Models.Domain
{
    
    public class MachineEquipmentDomain
    {
        public required VendingMachine  Machine { get; set; }
        
        public string ElectricityStatusPath => $"../../../Assets/Equipment/power{Machine.MachineEquipment?.ElectricityStatus}.png";
        
        public string ChangeStatusPath => $"../../../Assets/Equipment/cash{Machine.MachineEquipment?.ChangeStatus}.png";
        
        public string CardsStatusPath => $"../../../Assets/Equipment/cards{Machine.MachineEquipment?.CardsStatus}.png";
        
        public string ChequesStatusPath => $"../../../Assets/Equipment/cheques{Machine.MachineEquipment?.ChequesStatus}.png";
        
        public string MonitorStatusPath => $"../../../Assets/Equipment/display{Machine.MachineEquipment?.MonitorStatus}.png";
        
        public string ModemStatusPath => $"../../../Assets/Equipment/modem{Machine.MachineEquipment?.ModemStatus}.png";
        
        public string ContactlessPaymentStatusPath => $"../../../Assets/Equipment/contactless{Machine.MachineEquipment?.ContactlessPaymentStatus}.png";
       
    }
}
