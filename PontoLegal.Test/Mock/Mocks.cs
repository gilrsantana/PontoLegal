namespace PontoLegal.Test.Mock
{
    internal static class Mocks
    {
        internal static EmployeeModel GetEmployeeModel()
        {
            var name = "Employee Name";
            var hireDate = DateOnly.FromDateTime(DateTime.Now);
            var registrationNumber = "123456789";
            var jobPositionId = Guid.NewGuid();
            var pis = ValidPis;
            var companyId = Guid.NewGuid();
            var managerId = Guid.Empty;
            var workingDayId = Guid.NewGuid();
            var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
            return model;
        }

        internal static Employee GetEmployee()
        {
            var name = "Employee Name";
            var hireDate = DateOnly.FromDateTime(DateTime.Now);
            var registrationNumber = "123456789";
            var jobPositionId = Guid.NewGuid();
            var pis = new Pis(ValidPis);
            var companyId = Guid.NewGuid();
            var managerId = Guid.Empty;
            var workingDayId = Guid.NewGuid();
            var employee = new Employee(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
            return employee;
        }
        internal static string ValidCnpj => "23584296000130";

        internal static string ValidPis => "33080514967";

        internal static WorkingDay GetWorkingDay()
        {
            return new WorkingDay(
                "Working Day", 
                WorkingDayType.TEN_HOURS, 
                new TimeOnly(8, 0), 
                new TimeOnly(12, 0), 
                new TimeOnly(13, 0),
                new TimeOnly(18, 0),
                20);
        }

        internal static List<TimeClock> GetTimeClocks()
        {
            return new List<TimeClock>
            {
                new (Guid.NewGuid(), RegisterType.START_WORKING_DAY),
                new (Guid.NewGuid(), RegisterType.START_BREAK),
                new (Guid.NewGuid(), RegisterType.END_BREAK),
                new (Guid.NewGuid(), RegisterType.END_WORKING_DAY)
            };
        }

        internal static TimeClock GetTimeClock()
        {
            return new TimeClock(Guid.NewGuid(), RegisterType.START_WORKING_DAY);
        }
    }
}
