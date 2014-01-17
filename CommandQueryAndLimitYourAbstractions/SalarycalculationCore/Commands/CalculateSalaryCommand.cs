using System;
using System.Linq;
using Infrastructure.DataModel;
using Infrastructure.DomainBaseClasses;
using SalarycalculationCore.Events;
using SalarycalculationCore.Queries;

namespace SalarycalculationCore.Commands
{
    public class CalculateSalaryCommand : Command
    {
        private readonly string _employeeId;
        private readonly decimal _grossSalary;
        private readonly DateTime _periodEndDate;
        private readonly DateTime _periodStartDate;
        private readonly BlogdemoSQLEntities db = new BlogdemoSQLEntities();

        public CalculateSalaryCommand(decimal grossSalary, string employeeId, DateTime periodStartDate,
            DateTime periodEndDate)
        {
            _grossSalary = grossSalary;
            _employeeId = employeeId;
            _periodStartDate = periodStartDate;
            _periodEndDate = periodEndDate;
        }

        public override void Execute()
        {
            //If the execution of this command becomes overly complex we can create BL classes (and possibly new
            //abstractions) in the BL folder. This command would then become more like an orchestrator type of a
            //thing.

            if (QueryRunner.ExecuteQuery(
                new DoesEmployeeHaveSalarycalculatedForPeriod(db, _employeeId, _periodStartDate, _periodEndDate)))
            {
                MessageBus.Send(new SalaryCalculationFailedEvent("Salary already paid for this period"));
                return;
            }

            string idString = _employeeId;

            Taxcard taxCard = db.Taxcards.SingleOrDefault(card => card.EmployeeId == idString);
            if (taxCard == null)
            {
                MessageBus.Send(new SalaryCalculationFailedEvent("Employee does not have a tax card"));
                return;
            }

            decimal taxPercentage = taxCard.TaxPercentage;
            decimal tax = _grossSalary*(taxPercentage/100);
            decimal netSalary = _grossSalary - tax;

            SalaryCalculation salaryCalc = new SalaryCalculation
            {
                EmployeeId = _employeeId,
                GrossAmount = _grossSalary,
                NetAmount = netSalary,
                PeriodStartDate = _periodStartDate,
                PeriodEndDate = _periodEndDate,
                Tax = tax,
                SalaryCalculationId = Guid.NewGuid().ToString()
            };
            db.SalaryCalculations.Add(salaryCalc);

            db.SaveChanges();

            MessageBus.Send(new SalaryCalculationDoneEvent());
        }
    }
}