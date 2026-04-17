using System;
using System.Collections.Generic;

namespace residence.application.DTOs
{
    public class HouseFinancialStatementDto
    {
        public Guid HouseId { get; set; }
        public decimal TotalRappelToPay { get; set; }
        public decimal TotalRappelPaid { get; set; }
    }

    public class MonthlyStatementItemDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal ActiveTarifAmount { get; set; }
        public decimal Difference => ActiveTarifAmount - AmountPaid;
    }
}
