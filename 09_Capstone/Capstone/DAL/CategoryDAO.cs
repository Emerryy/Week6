﻿using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    class CategoryDAO
    {
        private string connectionString;

        private string sqlGetVenueInfoById = "  SELECT v.name, v.description, c.name, s.name, ca.name FROM category_venue cv "
                                    + "JOIN category ca ON ca.id = cv.category_id"
                                    + "JOIN venue v ON v.id = cv.venue_id"
                                    + "JOIN city c ON c.id = v.city_id"
                                    + "JOIN state s ON s.abbreviation = c.state_abbreviation"
                                    + "WHERE v.id = @id;";

        public CategoryDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private Category ConvertReaderToReservation(SqlDataReader reader)
        {
            Category category = new Category();

            category.CategoryId = Convert.ToInt32(reader["id"]);
            category.Name = Convert.ToString(reader["name"]);

            return category;
        }
    }
}