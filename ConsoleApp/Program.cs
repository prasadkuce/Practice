try
{
    Employee[] employees = new Employee[]
    {
        new Employee{ EmployeeId = 1,  Name = "Emp1", DepartmentId = 1},
        new Employee{ EmployeeId = 2,  Name = "Emp2", DepartmentId = 2},
        new Employee{ EmployeeId = 3,  Name = "Emp3", DepartmentId = 3},
        new Employee{ EmployeeId = 4,  Name = "Emp4"},
        new Employee{ EmployeeId = 5,  Name = "Emp5" }
    };
    Department[] departments = new Department[]
    {
        new Department{ DepartmentId = 1, Name = "Dept1" },
        new Department{ DepartmentId = 2, Name = "Dept2" },
        new Department{ DepartmentId = 3, Name = "Dept3" }
    };
    foreach(var employee in employees)
        Console.WriteLine($"Id: {employee.EmployeeId}, Name: {employee.Name}, DepartmentId: {employee.DepartmentId}");
    foreach (var department in departments)
        Console.WriteLine($"Id: {department.DepartmentId}, Name: {department.Name}");

    var innerJoin = from e in employees
                    join d in departments on e.DepartmentId equals d.DepartmentId
                    select new { e.EmployeeId, e.Name, e.DepartmentId, DeptName = d.Name };
    foreach (var item in innerJoin)
        Console.WriteLine($"EmployeeId: {item.EmployeeId}, Name: {item.Name}, DepartmentId: {item.DepartmentId}, DepartmentName: {item.DeptName}");
    var leftJoin = from e in employees
                   join d in departments on e.DepartmentId equals d.DepartmentId into joinEmpDept
                   from department in joinEmpDept.DefaultIfEmpty()
                   select new { e.EmployeeId, e.Name, e.DepartmentId, DeptName = department?.Name };
    foreach (var item in leftJoin)
        Console.WriteLine($"EmployeeId: {item.EmployeeId}, Name: {item.Name}, DepartmentId: {item.DepartmentId}, DepartmentName: {item.DeptName}");
    //Console.WriteLine("Enter a Number");
    //string? number = Console.ReadLine();
    //number = NumberToWords.ConvertAmount(double.Parse(number));
    //Console.WriteLine($"Number in words is {number}");
    //var lst = new List<int>() { 1,2,3,4,5,6,7,8,9,10};
    //lst = lst.OrderBy(x => new Random().Next()).ToList();
    //foreach(var item in lst)    
    //    Console.WriteLine(item);
    Console.ReadKey();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
public class NumberToWords
{
    private static string[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };
    private static string[] tens = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

    public static string ConvertAmount(double amount)
    {
        try
        {
            long amountLong = (long)amount;
            long amountDecimal = (long) Math.Round((amount - (double)(amount)) * 100);
            if (amountDecimal == 0)
                return Convert(amountLong) + " Only";
            else
                return Convert(amountLong) + " Point " + Convert(amountDecimal) + " Only.";
        }
        catch (Exception e)
        {
            throw;
        }
    }
    private static string Convert(long amount)
    {
        if (amount < 20)
            return units[amount];
        if (amount < 100)
            return tens[amount / 10] + ((amount % 10 > 0) ? Convert(amount % 10) : " ");
        if(amount < 1000)
            return units[amount / 100] + " Hundred " + ((amount % 100 > 0) ? Convert(amount % 100) : " ");
        if(amount < 100000)
            return units[amount / 1000] + " Thousand " + ((amount % 1000 > 0) ? Convert(amount % 1000) : " ");
        if (amount < 10000000)
            return units[amount / 100000] + " Lakh " + ((amount % 100000 > 0) ? Convert(amount % 100000) : " ");
        if (amount < 1000000000)
            return units[amount / 10000000] + " Crore " + ((amount % 10000000 > 0) ? Convert(amount % 10000000) : " ");
        return units[amount / 1000000000] + " Arab"  + ((amount % 1000000000 > 0) ? Convert(amount % 1000000000) : " ");
    }

}
public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
}
public class Department
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }
}