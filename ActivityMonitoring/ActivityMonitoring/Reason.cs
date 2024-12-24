using ActivityMonitor.Utilities;
using ActivityMonitor.Utilities.Implementation;
using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ActivityMonitor.windows
{
    public partial class Reason : Form
    {
        public Reason()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                int selected = checkedListBox1.SelectedIndex;
                string reason = checkedListBox1.Items[selected].ToString();
                Console.WriteLine(reason);
                ManageDB m = new ManageDB();
                DateTime PauseTime = m.GetLastTime();
                var diffOfDates = DateTime.Now - PauseTime;

                m.InsertUserActivity("Away", reason,PauseTime.ToString());
                this.Close();
            }
            else if (textBox1.Text.Trim().Length > 0)
            {
                var reason = textBox1.Text.Trim();
                Console.WriteLine(reason);
                this.Close();
            }

        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            var reason =
                textBox1.Text.Trim();
            Console.WriteLine(reason);
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                int selected = checkedListBox1.SelectedIndex;
                string reason = checkedListBox1.Items[selected].ToString();
                Console.WriteLine(reason);
                ManageDB mdb=new ManageDB();
                this.Close();
            }
            else if (textBox1.Text.Trim().Length > 0)
            {
                var reason = textBox1.Text.Trim();
                Console.WriteLine(reason);
                this.Close();
            }
        }

        private void checkedListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int selected = checkedListBox1.SelectedIndex;
            this.Text = checkedListBox1.Items[selected].ToString();
            Console.WriteLine(this.Text);
        }

        private void Reasons_Load_1(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
