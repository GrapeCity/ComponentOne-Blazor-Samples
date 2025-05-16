using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace BlazorExplorer
{
    public class XmlRepository
    {
        private static List<Employee> _employeeList = new List<Employee>();
        private static List<Department> _departmentList = new List<Department>();

        public XmlRepository()
        {
            var assembly = typeof(XmlRepository).GetTypeInfo().Assembly;

            //TODO: add culture
            using (var stream = assembly.GetManifestResourceStream("BlazorExplorer.Data.employees.xml"))
            {
                var root = XElement.Load(stream);
                _employeeList = (from employee in root.Elements("Employee")
                                 select new Employee
                                 {
                                     Name = (string)employee.Element("Name"),
                                     Title = (string)employee.Element("Title"),
                                     DepartmentId = (int)employee.Element("DepartmentId"),
                                     HireDate = (DateTime)employee.Element("HireDate"),
                                     Status = (double)employee.Element("Status"),
                                     FullTime = (bool)employee.Element("FullTime"),
                                     ThumbnailImage = (string)employee.Element("ThumbnailImage")
                                 }).ToList();
            }

            using (var stream = assembly.GetManifestResourceStream("BlazorExplorer.Data.departments.xml"))
            {
                var root = XElement.Load(stream);
                _departmentList = (from department in root.Elements("Department")
                                 select new Department
                                 {
                                     Id = (int)department.Element("Id"),
                                     Name = (string)department.Element("Name"),
                                 }).ToList();
            }
        }

        public List<Employee> GetEmployees()
        {
            return _employeeList;
        }

        public List<Department> GetDepartments()
        {
            return _departmentList;
        }

        public Department GetDepartment(int index)
        {
            int realIndex = index - 1;
            if (_departmentList.Count > 0 && _departmentList.Count > realIndex)
                return _departmentList[realIndex];
            else
                return null;
        }
    }

    public class Employee
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime HireDate { get; set; }
        public double Status { get; set; }
        public string ThumbnailImage { get; set; }
        public string ThumbnailUrl
        {
            get
            {
                return "Employees/" + ThumbnailImage;
            }
        }
        public int DepartmentId { get; set; }
        public bool FullTime { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
