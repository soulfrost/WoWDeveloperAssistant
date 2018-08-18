﻿using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WoWDeveloperAssistant
{
    public static class SQLModule
    {
        public static object DatabaseSelectQuery(string query)
        {
            DataSet dataSet = new DataSet();
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = "server = " + Properties.Settings.Default.Host + "; port = " + Properties.Settings.Default.Port + "; user id = " + Properties.Settings.Default.Username + "; password = " + Properties.Settings.Default.Password + "; database = " + Properties.Settings.Default.Database;
            try
            {
                sqlConnection.Open();
                MySqlCommand myCommand = new MySqlCommand(query, sqlConnection);
                MySqlDataAdapter DataAdapter = new MySqlDataAdapter();
                DataAdapter.SelectCommand = myCommand;
                DataAdapter.Fill(dataSet, "table1");
                return dataSet;
            }
            catch (MySqlException myerror)
            {
                MessageBox.Show("Error Connecting to Database: " + myerror.Message, "Database Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return null;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        public static bool TryConnectToDB()
        {
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = "server = " + Properties.Settings.Default.Host + "; port = " + Properties.Settings.Default.Port + "; user id = " + Properties.Settings.Default.Username + "; password = " + Properties.Settings.Default.Password + "; database = " + Properties.Settings.Default.Database;
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (MySqlException myerror)
            {
                MessageBox.Show("Error Connecting to Database please re-enter login information." + Environment.NewLine + myerror.Message);
                return false;
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}