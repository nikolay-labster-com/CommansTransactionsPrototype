namespace CommandTransactionsPrototype.PropEditors
{
	partial class ucMissionPropEditor
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtId = new System.Windows.Forms.TextBox();
			this.btnDeleteThisMission = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(126, 39);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(240, 26);
			this.txtName.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 74);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Description";
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(126, 71);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(240, 26);
			this.txtDescription.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Id";
			// 
			// txtId
			// 
			this.txtId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtId.Location = new System.Drawing.Point(126, 7);
			this.txtId.Name = "txtId";
			this.txtId.Size = new System.Drawing.Size(240, 26);
			this.txtId.TabIndex = 4;
			// 
			// btnDeleteThisMission
			// 
			this.btnDeleteThisMission.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDeleteThisMission.Location = new System.Drawing.Point(54, 133);
			this.btnDeleteThisMission.Name = "btnDeleteThisMission";
			this.btnDeleteThisMission.Size = new System.Drawing.Size(232, 43);
			this.btnDeleteThisMission.TabIndex = 6;
			this.btnDeleteThisMission.Text = "Delete";
			this.btnDeleteThisMission.UseVisualStyleBackColor = true;
			// 
			// ucMissionPropEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnDeleteThisMission);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtId);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtName);
			this.Name = "ucMissionPropEditor";
			this.Size = new System.Drawing.Size(369, 413);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtId;
		private System.Windows.Forms.Button btnDeleteThisMission;
	}
}
