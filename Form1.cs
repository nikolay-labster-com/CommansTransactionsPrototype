using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Commands;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype
{
	public partial class Form1 : Form
	{
		private Engine _engine = new Engine();
		private UiCommandControl _commandControl;
		private StateHistoryTree _history;
		private PropEditors.ucMissionPropEditor _missionPropEditor;

		//private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
		//private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				_history = new StateHistoryTree();
				_history.Init();
				_commandControl = new UiCommandControl(RefreshTrees, ShowError);
				InitCommands();

				_missionPropEditor = new PropEditors.ucMissionPropEditor();
				_missionPropEditor.InitCommands(_commandControl);

				RefreshTrees();
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private void InitCommands()
		{

			_commandControl.AssociateCommand(new CommandAddMission(trvModel, _engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuAddMission,
				btnAddMission);

			_commandControl.AssociateCommand(new CommandDeleteMission(trvModel, _engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuDeleteMission);

			_commandControl.AssociateCommand(new CommandAddConversation(_engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuAddConversation);

			_commandControl.AssociateCommand(new CommandDeleteConversation(_engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuDeleteConversation);

			_commandControl.AssociateCommand(new CommandUndo(_history),
				() => ToArr(),
				btnUndo);

			_commandControl.AssociateCommand(new CommandRedo(trvHistory, _history),
				() => ToArr(GetSelectedNodeTag(trvHistory)),
				btnRedo);

			_commandControl.AssociateCommand(new CommandAddTask(_engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuAddTask);

			_commandControl.AssociateCommand(new CommandDeleteTask(_engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel), GetSelectedNodeParentTag(trvModel)),
				mnuDeleteTask);

			_commandControl.RegisterCommand(new EditFieldCommand(_history));
		}
		private object GetSelectedNodeTag(TreeView trv)
		{
			if (trv.SelectedNode != null)
			{
				return trv.SelectedNode.Tag;
			}
			return null;
		}
		private object GetSelectedNodeParentTag(TreeView trv)
		{
			if (trv.SelectedNode != null && trv.SelectedNode.Parent != null)
			{
				return trv.SelectedNode.Parent.Tag;
			}
			return null;
		}
		private object[] ToArr(params object[] arr)
		{
			return arr;
		}



		private void ShowError(Exception ex)
		{
			MessageBox.Show(ex.Message);
		}

		private void DoTestChanges()
		{
			var cmd = new CommandAddMission(trvModel, _engine, _history);
			cmd.InitiateExecution();
			RefreshTrees();
		}

		private void RefreshTrees()
		{
			TreeRenderer.RenderModel(trvModel, _engine);
			HistoryTreeRenderer.RenderHistory(trvHistory, _history);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				DoTestChanges();
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			//try
			//{
			//	var cmd = new CommandUndo(_history);
			//	cmd.InitiateExecution();
			//	RefreshTrees();
			//}
			//catch (Exception ex)
			//{
			//	ShowError(ex);
			//}
		}

		private void btnRedo_Click(object sender, EventArgs e)
		{
			//try
			//{
			//	var cmd = new CommandRedo(trvHistory, _history);
			//	cmd.InitiateExecution();
			//	RefreshTrees();
			//}
			//catch (Exception ex)
			//{
			//	ShowError(ex);
			//}
		}


		private void trvModel_AfterSelect(object sender, TreeViewEventArgs e)
		{
			pnlProp.Controls.Clear();
			if (trvModel.SelectedNode != null)
			{
				var mission = trvModel.SelectedNode.Tag as Mission;
				_missionPropEditor.Mission = mission;
				if (mission != null)
				{
					pnlProp.Controls.Add(_missionPropEditor);
					_missionPropEditor.Dock = DockStyle.Fill;
				}
			}

			_commandControl.UpdateControlsAvailability();
		}

		private void trvModel_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				var node = trvModel.GetNodeAt(e.Location);
				trvModel.SelectedNode = node;
				if (node != null)
				{
					mnuCommonMenu.Show(trvModel, e.Location);
				}
			}
		}

		private void trvHistory_AfterSelect(object sender, TreeViewEventArgs e)
		{
			_commandControl.UpdateControlsAvailability();
		}
	}

	internal static class TreeRenderer
	{

		public static void RenderModel(TreeView treeView, Engine engine)
		{
			treeView.Nodes.Clear();
			var node = AddNode("engine", engine, treeView.Nodes);
			RenderEngine(node, engine);
		}

		public static void RenderEngine(TreeNode node, Engine model)
		{
			node.Nodes.Clear();

			foreach (var oneConversation in model.Conversations)
			{
				var subNode = AddNode(oneConversation.ToString(), oneConversation, node.Nodes);
			}
			foreach (var oneMission in model.Missions)
			{
				var subNode = AddNode(oneMission.ToString(), oneMission, node.Nodes);
				RenderMission(subNode, oneMission);
			}

			node.Expand();
		}

		public static void RenderMission(TreeNode node, Mission model)
		{
			node.Nodes.Clear();

			foreach (var oneTask in model.Tasks)
			{
				var subNode = AddNode(oneTask.ToString(), oneTask, node.Nodes);
			}

			node.Expand();
		}
		private static TreeNode AddNode(string text, object item, TreeNodeCollection parentCollection)
		{
			var node = new TreeNode();
			node.Text = text;
			node.Tag = item;
			parentCollection.Add(node);
			return node;
		}

	}

	internal static class HistoryTreeRenderer
	{
		public static void RenderHistory(TreeView treeView, StateHistoryTree history)
		{
			treeView.Nodes.Clear();
			RenderNode(treeView.Nodes, history.RootNode, history.CurrentNode);
			treeView.ExpandAll();
		}

		private static void RenderNode(TreeNodeCollection collection, HistoryTreeNode node, HistoryTreeNode currentNode)
		{
			var nNode = AddNode(node.Label, node, collection);
			if (node == currentNode)
			{
				nNode.ForeColor = Color.Red;
			}
			//collection.Add(nNode);
			foreach (var oneSubNode in node.Children)
			{
				RenderNode(nNode.Nodes, oneSubNode, currentNode);
			}
		}

		private static TreeNode AddNode(string text, object item, TreeNodeCollection parentCollection)
		{
			var node = new TreeNode();
			node.Text = text;
			node.Tag = item;
			parentCollection.Add(node);
			return node;
		}
	}
}
