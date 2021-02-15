using ProjectOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOrganizer.DAL
{
    public class ProjectSqlDAO : IProjectDAO
    {
        private string connectionString;

        private string sqlGetAllProjects = "SELECT * FROM project";

        private string sqlDeleteEmployeeFromProject = "DELETE FROM project_employee WHERE project_id = @projectid AND employee_id = @employeeid";

        private string sqlAddEmployeeToProject = "INSERT INTO project_employee VALUES (@projectid, @employeeid)";

        private string sqlAddNewProject = "INSERT INTO project VALUES (@name, @fromdate, @todate)";

        // Single Parameter Constructor
        public ProjectSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            IList<Project> projectList = new List<Project>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlGetAllProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project temp = ConvertReaderToProject(reader);
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

        private Project ConvertReaderToProject(SqlDataReader reader)
        {
            Project temp = new Project();

            temp.ProjectId = Convert.ToInt32(reader["project_id"]);
            temp.Name = Convert.ToString(reader["name"]);
            temp.StartDate = Convert.ToDateTime(reader["from_date"]);
            temp.EndDate = Convert.ToDateTime(reader["to_date"]);

            return temp;
        }

        /// <summary>
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool employeeAssigned = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlAddEmployeeToProject, conn);

                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);

                    cmd.ExecuteNonQuery();
                }
                employeeAssigned = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                employeeAssigned = false;
            }
            return employeeAssigned;
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool employeeRemoved = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlDeleteEmployeeFromProject, conn);

                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);

                    cmd.ExecuteNonQuery();
                }
                employeeRemoved = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                employeeRemoved = false;
            }
            return employeeRemoved;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            int projectId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlAddNewProject, conn);

                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@fromdate", newProject.StartDate);
                    cmd.Parameters.AddWithValue("@todate", newProject.EndDate);

                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("SELECT MAX(project_id) FROM project", conn);

                    projectId = Convert.ToInt32(cmd2.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return projectId;
        }

    }
}
