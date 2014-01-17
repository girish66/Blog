using System;
using System.Linq;
using Infrastructure.DataModel;
using Infrastructure.DomainBaseClasses;

namespace SalarycalculationCore.Queries
{
    public class DoesEmployeeHaveSalarycalculatedForPeriod : Query<bool>
    {
        private readonly string _employeeId;
        private readonly DateTime _endDate;
        private readonly BlogdemoSQLEntities _entities;
        private readonly DateTime _startDate;

        public DoesEmployeeHaveSalarycalculatedForPeriod(BlogdemoSQLEntities entities, string employeeId,
            DateTime startDate, DateTime endDate)
        {
            _entities = entities;
            _employeeId = employeeId;
            _startDate = startDate;
            _endDate = endDate;
        }

        public override bool Execute()
        {
            return _entities.SalaryCalculations.Any(calc => calc.EmployeeId == _employeeId &&
                calc.PeriodStartDate == _startDate && calc.PeriodEndDate == _endDate);
        }
    }
}