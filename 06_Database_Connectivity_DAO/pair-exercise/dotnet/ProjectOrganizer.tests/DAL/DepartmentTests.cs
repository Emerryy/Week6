using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ProjectOrganizer.DAL;
using ProjectOrganizer.Models;

namespace ProjectOrganizer.tests.DAL
{
    [TestClass]
    public class DepartmentTests
    {
        const string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=Company;Integrated Security=True";

        private TransactionScope tran;
        private int departmentId = 0;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO department VALUES('Test Department'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                departmentId = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetDepartmentsTest()
        {

            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);

            IList<Department> result = dao.GetDepartments();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

            int counter = 0;
            foreach (Department dept in result)
            {
                if (dept.Id == departmentId)
                {
                    counter++;
                }
            }

            Assert.AreEqual(1, counter);

        }
        [TestMethod()]
        public void CreateDepartmentTest()
        {
            int originalCount = GetCountOfDepartments();
            Department newDept = new Department
            {
                Name = "New Test Department"
            };
            DepartmentSqlDAO dao = new DepartmentSqlDAO(ConnectionString);

            int result = dao.CreateDepartment(newDept);

            Assert.AreEqual(departmentId + 1, result);
            Assert.AreEqual(originalCount + 1, GetCountOfDepartments());

        }

        [TestMethod()]
        public void UpdateDepartmentTest()
        {
            Department updatedDept = new Department
            {
                Id = departmentId,
                Name = "Updated Department"
            };
            DepartmentSqlDAO dal = new DepartmentSqlDAO(ConnectionString);

            bool result = dal.UpdateDepartment(updatedDept);

            Assert.IsTrue(result);
        }

        private int GetCountOfDepartments()
        {
            int counts = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                //Create a new test department
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from department", conn);
                counts = (int)cmd.ExecuteScalar();
            }

            return counts;
        }

    }
}
