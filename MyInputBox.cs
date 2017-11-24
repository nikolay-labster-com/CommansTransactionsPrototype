using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandTransactionsPrototype
{
	public partial class MyInputBox : Form
	{
		public MyInputBox()
		{
			InitializeComponent();
		}

		private void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				PressOk();
			}
		}

		private void PressOk()
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Hide();
		}

		public static string GetText(string title, string defaultText = null)
		{
			using (var nForm = new MyInputBox())
			{
				nForm.Text = title;
				nForm.txtText.Text = defaultText;
				if (nForm.ShowDialog() == DialogResult.OK)
				{
					return nForm.txtText.Text;
				}
				return null;
			}
		}

		private void MyInputBox_Load(object sender, EventArgs e)
		{
			txtText.Focus();
			txtText.SelectAll();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			PressOk();
		}
	}
}
