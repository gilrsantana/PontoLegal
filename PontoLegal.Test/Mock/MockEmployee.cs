namespace PontoLegal.Test.Mock;

public static class MockEmployee
{
    public static EmployeeModel GetEmployeeModel()
    {
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = MockPis.ValidPis;
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var model = new EmployeeModel(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        return model;
    }
    
    public static Employee GetEmployee()
    {
        var name = "Employee Name";
        var hireDate = DateOnly.FromDateTime(DateTime.Now);
        var registrationNumber = "123456789";
        var jobPositionId = Guid.NewGuid();
        var pis = new Pis(MockPis.ValidPis);
        var companyId = Guid.NewGuid();
        var managerId = Guid.Empty;
        var workingDayId = Guid.NewGuid();
        var employee = new Employee(name, hireDate, registrationNumber, jobPositionId, pis, companyId, managerId, workingDayId);
        return employee;
    }
}