﻿using Capstone.Exceptions;
using Capstone.Models;
using Capstone.Security.Models;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Collections.Generic;

namespace Capstone.DAO
{
    public class ImageSqlDao : IImageDao
    {
        private readonly string connectionString;

        public ImageSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Image UploadImage(Image image)
        {
            string sql = "INSERT INTO images (image_string, animal_id) " +
                         "OUTPUT INSERTED.image_id " +
                         "VALUES (@image_string, @animal_id)";

            Image createdImage;
            int imageId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@image_string", image.ImageString);
                    cmd.Parameters.AddWithValue("@animal_id", image.AnimalId);

                    imageId = Convert.ToInt32(cmd.ExecuteScalar());

                }

                createdImage = GetImageById(imageId);
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return createdImage;
        }

        public Image GetImageById(int imageId)
        {
            Image image = null;

            string sql = "SELECT image_id, image_string, animal_id FROM images WHERE image_id = @image_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@image_id", imageId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        image = MapRowToImage(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return image;
        }

        public List<Image> GetImagesByAnimalId(int id)
        {
            List<Image> images = new List<Image>();

            string sql = "SELECT image_id, image_string, animal_id FROM images WHERE animal_id = @animal_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@animal_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        images.Add(MapRowToImage(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new DaoException("SQL exception occurred", ex);
            }

            return images;
        }

        private Image MapRowToImage(SqlDataReader reader)
        {
            return new Image()
            {
                ImageId = Convert.ToInt32(reader["image_id"]),
                ImageString = Convert.ToString(reader["image_string"]),
                AnimalId = Convert.ToInt32(reader["animal_id"]),
            };
        }
    }
}