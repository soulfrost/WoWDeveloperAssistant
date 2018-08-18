﻿using System;
using System.Data;
using System.Windows.Forms;

namespace WoWDeveloperAssistant
{
    public partial class MainForm : Form
    {
        private DataSet tablesDataSet       = new DataSet();
        private DataTable combatDataTable   = new DataTable();
        private DataTable spellsDataTable   = new DataTable();
        private DataTable guidsDataTable    = new DataTable();

        private string creatureEntry        = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void createSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreatureSpellsCreator.FillSQLOutput(guidsDataTable, dataGridView_Spells, textBox_SQLOutput, listBox_CreatureGuids.SelectedItem.ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_Spells.SelectedRows)
            {
                dataGridView_Spells.Rows.Remove(row);
            }
        }

        private void toolStripButton_ImportSniff_Click(object sender, EventArgs e)
        {
            OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImportStarted();

                tablesDataSet = CreatureSpellsCreator.LoadSniffFile(openFileDialog.FileName);
                if (tablesDataSet != null)
                {
                    ImportSuccessful();
                    tablesDataSet.Tables.Clear();
                }
                else
                {
                    toolStripStatusLabel_FileStatus.Text = "No File Loaded";
                    toolStripButton_ImportSniff.Enabled = true;
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                return;
            }
        }

        private void toolStripButton_Search_Click(object sender, EventArgs e)
        {
            CreatureSpellsCreator.FillListBoxWithGuids(combatDataTable, guidsDataTable, listBox_CreatureGuids, toolStripTextBox_CreatureEntry.Text, checkBox_OnlyCombatSpells.Checked);
        }

        private void listBox_CreatureGuids_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreatureSpellsCreator.FillSpellsGrid(guidsDataTable, combatDataTable, spellsDataTable, listBox_CreatureGuids, dataGridView_Spells, listBox_CreatureGuids.SelectedItem.ToString(), checkBox_OnlyCombatSpells.Checked);
        }

        private void ImportStarted()
        {
            this.Cursor = Cursors.WaitCursor;
            combatDataTable = null;
            spellsDataTable = null;
            guidsDataTable = null;
            toolStripButton_ImportSniff.Enabled = false;
            toolStripButton_Search.Enabled = false;
            toolStripTextBox_CreatureEntry.Enabled = false;
            listBox_CreatureGuids.Enabled = false;
            listBox_CreatureGuids.DataSource = null;
            dataGridView_Spells.Enabled = false;
            dataGridView_Spells.Rows.Clear();
            toolStripStatusLabel_FileStatus.Text = "Loading File...";
        }

        private void ImportSuccessful()
        {
            combatDataTable = tablesDataSet.Tables[0];
            spellsDataTable = tablesDataSet.Tables[1];
            guidsDataTable = spellsDataTable.DefaultView.ToTable(true, "CreatureGuid", "CreatureEntry");
            toolStripButton_ImportSniff.Enabled = true;
            toolStripButton_Search.Enabled = true;
            toolStripTextBox_CreatureEntry.Enabled = true;
            toolStripStatusLabel_FileStatus.Text = openFileDialog.FileName + " is selected for input.";
            this.Cursor = Cursors.Default;
        }

        private void OpenFileDialog()
        {
            openFileDialog.Title = "Open File";
            openFileDialog.Filter = "Parsed Sniff File (*.txt)|*.txt";
            openFileDialog.FileName = "*.txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.ShowReadOnly = false;
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
        }
    }
}