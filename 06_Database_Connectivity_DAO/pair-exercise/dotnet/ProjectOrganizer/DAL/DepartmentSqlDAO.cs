using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class DepartmentSqlDAO : IDepartmentDAO
    {
        private string connectionString;
        private string sqlGetAllDepartments = "SELECT * FROM department";
        private string sqlAddNewDepartment = "INSERT INTO department VALUES (@name)";
        private string sqlUpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @departmentid";

        // Single Parameter Constructor
        public DepartmentSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            IList<Department> projectList = new List<Department>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlGetAllDepartments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Department temp = ConvertReaderToDepartment(reader);
                        projectList.Add(temp);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return projectList;
        }

        private Department ConvertReaderToDepartment(SqlDataReader reader)
        {
            Department temp = new Department();

            temp.Id = Convert.ToInt32(reader["department_id"]);
            temp.Name = Convert.ToString(reader["name"]);

            return temp;
        }


        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            int departmentId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlAddNewDepartment, conn);

                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);

                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("SELECT MAX(department_id) FROM department", conn);

                    departmentId = Convert.ToInt32(cmd2.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return departmentId;
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool wasDepartmentUpdated = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlUpdateDepartment, conn);

                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);
                    cmd.Parameters.AddWithValue("@departmentid", updatedDepartment.Id);

                    cmd.ExecuteNonQuery();
                    wasDepartmentUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return wasDepartmentUpdated;
        }

    }
}
