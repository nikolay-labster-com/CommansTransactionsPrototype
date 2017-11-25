namespace CommandTransactionsPrototype
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnRedo = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.trvModel = new System.Windows.Forms.TreeView();
			this.mnuCommonMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuAddMission = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteMission = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddTask = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteTask = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuAddConversation = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDeleteConversation = new System.Windows.Forms.ToolStripMenuItem();
			this.btnAddMission = new System.Windows.Forms.Button();
			this.trvHistory = new System.Windows.Forms.TreeView();
			this.pnlProp = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.mnuCommonMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnRedo
			// 
			this.btnRedo.Location = new System.Drawing.Point(389, 260);
			this.btnRedo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Size = new System.Drawing.Size(134, 66);
			this.btnRedo.TabIndex = 8;
			this.btnRedo.Text = "Redo";
			this.btnRedo.UseVisualStyleBackColor = true;
			this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Location = new System.Drawing.Point(389, 162);
			this.btnUndo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(134, 66);
			this.btnUndo.TabIndex = 9;
			this.btnUndo.Text = "Undo";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// trvModel
			// 
			this.trvModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.trvModel.Location = new System.Drawing.Point(595, 58);
			this.trvModel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.trvModel.Name = "trvModel";
			this.trvModel.Size = new System.Drawing.Size(301, 535);
			this.trvModel.TabIndex = 4;
			this.trvModel.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvModel_AfterSelect);
			this.trvModel.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvModel_NodeMouseClick);
			// 
			// mnuCommonMenu
			// 
			this.mnuCommonMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.mnuCommonMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAddMission,
            this.mnuDeleteMission,
            this.mnuAddTask,
            this.mnuDeleteTask,
            this.mnuAddConversation,
            this.mnuDeleteConversation});
			this.mnuCommonMenu.Name = "mnuCommonMenu";
			this.mnuCommonMenu.Size = new System.Drawing.Size(244, 184);
			// 
			// mnuAddMission
			// 
			this.mnuAddMission.Name = "mnuAddMission";
			this.mnuAddMission.Size = new System.Drawing.Size(243, 30);
			this.mnuAddMission.Text = "Add Mission";
			// 
			// mnuDeleteMission
			// 
			this.mnuDeleteMission.Name = "mnuDeleteMission";
			this.mnuDeleteMission.Size = new System.Drawing.Size(243, 30);
			this.mnuDeleteMission.Text = "Delete Mission";
			// 
			// mnuAddTask
			// 
			this.mnuAddTask.Name = "mnuAddTask";
			this.mnuAddTask.Size = new System.Drawing.Size(243, 30);
			this.mnuAddTask.Text = "Add Task";
			// 
			// mnuDeleteTask
			// 
			this.mnuDeleteTask.Name = "mnuDeleteTask";
			this.mnuDeleteTask.Size = new System.Drawing.Size(243, 30);
			this.mnuDeleteTask.Text = "Delete Task";
			// 
			// mnuAddConversation
			// 
			this.mnuAddConversation.Name = "mnuAddConversation";
			this.mnuAddConversation.Size = new System.Drawing.Size(243, 30);
			this.mnuAddConversation.Text = "Add Conversation";
			// 
			// mnuDeleteConversation
			// 
			this.mnuDeleteConversation.Name = "mnuDeleteConversation";
			this.mnuDeleteConversation.Size = new System.Drawing.Size(243, 30);
			this.mnuDeleteConversation.Text = "Delete Conversation";
			// 
			// btnAddMission
			// 
			this.btnAddMission.Location = new System.Drawing.Point(472, 553);
			this.btnAddMission.Name = "btnAddMission";
			this.btnAddMission.Size = new System.Drawing.Size(113, 40);
			this.btnAddMission.TabIndex = 11;
			this.btnAddMission.Text = "Add Mission";
			this.btnAddMission.UseVisualStyleBackColor = true;
			// 
			// trvHistory
			// 
			this.trvHistory.Location = new System.Drawing.Point(12, 13);
			this.trvHistory.Name = "trvHistory";
			this.trvHistory.Size = new System.Drawing.Size(298, 580);
			this.trvHistory.TabIndex = 12;
			this.trvHistory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvHistory_AfterSelect);
			// 
			// pnlProp
			// 
			this.pnlProp.Location = new System.Drawing.Point(902, 58);
			this.pnlProp.Name = "pnlProp";
			this.pnlProp.Size = new System.Drawing.Size(280, 535);
			this.pnlProp.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(353, 342);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(203, 131);
			this.label1.TabIndex = 14;
			this.label1.Text = "Redo button might not be available when the are two or more possible ways. select" +
    " needed node and redo should become available.";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1194, 606);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pnlProp);
			this.Controls.Add(this.trvHistory);
			this.Controls.Add(this.btnAddMission);
			this.Controls.Add(this.btnRedo);
			this.Controls.Add(this.btnUndo);
			this.Controls.Add(this.trvModel);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.mnuCommonMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnRedo;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.TreeView trvModel;
		private System.Windows.Forms.ContextMenuStrip mnuCommonMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuAddMission;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteMission;
		private System.Windows.Forms.ToolStripMenuItem mnuAddTask;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteTask;
		private System.Windows.Forms.ToolStripMenuItem mnuAddConversation;
		private System.Windows.Forms.ToolStripMenuItem mnuDeleteConversation;
		private System.Windows.Forms.Button btnAddMission;
		private System.Windows.Forms.TreeView trvHistory;
		private System.Windows.Forms.Panel pnlProp;
		private System.Windows.Forms.Label label1;
	}
}

