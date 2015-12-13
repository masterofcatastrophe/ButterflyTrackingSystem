using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Common;


namespace ButterflyTrackingSystem
{
    public partial class BTS : Form
    {
        static DBConnect con = new DBConnect();
        MySqlConnection dbcon = con.connection;
        int Emp_ID = -1; // currently logged in user id
        bool Cred; // check tagger/nonTagger
        bool cal = false; // enable disabled date
        bool tim = false; // enable disabled time
        
        public BTS()
        {
            InitializeComponent();
            con.OpenConnection(); // open db connection
            functionalitiesTabs.SelectedIndexChanged += tabControl1_SelectedIndexChanged; // condition for tagger/nonTagger
            //functionalitiesTabs.Selecting += ideaTabControl_Selecting;
           mainPanel.VisibleChanged += new EventHandler(listView_VisibleChanged); // conditions for nonTagger
        }

        void listView_VisibleChanged(object sender, EventArgs e)
        {
            //mainPanel.VisibleChanged -= new EventHandler(listView_VisibleChanged);
            if (functionalitiesTabs.SelectedIndex == 2) // clears search tab initially
            {
                searchDateTimePicker.Value = DateTime.Now;
                dateTimePicker1.Value = DateTime.Now;
                searchDateTimePicker.CustomFormat = " ";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = " ";
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.CustomFormat = " ";

            }
        }

        /*
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (!e.TabPage.Enabled)
            {
                e.Cancel = true;
            }
        }
        */

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check Credentials Here 

            if ((Cred == true) && (functionalitiesTabs.SelectedIndex == 0))
            {
                (functionalitiesTabs.TabPages[0] as TabPage).Enabled = true;
            }
            if ((Cred == true) && (functionalitiesTabs.SelectedIndex == 1))
            {
                (functionalitiesTabs.TabPages[1] as TabPage).Enabled = true;
            }
            else if ((Cred == false) && ((functionalitiesTabs.SelectedIndex == 0)
                                         || functionalitiesTabs.SelectedIndex == 1))
            {
               // (functionalitiesTabs.TabPages[0] as TabPage).Enabled = false; // disable controls
               // (functionalitiesTabs.TabPages[1] as TabPage).Enabled = false; // disable controls
                (functionalitiesTabs.TabPages[1] as TabPage).Visible = false; // hide controls
                (functionalitiesTabs.TabPages[0] as TabPage).Visible = false; // hide controls
                //MessageBox.Show("Unable to load tab. You are not a tagger.");
            }
            if (functionalitiesTabs.SelectedIndex == 2) // clears search tab initially
            {
                /*
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.ShowUpDown = true;
                searchDateTimePicker.CustomFormat = "MM/dd/yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.ShowUpDown = true;
                dateTimePicker1.CustomFormat = "hh:mm:ss tt";
                */
                searchDateTimePicker.Value = DateTime.Now;
                dateTimePicker1.Value = DateTime.Now;
                searchDateTimePicker.CustomFormat = " ";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = " ";
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.CustomFormat = " ";

            }
        }
      
        private void BTS_Load(object sender, EventArgs e)
        {

        }

        private void loginPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void loginLabel_Click(object sender, EventArgs e)
        {

        }

        private void userNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string LoginUserName = userNameBox.Text;
            string LoginPassword = passwordBox.Text;

            if (dbcon.State == ConnectionState.Open)
            {
                MySqlCommand user_validation =
                    new MySqlCommand(
                        "SELECT * FROM Employee WHERE User_ID='" + userNameBox.Text + "'AND Password='" +
                        passwordBox.Text + "';", dbcon);

                MySqlDataReader myReader;
                myReader = user_validation.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count = count + 1;
                }

                if (count == 1)
                {
                    myReader.Close();
                    try
                    {
                        // Save Employee ID into variable
                        MySqlDataReader reader;
                        string selectCmd = ("SELECT Employee_ID FROM Employee WHERE User_ID LIKE'" + userNameBox.Text +
                                            "';");

                        MySqlCommand Get_Emp_ID = new MySqlCommand(selectCmd, dbcon);
                        reader = Get_Emp_ID.ExecuteReader();

                        while (reader.Read())
                        {
                            Emp_ID = Convert.ToInt32(reader["Employee_ID"]);
                        }
                        reader.Close();

                        // Find if account is tagger nonTagger
                        string selectCred = ("SELECT Position FROM Employee WHERE User_ID LIKE'" + userNameBox.Text +
                                             "';");
                        MySqlCommand retreiveCred = new MySqlCommand(selectCred, dbcon);
                        MySqlDataReader CredReader;
                        CredReader = retreiveCred.ExecuteReader();
                        while (CredReader.Read())
                        {
                            if ((string) (CredReader["Position"]) == "tagger")
                            {
                                Cred = true;
                            }
                            else
                            {
                                Cred = false;
                            }
                        }
                        CredReader.Close();

                        /*
                        while (myReader.Read())
                        {
                            string sUser = myReader.GetString("User_ID");
                            string sPassword = myReader.GetString("Password");
                            string sName = myReader.GetString("Name");
                            string sPhone = myReader.GetString("Phone_Number");
                            string sStreet = myReader.GetString("Street_Address");
                            string sCity = myReader.GetString("City");
                            string sState = myReader.GetString("State");
                            string sPosition = myReader.GetString("Position");

                            //updateUserNameTextBox.Text = sUser;

                            updatePasswordTextBox.Text = sPassword;
                            updateEmployeeNameTextBox.Text = sName;
                            updatePhoneNumberTextBox.Text = sPhone;
                            updateEmployeeStreetTextBox.Text = sStreet;
                            updateEmployeeCityTextBox.Text = sCity;
                            updateEmployeeStateTextBox.Text = sState;
                            positionOptionsUpdateComboBox.Text = sPosition;
                        }
                        myReader.Close();
                        */

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    //MessageBox.Show(Emp_ID.ToString());

                    loginPanel.Visible = false;
                    registrationPanel.Visible = false;
                    if (Cred == false)
                    {
                        functionalitiesTabs.SelectedTab = functionalitiesTabs.TabPages[2];
                    }
                    mainPanel.Visible = true;

                }
                else
                {
                    myReader.Close();
                    MessageBox.Show("Invalid user name/password!");
                }



                /*else if (!String.IsNullOrEmpty(userNameBox.Text) && !String.IsNullOrEmpty(passwordBox.Text))
            {
                loginPanel.Visible = false; //To-Do: if credentials are correct, enter system. otherwise, show alert box invalid credentials!
                registrationPanel.Visible = false;
                mainPanel.Visible = true;
            }*/
                if (String.IsNullOrEmpty(userNameBox.Text))
                {
                    loginUserError.SetError(userNameBox, "User Name field is empty!");
                }
                else
                {
                    loginUserError.Clear();
                }
                if (String.IsNullOrEmpty(passwordBox.Text))
                {
                    loginPasswordError.SetError(passwordBox, "Password field is empty!");
                }
                else
                {
                    loginPasswordError.Clear();
                }
            }
            else
            {
                con.CloseConnection();
                con.OpenConnection();
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            loginPanel.Visible = false;
            registrationPanel.Visible = true;

            //resetting the fields when going to create account
            foreach (Control item in registrationPanel.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            } //end foreach
        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void passwordLabel_Click(object sender, EventArgs e)
        {

        }

        private void dontHaveanAccountLabel_Click(object sender, EventArgs e)
        {

        }

        private void registrationPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void createAccountLabel_Click(object sender, EventArgs e)
        {

        }

        private void createUserNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void createEmployeeUserNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createPasswordLabel_Click(object sender, EventArgs e)
        {

        }

        private void createEmployeePasswordBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void EmployeeNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void createEmployeeNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void employeePhoneNumberLabel_Click(object sender, EventArgs e)
        {

        }

        private void createEmployeePhoneNumberBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void employeePositionLabel_Click(object sender, EventArgs e)
        {

        }

        private void taggerNontaggerOptionsBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void employeeAddressLabel_Click(object sender, EventArgs e)
        {

        }

        private void employeeStreetLabel_Click(object sender, EventArgs e)
        {

        }

        private void employeeCityLabel_Click(object sender, EventArgs e)
        {

        }

        private void employeeStateLabel_Click(object sender, EventArgs e)
        {

        }

        private void createEmployeeStreetBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createEmployeeCityBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createEmployeeStateBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createEmployeeAccountButton_Click(object sender, EventArgs e)
        {
            string employeeUserName = createEmployeeUserNameBox.Text;
            string employeePassword = createEmployeePasswordBox.Text;
            string employeeName = createEmployeeNameBox.Text;
            string employeeStreet = createEmployeeStreetBox.Text;
            string employeeCity = createEmployeeCityBox.Text;
            string employeeState = createEmployeeStateBox.Text;
            string employeePosition = taggerNontaggerOptionsBox.Text;
            string employeePhoneNumber = createEmployeePhoneNumberBox.Text;

            /*
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "butterflytrackingsystem.coriiiuartnb.us-east-1.rds.amazonaws.com";
            builder.UserID = "root";
            builder.Password = "master77";
            builder.Database = "BTS";
            MySqlConnection connection = new MySqlConnection(builder.ToString());
            connection.Open();
            */

            if (dbcon.State == ConnectionState.Open)
            {
                // inserting values into Employee table
                string newuser_sql =
                    "INSERT INTO Employee (Name, Position, Phone_Number, City, State, Street_Address, User_ID, Password) VALUES (@EmployeeName, @EmployeePosition, @EmployeePhoneNumber, @EmployeeCity, @EmployeeState, @EmployeeStreetAddress, @EmployeeUserID, @EmployeePassword)";
                string newuser_validation = "SELECT USER_ID FROM Employee WHERE (User_ID=@EmployeeUserID)";

                MySqlCommand newuser_valid = new MySqlCommand(newuser_validation, dbcon);
                MySqlCommand newuser = new MySqlCommand(newuser_sql, dbcon);


                //Validating if employee id exists
                newuser_valid.Parameters.AddWithValue("@EmployeeUserID", employeeUserName);
                var nID = newuser_valid.ExecuteScalar();
                if (nID != null)
                {
                    MessageBox.Show("user exists!");
                }
                else if (!String.IsNullOrEmpty(createEmployeeUserNameBox.Text) &&
                         !String.IsNullOrEmpty(createEmployeePasswordBox.Text)
                         && !String.IsNullOrEmpty(createEmployeeNameBox.Text) &&
                         !String.IsNullOrEmpty(createEmployeeStreetBox.Text)
                         && !String.IsNullOrEmpty(createEmployeeCityBox.Text) &&
                         !String.IsNullOrEmpty(createEmployeeStateBox.Text)
                         && !String.IsNullOrEmpty(taggerNontaggerOptionsBox.Text))
                {
                    newuser.CommandText = newuser_sql;
                    newuser.Parameters.AddWithValue("@EmployeeName", employeeName);
                    newuser.Parameters.AddWithValue("@EmployeePosition", employeePosition);
                    newuser.Parameters.AddWithValue("@EmployeePhoneNumber", employeePhoneNumber);
                    newuser.Parameters.AddWithValue("@EmployeeCity", employeeCity);
                    newuser.Parameters.AddWithValue("@EmployeeState", employeeState);
                    newuser.Parameters.AddWithValue("@EmployeeStreetAddress", employeeStreet);
                    newuser.Parameters.AddWithValue("@EmployeeUserID", employeeUserName);
                    newuser.Parameters.AddWithValue("@EmployeePassword", employeePassword);
                    newuser.ExecuteNonQuery();
                    MessageBox.Show("Account created Successfully!");

                    loginPanel.Visible = true;
                    registrationPanel.Visible = false;

                    //resetting the fields when going to login after finishing creating account
                    foreach (Control item in loginPanel.Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = "";
                        }
                    } //end foreach
                }

                if (String.IsNullOrEmpty(createEmployeeUserNameBox.Text))
                {
                    registerUserNameError.SetError(createEmployeeUserNameBox, "User Name field is empty!");
                }
                else
                {
                    registerUserNameError.Clear();
                }
                if (String.IsNullOrEmpty(createEmployeePasswordBox.Text))
                {
                    registerPasswordError.SetError(createEmployeePasswordBox, "Password field is empty!");
                }
                else
                {
                    registerPasswordError.Clear();
                }
                if (String.IsNullOrEmpty(createEmployeeNameBox.Text))
                {
                    registerEmployeeNameError.SetError(createEmployeeNameBox, "Employee Name field is empty!");
                }
                else
                {
                    registerEmployeeNameError.Clear();
                }
                if (String.IsNullOrEmpty(createEmployeeStreetBox.Text))
                {
                    registerStreetError.SetError(createEmployeeStreetBox, "Street field is empty!");
                }
                else
                {
                    registerStreetError.Clear();
                }
                if (String.IsNullOrEmpty(createEmployeeCityBox.Text))
                {
                    registerCityError.SetError(createEmployeeCityBox, "City field is empty!");
                }
                else
                {
                    registerCityError.Clear();
                }
                if (String.IsNullOrEmpty(createEmployeeStateBox.Text))
                {
                    registerStateError.SetError(createEmployeeStateBox, "State field is empty!");
                }
                else
                {
                    registerStateError.Clear();
                }
                if (String.IsNullOrEmpty(taggerNontaggerOptionsBox.Text))
                {
                    registerSelectPositionError.SetError(taggerNontaggerOptionsBox, "No Postion selected!");
                }
                else
                {
                    registerSelectPositionError.Clear();
                }
            }
            else
            {
                con.CloseConnection();
                con.OpenConnection();
            }
        }

        private void alreadyHaveanAccountLabel_Click(object sender, EventArgs e)
        {

        }

        private void loginHereButton_Click(object sender, EventArgs e)
        {
            loginPanel.Visible = true;
            registrationPanel.Visible = false;

            //resetting the fields when going to login
            foreach (Control item in loginPanel.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            } //end foreach

        }

        private void employeeRequiredLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateAccountLabel_Click(object sender, EventArgs e)
        {

        }

        private void userNameUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateUserNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void employeeNameUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void selectPositionUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void passwordUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void phoneNumberUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void streetUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void addressUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void cityUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void stateUpdateLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateAccountButton_Click(object sender, EventArgs e)
        {
            //string UpdateEmployeeUserName = updateUserNameTextBox.Text;
            string UpdateEmployeePassword = updatePasswordTextBox.Text;
            string UpdateEmployeeName = updateEmployeeNameTextBox.Text;
            string UpdateEmployeeStreet = updateEmployeeStreetTextBox.Text;
            string UpdateEmployeeCity = updateEmployeeCityTextBox.Text;
            string UpdateEmployeeState = updateEmployeeStateTextBox.Text;
            string UpdateEmployeePosition = positionOptionsUpdateComboBox.Text;
            string UpdateEmployeePhoneNumber = updatePhoneNumberTextBox.Text;

            //To-DO: update query here
            if (dbcon.State == ConnectionState.Open)
            {
                if (!String.IsNullOrEmpty(updatePasswordTextBox.Text)
                    && !String.IsNullOrEmpty(updateEmployeeNameTextBox.Text) &&
                    !String.IsNullOrEmpty(updateEmployeeStreetTextBox.Text)
                    && !String.IsNullOrEmpty(updateEmployeeCityTextBox.Text) &&
                    !String.IsNullOrEmpty(updateEmployeeStateTextBox.Text)
                    && !String.IsNullOrEmpty(updatePhoneNumberTextBox.Text))
                {
                    string updateuser_sql = "UPDATE Employee SET Name='" + updateEmployeeNameTextBox.Text +
                                            "', Position='" + positionOptionsUpdateComboBox.Text + "', Phone_Number='" +
                                            updatePhoneNumberTextBox.Text + "', City='" + updateEmployeeCityTextBox.Text +
                                            "', State='" + updateEmployeeStateTextBox.Text + "', Street_Address='" +
                                            updateEmployeeStreetTextBox.Text + "', Password='" +
                                            updatePasswordTextBox.Text + "'WHERE User_ID='" + userNameBox.Text + "' ;";
                    MySqlCommand updateuser = new MySqlCommand(updateuser_sql, dbcon);
                    MySqlDataReader updateReader = updateuser.ExecuteReader();
                    MessageBox.Show("Account up to date!");
                    updateReader.Close();
                }
            }
            else if (String.IsNullOrEmpty(updatePasswordTextBox.Text))
            {
                MessageBox.Show("Password Field is empty !");
            }
            else if (String.IsNullOrEmpty(updateEmployeeNameTextBox.Text))
            {
                MessageBox.Show("Employee Name is empty !");
            }
        }

        private void updatePasswordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEmployeeNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updatePhoneNumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEmployeeStreetTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void positionOptionsUpdateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void updateAccountTab_Click(object sender, EventArgs e)
        {

        }

        private void searchTab_Click(object sender, EventArgs e)
        {
            /*
            if (cal == false)
            {
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.ShowUpDown = true;
                searchDateTimePicker.CustomFormat = "MM/dd/yyyy";
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.ShowUpDown = true;
                dateTimePicker1.CustomFormat = "hh:mm:ss tt";
                cal = true;
            }
            else
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = " ";
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.CustomFormat = " ";
                cal = false;
            }
            */
            
        }

        private void searchLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchTagIDLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchUserNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchSpeciesLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchGenderLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchLocationLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchCityLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchStateLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchCountryLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchNoteLabel_Click(object sender, EventArgs e)
        {

        }

        private void searchTagIDTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchUserNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchSpeciesTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchGendercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void searchCityTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchStateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchCountryTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchtimepicker = searchDateTimePicker.Text;
            string datetimepicker = dateTimePicker1.Text;
            string searchgender = searchGendercomboBox.Text;
            string searchcountry = searchCountryTextBox.Text;
            string searchstate = searchStateTextBox.Text;
            string searchcity = searchCityTextBox.Text;
            string searchspecies = searchSpeciesTextBox.Text;
            string searchtagid = searchTagIDTextBox.Text;
            string searchusername = searchUserNameTextBox.Text;

            if (!String.IsNullOrEmpty(searchUserNameTextBox.Text) || !String.IsNullOrEmpty(searchTagIDTextBox.Text) ||
                !String.IsNullOrEmpty(searchSpeciesTextBox.Text) || !String.IsNullOrEmpty(searchCityTextBox.Text) ||
                !String.IsNullOrEmpty(searchStateTextBox.Text) || !String.IsNullOrEmpty(searchCountryTextBox.Text)
                || !String.IsNullOrEmpty(searchGendercomboBox.Text) || cal == true || tim == true)
            {

                if (dbcon.State == ConnectionState.Open)
                {
                    string SearchDate = searchDateTimePicker.Value.ToString("MM-dd-yyyy"); // user defined date
                    string SearchTime = dateTimePicker1.Value.ToString("hh:mm tt"); // user defined date
                    
                    MySqlDataAdapter mySqlDataAdapter;
                    MySqlCommandBuilder mySqlCommandBuilder;
                    DataTable dataTable;
                    BindingSource bindingSource;

                    StringBuilder searchquery = new StringBuilder("SELECT User_ID AS UserID, Sight_ID AS TagID, BTS.Sighting_Locations.Employee_ID AS EmpID, Longitude AS Lon, Latitude AS Lat, BTS.Sighting_Locations.City," +
                        " BTS.Sighting_Locations.State AS ST, Country," +
                        " Date_of_sighting AS sDate, Time_of_sighting AS sTime, Species, Age, Gender" +
                        " FROM BTS.Sighting_Locations INNER JOIN BTS.Butterfly ON BTS.Sighting_Locations.Sight_ID = BTS.Butterfly.Tag_ID" +
                        " RIGHT JOIN BTS.Employee ON BTS.Employee.Employee_ID = BTS.Sighting_Locations.Employee_ID WHERE 1=1");
                    if (!String.IsNullOrEmpty(searchUserNameTextBox.Text))
                        searchquery.Append(" AND User_ID = '" + searchUserNameTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchTagIDTextBox.Text))
                        searchquery.Append(" AND Sight_ID = '" + searchTagIDTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchSpeciesTextBox.Text))
                        searchquery.Append(" AND Species = '" + searchSpeciesTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchCityTextBox.Text))
                        searchquery.Append(" AND BTS.Sighting_Locations.City = '" + searchCityTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchStateTextBox.Text))
                        searchquery.Append(" AND BTS.Sighting_Locations.State = '" + searchStateTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchCountryTextBox.Text))
                        searchquery.Append(" AND Country = '" + searchCountryTextBox.Text + "'");
                    if (!String.IsNullOrEmpty(searchGendercomboBox.Text))
                    {
                        string gend;
                        if (searchgender == "Male")
                        {
                            gend = "M";
                        }
                        else
                        {
                            gend = "F";
                        }
                        searchquery.Append(" AND Gender = '" + gend + "'");
                    }
                    if (!String.IsNullOrEmpty(searchDateTimePicker.Text) && cal == true)
                    {
                        searchquery.Append(" AND Date_of_sighting = '" + SearchDate + "'");
                    }
                    if (!String.IsNullOrEmpty(dateTimePicker1.Text) && tim == true)
                    {
                        searchquery.Append(" AND Time_of_sighting = '" + SearchTime + "'");
                    }

                    mySqlDataAdapter = new MySqlDataAdapter(searchquery.ToString(), dbcon);
                    mySqlCommandBuilder = new MySqlCommandBuilder(mySqlDataAdapter);
                    dataTable = new DataTable();
                    mySqlDataAdapter.Fill(dataTable);

                    bindingSource = new BindingSource();
                    bindingSource.DataSource = dataTable;

                    searchDataGrid.ReadOnly = true;
                    searchDataGrid.DataSource = bindingSource;
                    searchDataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                    searchDataGrid.AllowUserToAddRows = false;
                    searchDataGrid.RowHeadersVisible = false;
                    searchDataGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    //resetting the fields when going to search
                    foreach (Control item in searchTab.Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = "";
                        }
                        if (item is ComboBox)
                        {
                            (item as ComboBox).SelectedIndex = -1;
                            (item as ComboBox).Text = "";
                        }
                    } //end foreach
                }
                else
                {
                    con.CloseConnection();
                    con.OpenConnection();
                }
            }
            else
            {
                /*
                DataGridView1.DataSource = DataSet.Tables["TableName"];
                DataGridView.DataSource = DataTable;
                DataGridView.DataSource = null;
                */
                if (searchDataGrid.DataSource != null)
                {
                    searchDataGrid.DataSource = null;
                    searchDataGrid.Refresh();
                }
                else
                {
                    searchDataGrid.Rows.Clear();
                }
                //MessageBox.Show("Have to use at least one field.", "Error", MessageBoxButtons.OK);
            }
        }

        private void uploadSightingsFileButton_Click(object sender, EventArgs e)
        {

        }

        private void downloadSightingsFileButton_Click(object sender, EventArgs e)
        {

        }

        private void createEntryTab_Click(object sender, EventArgs e)
        {

        }

        private void createEntryLabel_Click(object sender, EventArgs e)
        {

        }

        private void createSpeciesLabel_Click(object sender, EventArgs e)
        {

        }

        private void createAgeLabel_Click(object sender, EventArgs e)
        {

        }

        private void createGenderLabel_Click(object sender, EventArgs e)
        {

        }

        private void createLocationLabel_Click(object sender, EventArgs e)
        {

        }

        private void createCityLabel_Click(object sender, EventArgs e)
        {

        }

        private void createStateLabel_Click(object sender, EventArgs e)
        {

        }

        private void createCountryLabel_Click(object sender, EventArgs e)
        {

        }

        private void createLongitudeLabel_Click(object sender, EventArgs e)
        {

        }

        private void createLatitudeLabel_Click(object sender, EventArgs e)
        {

        }

        private void requiredButterflyFieldsLabel_Click(object sender, EventArgs e)
        {

        }

        private void createSpeciesTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createAgeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void selectGenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void createCityTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createStateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createCountryTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createLongitudeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createLatitudeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void createEntryButton_Click(object sender, EventArgs e)
        {
            /*//if required boxes are left empty, reveal error
            if (String.IsNullOrEmpty(createCityTextBox.Text) && String.IsNullOrEmpty(createStateTextBox.Text) && String.IsNullOrEmpty(createCountryTextBox.Text) && String.IsNullOrEmpty(createLongitudeTextBox.Text) && String.IsNullOrEmpty(createLatitudeTextBox.Text))
            {
                cityError.SetError(createCityTextBox, "City field is empty!");
                stateError.SetError(createStateTextBox, "State field is empty!");
                countryError.SetError(createCountryTextBox, "Country field is empty!");
                longitudeError.SetError(createLongitudeTextBox, "Longitude field is empty!");
                latitudeError.SetError(createLatitudeTextBox, "Latitude field is empty!");
            }
            */

            string sightingcity = createCityTextBox.Text;
            string sightingstate = createStateTextBox.Text;
            string sightingcountry = createCountryTextBox.Text;
            string sightinglongitude = createLongitudeTextBox.Text;
            string sightinglatitude = createLatitudeTextBox.Text;
            string butterflyspecies = createSpeciesTextBox.Text;
            string butterflyage = createAgeTextBox.Text;
            string butterflygender = selectGenderComboBox.Text;

            if (!String.IsNullOrEmpty(createCityTextBox.Text) && !String.IsNullOrEmpty(createStateTextBox.Text) &&
                !String.IsNullOrEmpty(createCountryTextBox.Text) && !String.IsNullOrEmpty(createLongitudeTextBox.Text) &&
                !String.IsNullOrEmpty(createLatitudeTextBox.Text))
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    string EntryDate = createEntryDateTimePicker.Value.ToString("MM-dd-yyyy"); // user defined date
                    string EntryTime = createEntryDateTimePicker.Value.ToString("hh:mm tt"); // user defined date

                    // string bDate = DateTime.Now.ToString("MM-dd-yyyy"); // system date
                    //string bTime = DateTime.Now.ToString("hh:mm tt"); // system time
                    // inserting values into Butterfly table

                    string addButterfly =
                        "INSERT INTO Butterfly (Tag_ID, Species, Gender, Age, Date_of_Tagging, Time_of_Tagging, Emp_ID)" +
                        " VALUES (@TagID, @Species, @Gender, @Age, @Date_of_Tagging, @Time_of_Tagging, @Emp_ID)";

                    MySqlCommand Butterfly = new MySqlCommand(addButterfly, dbcon);
                    Butterfly.CommandText = addButterfly;
                    Butterfly.Parameters.AddWithValue("@TagID", null);
                    Butterfly.Parameters.AddWithValue("@Species", butterflyspecies);
                    Butterfly.Parameters.AddWithValue("@Gender", butterflygender);
                    Butterfly.Parameters.AddWithValue("@Age", butterflyage);
                    Butterfly.Parameters.AddWithValue("@Date_of_Tagging", EntryDate);
                    Butterfly.Parameters.AddWithValue("@Time_of_Tagging", EntryTime);
                    Butterfly.Parameters.AddWithValue("@Emp_ID", Emp_ID);
                    Butterfly.ExecuteNonQuery();

                    string addSightings =
                        "INSERT INTO Sighting_Locations (Sight_ID, Longitude, Latitude, City, State, Country, Employee_ID, Date_of_sighting, Time_of_sighting)" +
                        " VALUES (@SightID, @Longitude, @Latitude, @City, @State, @Country, @Employee_ID, @Date_of_sighting, @Time_of_sighting)";

                    MySqlCommand Sightings = new MySqlCommand(addSightings, dbcon);
                    Sightings.CommandText = addSightings;
                    Sightings.Parameters.AddWithValue("@SightID", null);
                    Sightings.Parameters.AddWithValue("@Longitude", sightinglongitude);
                    Sightings.Parameters.AddWithValue("@Latitude", sightinglatitude);
                    Sightings.Parameters.AddWithValue("@City", sightingcity);
                    Sightings.Parameters.AddWithValue("@State", sightingstate);
                    Sightings.Parameters.AddWithValue("@Country", sightingcountry);
                    Sightings.Parameters.AddWithValue("@Employee_ID", Emp_ID);
                    Sightings.Parameters.AddWithValue("@Date_of_sighting", EntryDate);
                    Sightings.Parameters.AddWithValue("@Time_of_sighting", EntryTime);
                    Sightings.ExecuteNonQuery();

                    MessageBox.Show("New butterfly entry created!");

                    //resetting the fields
                    foreach (Control item in createEntryTab.Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = "";
                        }

                    } //end foreach
                }
                else
                {
                    con.CloseConnection();
                    con.OpenConnection();
                }
            }
            if (String.IsNullOrEmpty(createCityTextBox.Text))
            {
                cityError.SetError(createCityTextBox, "City field is empty!");
            }
            else
            {
                cityError.Clear();
            }
            if (String.IsNullOrEmpty(createStateTextBox.Text))
            {
                stateError.SetError(createStateTextBox, "State field is empty!");
            }
            else
            {
                stateError.Clear();
            }
            if (String.IsNullOrEmpty(createCountryTextBox.Text))
            {
                countryError.SetError(createCountryTextBox, "Country field is empty!");
            }
            else
            {
                countryError.Clear();
            }
            if (String.IsNullOrEmpty(createLongitudeTextBox.Text))
            {
                longitudeError.SetError(createLongitudeTextBox, "Longitude field is empty!");
            }
            else
            {
                longitudeError.Clear();
            }
            if (String.IsNullOrEmpty(createLatitudeTextBox.Text))
            {
                latitudeError.SetError(createLatitudeTextBox, "Latitude field is empty!");
            }
            else
            {
                latitudeError.Clear();
            }
        }

        private void updateEntryTab_Click(object sender, EventArgs e)
        {

        }

        private void leaderboardTab_Click(object sender, EventArgs e)
        {

        }

        private void graphTab_Click(object sender, EventArgs e)
        {

        }

        private void createCityTextBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(createCityTextBox.Text))
            {
                cityError.SetError(createCityTextBox, "City field is empty!");
            }
            else
            {
                cityError.Clear();
            }
        }

        private void createStateTextBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(createStateTextBox.Text))
            {
                stateError.SetError(createStateTextBox, "State field is empty!");
            }
            else
            {
                stateError.Clear();
            }
        }

        private void createCountryTextBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(createCountryTextBox.Text))
            {
                countryError.SetError(createCountryTextBox, "Country field is empty!");
            }
            else
            {
                countryError.Clear();
            }
        }

        private void createLongitudeTextBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(createLongitudeTextBox.Text))
            {
                longitudeError.SetError(createLongitudeTextBox, "Longitude field is empty!");
            }
            else
            {
                longitudeError.Clear();
            }
        }

        private void createLatitudeTextBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(createLatitudeTextBox.Text))
            {
                latitudeError.SetError(createLatitudeTextBox, "Latitude field is empty!");
            }
            else
            {
                latitudeError.Clear();
            }
        }

        private void userNameBox_Leave(object sender, EventArgs e)
        {
            //when tabbing to next textbox, remove error if required field is filled
            if (String.IsNullOrEmpty(userNameBox.Text))
            {
                loginUserError.SetError(userNameBox, "User Name field is empty!");
            }
            else
            {
                loginUserError.Clear();
            }
        }

        private void passwordBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(passwordBox.Text))
            {
                loginPasswordError.SetError(passwordBox, "Password field is empty!");
            }
            else
            {
                loginPasswordError.Clear();
            }
        }

        private void createEmployeeUserNameBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeeUserNameBox.Text))
            {
                registerUserNameError.SetError(createEmployeeUserNameBox, "User Name field is empty!");
            }
            else
            {
                registerUserNameError.Clear();
            }
        }

        private void createEmployeePasswordBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeePasswordBox.Text))
            {
                registerPasswordError.SetError(createEmployeePasswordBox, "Password field is empty!");
            }
            else
            {
                registerPasswordError.Clear();
            }
        }

        private void createEmployeeNameBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeeNameBox.Text))
            {
                registerEmployeeNameError.SetError(createEmployeeNameBox, "Employee Name field is empty!");
            }
            else
            {
                registerEmployeeNameError.Clear();
            }
        }

        private void createEmployeePhoneNumberBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            //if (String.IsNullOrEmpty(createEmployeePhoneNumberBox.Text))
            //{
            //    loginPasswordError.SetError(createEmployeePhoneNumberBox, "Phone # field is empty!");
            //}
            //else
            //{
            //    loginPasswordError.Clear();
            //}
        }

        private void createEmployeeStreetBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeeStreetBox.Text))
            {
                registerStreetError.SetError(createEmployeeStreetBox, "Street field is empty!");
            }
            else
            {
                registerStreetError.Clear();
            }
        }

        private void createEmployeeCityBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeeCityBox.Text))
            {
                registerCityError.SetError(createEmployeeCityBox, "City field is empty!");
            }
            else
            {
                registerCityError.Clear();
            }
        }

        private void createEmployeeStateBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(createEmployeeStateBox.Text))
            {
                registerStateError.SetError(createEmployeeStateBox, "State field is empty!");
            }
            else
            {
                registerStateError.Clear();
            }
        }

        private void taggerNontaggerOptionsBox_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(taggerNontaggerOptionsBox.Text))
            {
                registerSelectPositionError.SetError(taggerNontaggerOptionsBox, "No Postion selected!");
            }
            else
            {
                registerSelectPositionError.Clear();
            }
        }

        //private void retreive_Click(object sender, EventArgs e)
        //{
        /*
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "butterflytrackingsystem.coriiiuartnb.us-east-1.rds.amazonaws.com";
            builder.UserID = "root";
            builder.Password = "master77";
            builder.Database = "BTS";
            MySqlConnection connection = new MySqlConnection(builder.ToString());
            connection.Open();
            */
        /*if (dbcon.State == ConnectionState.Open)
            {

                string retreiveAccount = "SELECT * FROM Employee WHERE (User_ID=@user)";
                MySqlCommand retreiveData = new MySqlCommand(retreiveAccount, dbcon);
                retreiveData.Parameters.AddWithValue("@user", userNameBox.Text);
                MySqlDataReader myReader;
                myReader = retreiveData.ExecuteReader();

                while (myReader.Read())
                {
                    string sUser = myReader.GetString("User_ID");
                    string sPassword = myReader.GetString("Password");
                    string sName = myReader.GetString("Name");
                    string sPhone = myReader.GetString("Phone_Number");
                    string sStreet = myReader.GetString("Street_Address");
                    string sCity = myReader.GetString("City");
                    string sState = myReader.GetString("State");
                    string sPosition = myReader.GetString("Position");

                    //updateUserNameTextBox.Text = sUser;

                    updatePasswordTextBox.Text = sPassword;
                    updateEmployeeNameTextBox.Text = sName;
                    updatePhoneNumberTextBox.Text = sPhone;
                    updateEmployeeStreetTextBox.Text = sStreet;
                    updateEmployeeCityTextBox.Text = sCity;
                    updateEmployeeStateTextBox.Text = sState;
                    positionOptionsUpdateComboBox.Text = sPosition;
                }
                myReader.Close();
            }
            else
            {
                con.CloseConnection();
                con.OpenConnection();
            }
    }*/


        private void createEntryDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            createEntryDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            createEntryDateTimePicker.Format = DateTimePickerFormat.Custom;
        }

        private void searchDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            searchDateTimePicker.Format = DateTimePickerFormat.Custom;
            searchDateTimePicker.ShowUpDown = true;
            searchDateTimePicker.CustomFormat = "MM/dd/yyyy";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.CustomFormat = "hh:mm tt";
        }

        public class DBConnect
        {
            public MySqlConnection connection;
            private string server;
            private string database;
            private string uid;
            private string password;

            //Constructor
            public DBConnect()
            {
                Initialize();
            }

            //Initialize values
            private void Initialize()
            {
                server = "butterflytrackingsystem.coriiiuartnb.us-east-1.rds.amazonaws.com";
                database = "BTS";
                uid = "root";
                password = "master77";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                                   database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

                connection = new MySqlConnection(connectionString);

            }

            //open connection to database
            public bool OpenConnection()
            {
                /*
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "butterflytrackingsystem.coriiiuartnb.us-east-1.rds.amazonaws.com";
            builder.UserID = "root";
            builder.Password = "master77";
            builder.Database = "BTS";
            MySqlConnection connection = new MySqlConnection(builder.ToString());
            */
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Cannot connect to server.  Contact administrator", "Error",
                                MessageBoxButtons.OK);
                            break;
                        case 1042:
                            MessageBox.Show("Unable to connect to any of the specified MySQL hosts", "Error",
                                MessageBoxButtons.OK);
                            break;
                        case 1045:
                            MessageBox.Show("Invalid username/password, please try again", "Error", MessageBoxButtons.OK);
                            break;
                    }
                    return false;
                }
            }

            //Close connection
            public bool CloseConnection()
            {
                try
                {
                    connection.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }

        }

        private MySqlDataAdapter entry2;
        private BindingSource bindingsource1;
        private DataSet DS;

        private void functionalitiesTabs_Click_1(object sender, EventArgs e)
        {
            if (dbcon.State == ConnectionState.Open)
            {
                // retreive leaderboard
                string retreiveLeaderBoard =
                    "SELECT Employee.User_ID, COUNT(*) AS Tags_Made FROM Employee JOIN Butterfly" +
                    " ON Employee.Employee_ID = Butterfly.Emp_ID GROUP BY Employee.User_ID" +
                    " ORDER BY Tags_Made DESC LIMIT 10;";

                MySqlCommand retreiveBoard = new MySqlCommand(retreiveLeaderBoard, dbcon);
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = retreiveBoard;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();
                bsource.DataSource = dbdataset;
                leaderboardGrid.DataSource = bsource;
                sda.Update(dbdataset);
                leaderboardGrid.AllowUserToAddRows = false;
                leaderboardGrid.RowHeadersVisible = false;

                // retreive entries
                string retreiveEntries =
                    "SELECT Tag_ID, Species, Gender, Age, Date_of_Tagging, Time_of_Tagging, Longitude, Latitude," +
                    " Sighting_Locations.City, Sighting_Locations.State, Country FROM BTS.Butterfly" +
                    " INNER JOIN BTS.Sighting_Locations ON (Butterfly.Tag_ID = Sighting_Locations.Sight_ID)" +
                    " INNER JOIN BTS.Employee ON(Sighting_Locations.Employee_ID = Employee.Employee_ID)" +
                    " WHERE(Employee.User_ID = '"+userNameBox.Text+"');"; //WHERE (Employee.User_ID =@user)

                entry2 = new MySqlDataAdapter(retreiveEntries, dbcon);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(entry2);
                DS = new DataSet();
                entry2.Fill(DS, "Entries");
                bindingsource1 = new BindingSource();
                bindingsource1.DataSource = DS.Tables[0];

                BindingNavigator bindingNavigator1 = new BindingNavigator();
                bindingNavigator1.BindingSource = bindingsource1;
                updateEntryGrid.DataSource = bindingsource1;

                // retreive account info
                string retreiveAccount = "SELECT * FROM Employee WHERE (User_ID=@user)";
                MySqlCommand retreiveData = new MySqlCommand(retreiveAccount, dbcon);
                retreiveData.Parameters.AddWithValue("@user", userNameBox.Text);
                MySqlDataReader myReader;
                myReader = retreiveData.ExecuteReader();


                while (myReader.Read())
                {
                    string sUser = myReader.GetString("User_ID");
                    string sPassword = myReader.GetString("Password");
                    string sName = myReader.GetString("Name");
                    string sPhone = myReader.GetString("Phone_Number");
                    string sStreet = myReader.GetString("Street_Address");
                    string sCity = myReader.GetString("City");
                    string sState = myReader.GetString("State");
                    string sPosition = myReader.GetString("Position");

                    //updateUserNameTextBox.Text = sUser;

                    updatePasswordTextBox.Text = sPassword;
                    updateEmployeeNameTextBox.Text = sName;
                    updatePhoneNumberTextBox.Text = sPhone;
                    updateEmployeeStreetTextBox.Text = sStreet;
                    updateEmployeeCityTextBox.Text = sCity;
                    updateEmployeeStateTextBox.Text = sState;
                    positionOptionsUpdateComboBox.Text = sPosition;
                }
                myReader.Close();
            }
            else
            {
                con.CloseConnection();
                con.OpenConnection();
            }
        }

        private void leaderboardGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadLeaderboardButton_Click(object sender, EventArgs e)
        {


        }

        private void loadLeaderboardButton_Click_1(object sender, EventArgs e)
        {
            if (dbcon.State == ConnectionState.Open)
            {
                string retreiveLeaderBoard =
                    "SELECT DISTINCT Employee.User_ID, COUNT(*) AS Tags_Made FROM Employee JOIN Butterfly" +
                    " ON Employee.Employee_ID = Butterfly.Emp_ID GROUP BY Employee.User_ID" +
                    " ORDER BY Tags_Made DESC LIMIT 10;";

                MySqlCommand retreiveBoard = new MySqlCommand(retreiveLeaderBoard, dbcon);
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = retreiveBoard;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();

                bsource.DataSource = dbdataset;
                leaderboardGrid.DataSource = bsource;
                sda.Update(dbdataset);
            }
            else
            {
                con.CloseConnection();
                con.OpenConnection();
            }
        }

        private void updateEntryGrid_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void loadEntry_Click(object sender, EventArgs e)
        {
            if (dbcon.State == ConnectionState.Open)
            {
                if (!String.IsNullOrEmpty(updateEntryTagIDBox.Text) && !String.IsNullOrEmpty(updateEntrySpeciesBox.Text) &&
                !String.IsNullOrEmpty(updateEntryGenderComboBox.Text) && !String.IsNullOrEmpty(updateEntryAgeBox.Text) &&
                !String.IsNullOrEmpty(updateEntryLongitudeBox.Text) && !String.IsNullOrEmpty(updateEntryLatitudeBox.Text) &&
                !String.IsNullOrEmpty(updateEntryCityBox.Text) && !String.IsNullOrEmpty(updateEntryStateBox.Text)
                && !String.IsNullOrEmpty(updateEntryCountryBox.Text))
                {
                    DataGridViewRow row = updateEntryGrid.Rows[i];
                    row.Cells[0].Value = updateEntryTagIDBox.Text;
                    row.Cells[1].Value = updateEntrySpeciesBox.Text;
                    row.Cells[2].Value = updateEntryGenderComboBox.Text;
                    row.Cells[3].Value = updateEntryAgeBox.Text;
                    row.Cells[6].Value = updateEntryLongitudeBox.Text;
                    row.Cells[7].Value = updateEntryLatitudeBox.Text;
                    row.Cells[8].Value = updateEntryCityBox.Text;
                    row.Cells[9].Value = updateEntryStateBox.Text;
                    row.Cells[10].Value = updateEntryCountryBox.Text;






                    string updateEntry_sql = "UPDATE Butterfly SET Species='" + updateEntrySpeciesBox.Text +
                                            "', Gender='" + updateEntryGenderComboBox.Text + "', Age='" +
                                            updateEntryAgeBox.Text + "'WHERE Tag_ID='" + updateEntryTagIDBox.Text + "' ;";

                    MySqlCommand updateEntry = new MySqlCommand(updateEntry_sql, dbcon);
                    MySqlDataReader updateReader = updateEntry.ExecuteReader();

                    updateReader.Close();


                    string updateEntrySight_sql = "UPDATE Sighting_Locations SET Longitude='" + updateEntryLongitudeBox.Text +
                                                "', Latitude='" + updateEntryLatitudeBox.Text + "', City='" +
                                                updateEntryCityBox.Text + "', State='" + updateEntryStateBox.Text + "', Country='" +
                                                updateEntryCountryBox.Text + "'WHERE Sight_ID='" + updateEntryTagIDBox.Text + "' ;";

                    MySqlCommand updateEntrySight = new MySqlCommand(updateEntrySight_sql, dbcon);
                    MySqlDataReader updateSightReader = updateEntrySight.ExecuteReader();
                    //updateSightReader = updateEntrySight.ExecuteReader();
                    updateSightReader.Close();
                    MessageBox.Show("Entry up to date!");
                }


                else MessageBox.Show("Please fill missing fields !");

            }

            
            
           
        }
            

        private void updateEntryLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateEntryTagIDBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntrySpeciesBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryCityBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryStateBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryCountryBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryLongitudeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryLatitudeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryAgeBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryGenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void updateEntryRequiredTagIDLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateLocationLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateTagIDLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateSpeciesLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateCityLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateStateLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateCountryLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateLongitudeLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateLatitudeLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateGenderLabel_Click(object sender, EventArgs e)
        {

        }

        private void updateAgeLabel_Click(object sender, EventArgs e)
        {

        }
        int i;
        private void updateEntryGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void updateEntryTagIDBox_Leave(object sender, EventArgs e)
        {
            //when tabbing, remove error if required field is filled
            if (String.IsNullOrEmpty(updateEntryTagIDBox.Text))
            {
                registerStreetError.SetError(updateEntryTagIDBox, "Tag ID field Invalid!");
            }
            else
            {
                registerStreetError.Clear();
            }
        }

        private void updateEntryGrid_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            i = e.RowIndex;
            DataGridViewRow row = updateEntryGrid.Rows[i];
            updateEntryTagIDBox.Text = row.Cells[0].Value.ToString();
            updateEntrySpeciesBox.Text = row.Cells[1].Value.ToString();
            updateEntryGenderComboBox.Text = row.Cells[2].Value.ToString();
            updateEntryAgeBox.Text = row.Cells[3].Value.ToString();
            updateEntryLongitudeBox.Text = row.Cells[6].Value.ToString();
            updateEntryLatitudeBox.Text = row.Cells[7].Value.ToString();
            updateEntryCityBox.Text = row.Cells[8].Value.ToString();
            updateEntryStateBox.Text = row.Cells[9].Value.ToString();
            updateEntryCountryBox.Text = row.Cells[10].Value.ToString();
        }
        
        private void ResetDate_Click(object sender, EventArgs e){}
        private void ResetTime_Click(object sender, EventArgs e){}

        private void ResetDate_Click_1(object sender, EventArgs e)
        {
            if (cal == false)
            {
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.ShowUpDown = true;
                searchDateTimePicker.CustomFormat = "MM-dd-yyyy";
                cal = true;
            }
            else
            {
                searchDateTimePicker.Format = DateTimePickerFormat.Custom;
                searchDateTimePicker.CustomFormat = " ";
                cal = false;
            }

        }

        private void ResetTime_Click_1(object sender, EventArgs e)
        {
            if (tim == false)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.ShowUpDown = true;
                dateTimePicker1.CustomFormat = "hh:mm tt";
                tim = true;
            }
            else
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = " ";
                tim = false;
            }
        }
    }
}
