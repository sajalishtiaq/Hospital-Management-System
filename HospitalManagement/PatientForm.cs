// PatientForm.cs
// Hospital Management System - Patient Management
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HospitalManagement
{
    public partial class PatientForm : Form
    {
        private int selectedPatientID = 0;

        public PatientForm()
        {
            InitializeComponent();
            LoadPatients();
        }

        private void LoadPatients(string searchTerm = "")
        {
            string query = @"SELECT PatientID, FullName, CNIC, Contact, Address, CreatedAt 
                             FROM Patients";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE FullName LIKE @Search OR CNIC LIKE @Search OR Contact LIKE @Search";
                SqlParameter[] p = { new SqlParameter("@Search", "%" + searchTerm + "%") };
                dgvPatients.DataSource = DBHelper.ExecuteQuery(query, p);
            }
            else
            {
                dgvPatients.DataSource = DBHelper.ExecuteQuery(query);
            }

            dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            // Check duplicate CNIC
            string checkQuery = "SELECT COUNT(*) FROM Patients WHERE CNIC = @CNIC";
            SqlParameter[] checkP = { new SqlParameter("@CNIC", txtCNIC.Text.Trim()) };
            int count = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (count > 0)
            {
                MessageBox.Show("A patient with this CNIC already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"INSERT INTO Patients (FullName, CNIC, Contact, Address) 
                             VALUES (@Name, @CNIC, @Contact, @Address)";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", txtName.Text.Trim()),
                new SqlParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqlParameter("@Contact", txtContact.Text.Trim()),
                new SqlParameter("@Address", txtAddress.Text.Trim())
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Patient added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadPatients();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == 0)
            {
                MessageBox.Show("Please select a patient to update.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            // Check duplicate CNIC for other patients
            string checkQuery = "SELECT COUNT(*) FROM Patients WHERE CNIC = @CNIC AND PatientID != @ID";
            SqlParameter[] checkP = {
                new SqlParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqlParameter("@ID", selectedPatientID)
            };
            int count = Convert.ToInt32(DBHelper.ExecuteScalar(checkQuery, checkP));
            if (count > 0)
            {
                MessageBox.Show("Another patient with this CNIC already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"UPDATE Patients SET FullName=@Name, CNIC=@CNIC, Contact=@Contact, 
                             Address=@Address WHERE PatientID=@ID";
            SqlParameter[] parameters = {
                new SqlParameter("@Name", txtName.Text.Trim()),
                new SqlParameter("@CNIC", txtCNIC.Text.Trim()),
                new SqlParameter("@Contact", txtContact.Text.Trim()),
                new SqlParameter("@Address", txtAddress.Text.Trim()),
                new SqlParameter("@ID", selectedPatientID)
            };

            if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
            {
                MessageBox.Show("Patient updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadPatients();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPatientID == 0)
            {
                MessageBox.Show("Please select a patient to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this patient?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string query = "DELETE FROM Patients WHERE PatientID = @ID";
                SqlParameter[] parameters = { new SqlParameter("@ID", selectedPatientID) };

                if (DBHelper.ExecuteNonQuery(query, parameters) > 0)
                {
                    MessageBox.Show("Patient deleted successfully.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadPatients();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPatients(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadPatients();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
                selectedPatientID = Convert.ToInt32(row.Cells["PatientID"].Value);
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtCNIC.Text = row.Cells["CNIC"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            { MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtCNIC.Text))
            { MessageBox.Show("CNIC is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            { MessageBox.Show("Contact is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            { MessageBox.Show("Address is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void ClearForm()
        {
            selectedPatientID = 0;
            txtName.Clear();
            txtCNIC.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            dgvPatients.ClearSelection();
        }
    }
}
