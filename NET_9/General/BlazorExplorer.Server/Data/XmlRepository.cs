using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

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
            var stream = assembly.GetManifestResourceStream("BlazorExplorer.Data.employees.xml");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(List<Employee>));
                _employeeList = (List<Employee>)serializer.Deserialize(reader);
            }

            var dstream = assembly.GetManifestResourceStream("BlazorExplorer.Data.departments.xml");

            using (var reader = new System.IO.StreamReader(dstream))
            {
                var serializer = new XmlSerializer(typeof(List<Department>));
                _departmentList = (List<Department>)serializer.Deserialize(reader);
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
