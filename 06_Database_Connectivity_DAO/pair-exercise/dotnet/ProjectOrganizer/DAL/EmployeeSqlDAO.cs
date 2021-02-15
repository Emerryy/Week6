using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class EmployeeSqlDAO : IEmployeeDAO
    {
        private string connectionString;

        private string sqlGetAllEmployees = "SELECT * FROM employee";

        private string sqlSearchEmployees = "SELECT * FROM employee WHERE first_name = @firstname AND last_name = @lastname";

        private string sqlJoinEmployeeProjectTables = "SELECT em.employee_id, em.department_id, em.first_name, em.last_name, em.job_title, em.birth_date, em.gender, em.hire_date FROM project_employee pe " +
            "RIGHT JOIN employee em ON pe.employee_id = em.employee_id " +
            "WHERE pe.project_id IS NULL";

        /*
           SELECT em.employee_id, em.first_name, em.last_name FROM project_employee pe
            RIGHT JOIN employee em ON pe.employee_id = em.employee_id
            WHERE pe.project_id IS NULL
         */

        // Single Parameter Constructor
        public EmployeeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            IList<Employee> employeeList = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlGetAllEmployees, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee temp = ConvertReaderToEmployee(reader);
                        employeeList.Add(temp);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return employeeList;
        }

        private Employee ConvertReaderToEmployee(SqlDataReader reader)
        {
            Employee temp = new Employee();

            temp.EmployeeId = Convert.ToInt32(reader["employee_id"]);
            temp.DepartmentId = Convert.ToInt32(reader["department_id"]);
            temp.FirstName = Convert.ToString(reader["first_name"]);
            temp.LastName = Convert.ToString(reader["last_name"]);
            temp.JobTitle = Convert.ToString(reader["job_title"]);
            temp.BirthDate = Convert.ToDateTime(reader["birth_date"]);
            temp.Gender = Convert.ToString(reader["gender"]);
            temp.HireDate = Convert.ToDateTime(reader["hire_date"]);

            return temp;
        }

        /// <summary>
        /// Find all employees whose names contain the search strings.
        /// Returned employees names must contain *both* first and last names.
        /// </summary>
        /// <remarks>Be sure to use LIKE for proper search matching.</remarks>
        /// <param name="firstname">The string to search for in the first_name field</param>
        /// <param name="lastname">The string to search for in the last_name field</param>
        /// <returns>A list of employees that matches the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            IList<Employee> employeeList = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlSearchEmployees, conn);

                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee temp = ConvertReaderToEmployee(reader);
                        employeeList.Add(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return employeeList;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            IList<Employee> employeeWithoutProjects = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlJoinEmployeeProjectTables, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee temp = ConvertReaderToEmployee(reader);
                        employeeWithoutProjects.Add(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return employeeWithoutProjects;
        }
    }
}
