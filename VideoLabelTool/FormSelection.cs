using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoLabelTool
{
    public partial class FormSelection : Form
    {
        //List<int> selectedPersonID { get; set; }
        List<int> selectedPersonID;

        //public class Person
        //{
        //    public int Age { get; set;}
        //    public string FirstName { get; set; }
        //    public string LastName { get; set; }
        //}

        public FormSelection(List<int> selectedPersonID)
        {
            InitializeComponent();
            this.selectedPersonID = selectedPersonID;

            //List<Person> people = new List<Person>();
            //people.Add(new Person { Age = 25, FirstName = "Alex", LastName = "Johnson" });
            //people.Add(new Person { Age = 23, FirstName = "Jack", LastName = "Jones" });
            //people.Add(new Person { Age = 35, FirstName = "Mike", LastName = "Williams" });
            //people.Add(new Person { Age = 25, FirstName = "Gill", LastName = "JAckson" });
            //this.listBoxSelection.DataSource = people;
            //this.listBoxSelection.DisplayMember = "FirstName";
            //this.listBoxSelection.ValueMember = "Age";            

            this.listBoxSelection.DataSource = this.selectedPersonID;
            //this.listBoxSelection.DisplayMember = "FirstName";
            //this.listBoxSelection.ValueMember = "Age";
        }


        private void listBoxSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
