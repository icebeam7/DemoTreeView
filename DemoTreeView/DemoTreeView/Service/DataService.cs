using System.Linq;
using System.Collections.Generic;

using DemoTreeView.Model;

namespace DemoTreeView.Service
{
    public static class DataService
    {
        public static XamlItemGroup GroupData()
        {
            var company = GetCompany();
            var departments = GetDepartments().OrderBy(x => x.ParentDepartmentId);
            var employees = GetEmployees();

            var companyGroup = new XamlItemGroup();
            companyGroup.Name = company.CompanyName;

            foreach (var dept in departments)
            {
                var itemGroup = new XamlItemGroup();
                itemGroup.Name = dept.DepartmentName;
                itemGroup.GroupId = dept.DepartmentId;

                // Employees first
                var employeesDepartment = employees.Where(x => x.DepartmentId == dept.DepartmentId);

                foreach (var emp in employeesDepartment)
                {
                    var item = new XamlItem();
                    item.ItemId = emp.EmployeeId;
                    item.Key = emp.EmployeeName;

                    itemGroup.XamlItems.Add(item);
                }

                // Departments now
                if (dept.ParentDepartmentId == -1)
                {
                    companyGroup.Children.Add(itemGroup);
                }
                else
                {
                    XamlItemGroup parentGroup = null;

                    foreach (var group in companyGroup.Children)
                    {
                        parentGroup = FindParentDepartment(group, dept);

                        if (parentGroup != null)
                        {
                            parentGroup.Children.Add(itemGroup);
                            break;
                        }
                    }
                }
            }

            return companyGroup;
        }

        private static XamlItemGroup FindParentDepartment(XamlItemGroup group, Department department) 
        {
            if (group.GroupId == department.ParentDepartmentId)
                return group;

            if (group.Children != null)
            {
                foreach (var currentGroup in group.Children)
                {
                    var search = FindParentDepartment(currentGroup, department);

                    if (search != null)
                        return search;
                }
            }
            return null;
        }

        private static Company GetCompany()
        {
            return new Company()
            {
                CompanyId = 1,
                CompanyName = "TC Solutions"
            };
        }

        private static List<Department> GetDepartments()
        {
            return new List<Department>()
            {
                new Department() { DepartmentId = 1, DepartmentName = "IT", ParentDepartmentId = -1 },
                new Department() { DepartmentId = 2, DepartmentName = "Accounting", ParentDepartmentId = -1 },
                new Department() { DepartmentId = 3, DepartmentName = "Production", ParentDepartmentId = -1 },
                new Department() { DepartmentId = 4, DepartmentName = "Software", ParentDepartmentId = 1 },
                new Department() { DepartmentId = 5, DepartmentName = "Support", ParentDepartmentId = 1 },
                new Department() { DepartmentId = 6, DepartmentName = "Testing", ParentDepartmentId = 4 },
                new Department() { DepartmentId = 7, DepartmentName = "Accounts receivable", ParentDepartmentId = 2 },
                new Department() { DepartmentId = 8, DepartmentName = "Accounts payable", ParentDepartmentId = 2 },
                new Department() { DepartmentId = 9, DepartmentName = "Customers and services", ParentDepartmentId = 8 }
            };
        }

        private static List<Employee> GetEmployees()
        {
            return new List<Employee>()
            {
                new Employee() { EmployeeId = 1, EmployeeName = "Luis", DepartmentId = 1 },
                new Employee() { EmployeeId = 2, EmployeeName = "Pepe", DepartmentId = 1 },
                new Employee() { EmployeeId = 3, EmployeeName = "Juan", DepartmentId = 2 },
                new Employee() { EmployeeId = 4, EmployeeName = "Inés", DepartmentId = 3 },
                new Employee() { EmployeeId = 5, EmployeeName = "Sara", DepartmentId = 3 },
                new Employee() { EmployeeId = 6, EmployeeName = "Sofy", DepartmentId = 4 },
                new Employee() { EmployeeId = 7, EmployeeName = "Hugo", DepartmentId = 5 },
                new Employee() { EmployeeId = 8, EmployeeName = "Gema", DepartmentId = 5 },
                new Employee() { EmployeeId = 9, EmployeeName = "Olga", DepartmentId = 6 },
                new Employee() { EmployeeId = 1, EmployeeName = "Otto", DepartmentId = 6 },
                new Employee() { EmployeeId = 2, EmployeeName = "Axel", DepartmentId = 6 },
                new Employee() { EmployeeId = 3, EmployeeName = "Eloy", DepartmentId = 7 },
                new Employee() { EmployeeId = 4, EmployeeName = "Flor", DepartmentId = 8 },
                new Employee() { EmployeeId = 5, EmployeeName = "Aída", DepartmentId = 9 },
                new Employee() { EmployeeId = 6, EmployeeName = "Ruth", DepartmentId = 9 }
            };
        }
    }
}