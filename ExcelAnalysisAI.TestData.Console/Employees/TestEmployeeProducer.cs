namespace ExcelAnalysisAI.TestData.Console.Employees;

internal class TestEmployeeProducer
{
    private static readonly string[] FirstNames =
    {
        "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Henry", "Ivy", "Jack",
        "Olivia", "Liam", "Sophia", "Noah", "Emma", "Mason", "Ava", "James", "Isabella", "Benjamin",
        "Mia", "Lucas", "Charlotte", "Ethan", "Amelia", "Michael", "Harper", "Alexander", "Evelyn", "William",
        "Abigail", "Daniel", "Emily", "Matthew", "Elizabeth", "Samuel", "Sofia", "David", "Victoria", "Joseph",
        "Grace", "Andrew", "Chloe", "Christopher", "Zoey", "Joshua", "Lily", "Ryan", "Hannah", "John"
    };

    private static readonly string[] LastNames =
    {
        "Johnson", "Smith", "Brown", "Prince", "Adams", "Wilson", "Liu", "Davis", "Chen", "Miller",
        "Martinez", "Thompson", "Garcia", "Rodriguez", "Wilson", "Lee", "Anderson", "Taylor", "Clark", "Walker",
        "Hall", "Allen", "Young", "King", "Scott", "Green", "Adams", "Baker", "Nelson", "Carter",
        "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins",
        "Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey"
    };

    private static readonly string[] Departments =
    {
        "Engineering", "Sales", "HR", "Marketing"
    };

    private static readonly string[] Regions =
    {
        "Russia", "US", "China", "Germany", "South Africa",
        "India", "Belarus", "Canada", "Turkey", "Malaysia"
    };

    private static readonly Random random = new Random();

    public List<Employee> Generate(int entryCount)
    {
        var employees = new List<Employee>();

        for (int i = 0; i < entryCount; i++)
        {
            var employee = new Employee
            {
                Id = i + 1,
                Name = GenerateRandomName(),
                Department = GenerateRandomDepartment(),
                Region = GenerateRandomRegion(),
                Salary = GenerateRandomSalary(),
                HireDate = GenerateRandomHireDate(),
                YearsExperience = GenerateRandomYearsExperience(),
                HasHigherEducation = GenerateRandomEducation()
            };

            employees.Add(employee);
        }

        return employees;
    }

    private string GenerateRandomName()
    {
        string firstName = FirstNames[random.Next(FirstNames.Length)];
        string lastName = LastNames[random.Next(LastNames.Length)];
        return $"{firstName} {lastName}";
    }

    private string GenerateRandomDepartment()
    {
        return Departments[random.Next(Departments.Length)];
    }

    private string GenerateRandomRegion()
    {
        return Regions[random.Next(Regions.Length)];
    }

    private decimal GenerateRandomSalary()
    {
        // Больший разброс: от 30,000 до 300,000
        return random.Next(30, 300) * 1000;
    }

    private DateTime GenerateRandomHireDate()
    {
        // Больший разброс: от 2000 года до текущего года
        DateTime startDate = new DateTime(2000, 1, 1);
        DateTime endDate = DateTime.Now;

        int range = (endDate - startDate).Days;
        return startDate.AddDays(random.Next(range));
    }

    private int GenerateRandomYearsExperience()
    {
        // Больший разброс: от 0 до 25 лет
        return random.Next(0, 26);
    }

    private bool GenerateRandomEducation()
    {
        return random.Next(2) == 1; // 50% шанс иметь высшее образование
    }
}