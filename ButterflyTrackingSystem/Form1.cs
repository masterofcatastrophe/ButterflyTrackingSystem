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

namespace ButterflyTrackingSystem
{
    public partial class BTS : Form
    {
        static DBConnect con = new DBConnect();
        MySqlConnection dbcon = con.connection;
        
        public BTS()
        {
            InitializeComponent();
            con.OpenConnection();
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
                    loginPanel.Visible = false;

                    registrationPanel.Visible = false;

                    mainPanel.Visible = true;
                }
                else
                {
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
            }//end foreach
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
                    MessageBox.Show("user exists !");
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
            loginPanel.Visible = true; registrationPanel.Visible = false;

            //resetting the fields when going to login
            foreach (Control item in loginPanel.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }//end foreach
            
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
            //To-DO: update query here

            MessageBox.Show("Account successfully Updated!");

            foreach (Control item in updateAccountTab.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }//end foreach
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
            //resetting the fields when going to login
            foreach (Control item in searchTab.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }//end foreach
        }

        private void searchTab_Click(object sender, EventArgs e)
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

            if (!String.IsNullOrEmpty(createCityTextBox.Text) && !String.IsNullOrEmpty(createStateTextBox.Text) && !String.IsNullOrEmpty(createCountryTextBox.Text) && !String.IsNullOrEmpty(createLongitudeTextBox.Text) && !String.IsNullOrEmpty(createLatitudeTextBox.Text))
            {
                MessageBox.Show("New butterfly entry created!");

                //resetting the fields
                foreach (Control item in createEntryTab.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }

                }//end foreach
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

        private void retreive_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "butterflytrackingsystem.coriiiuartnb.us-east-1.rds.amazonaws.com";
            builder.UserID = "root";
            builder.Password = "master77";
            builder.Database = "BTS";
            MySqlConnection connection = new MySqlConnection(builder.ToString());
            connection.Open();
            
            string retreiveAccount = "SELECT * FROM Employee WHERE (User_ID=@user)";
            MySqlCommand retreiveData = new MySqlCommand(retreiveAccount, connection);
            retreiveData.Parameters.AddWithValue("@user",userNameBox.Text);
            MySqlDataReader myReader;
            myReader=retreiveData.ExecuteReader();

            while(myReader.Read())
            {
                string sUser = myReader.GetString("User_ID");
                string sPassword = myReader.GetString("Password");
                string sName = myReader.GetString("Name");
                string sPhone = myReader.GetString("Phone_Number");
                string sStreet = myReader.GetString("Street_Address");
                string sCity = myReader.GetString("City");
                string sState = myReader.GetString("State");
                string sPosition = myReader.GetString("Position");
                updateUserNameTextBox.Text = sUser;
                updatePasswordTextBox.Text = sPassword;
                updateEmployeeNameTextBox.Text = sName;
                updatePhoneNumberTextBox.Text = sPhone;
                updateEmployeeStreetTextBox.Text = sStreet;
                updateEmployeeCityTextBox.Text = sCity;
                updateEmployeeStateTextBox.Text = sState;
                positionOptionsUpdateComboBox.Text = sPosition;
            }
            connection.Close();
        }
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
                        MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 1042:
                        MessageBox.Show("Unable to connect to any of the specified MySQL hosts", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
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
}
