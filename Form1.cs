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

		private StateHistoryTree _history;
		private ItemToCollectionDictionary<ICommand, IGenericUiElement> _allCommands;
		private Dictionary<ICommand, Func<object[]>> _paramMakers;

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
				InitCommands();
				//_engine.Missions.Add(new Mission());
				//_engine.Missions.Add(new Mission());
				RefreshTrees();
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		private object GetSelectedNodeTag(TreeView trv)
		{
			if (trv.SelectedNode != null)
			{
				return trv.SelectedNode.Tag;
			}
			return null;
		}

		private object[] ToArr(params object[] arr)
		{
			return arr;
		}

		private void InitCommands()
		{
			_allCommands = new ItemToCollectionDictionary<ICommand, IGenericUiElement>();
			_paramMakers = new Dictionary<ICommand, Func<object[]>>();

			AssociateCommand(new CommandAddMission(trvModel, _engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuAddMission,
				btnAddMission);

			AssociateCommand(new CommandDeleteMission(trvModel, _engine, _history),
				() => ToArr(GetSelectedNodeTag(trvModel)),
				mnuDeleteMission);

			//AssociateCommand(_allCommands, new CommandAddConversation(trvModel, _engine, _history),
			//	mnuAddConversation);

			//AssociateCommand(_allCommands, new CommandDeleteConversation(trvModel, _engine, _history),
			//	mnuDeleteConversation);

			AssociateCommand(new CommandUndo(_history),
				() => ToArr(),
				btnUndo);

			AssociateCommand(new CommandRedo(trvHistory, _history),
				() => ToArr(GetSelectedNodeTag(trvHistory)),
				btnRedo);

			//AssociateCommand(_allCommands, new CommandAddTask(trvModel, _engine, _history),
			//	mnuAddTask);

			//AssociateCommand(_allCommands, new CommandDeleteTask(trvModel, _engine, _history),
			//	mnuDeleteTask);
		}

		private void UpdateControlsAvailability()
		{
			foreach (var oneKvp in _allCommands.GetAllControls())
			{
				var paramsMaker = _paramMakers[oneKvp.Value];
				oneKvp.Key.Enabled = oneKvp.Value.CanBeExecutedOn(paramsMaker());
			}
		}

		private void AssociateCommand(
			ICommand command,
			Func<object[]> paramsMaker,
			params object[] controls)
		{
			_paramMakers.Add(command, paramsMaker);

			foreach (var oneControl in controls)
			{
				IGenericUiElement uiElement;
				if (oneControl is Control)
				{
					uiElement = new ControlItem((Control) oneControl);
					
				}
				else if (oneControl is ToolStripItem)
				{
					uiElement = new MenuItem((ToolStripItem) oneControl);
				}
				else
				{
					throw new InvalidOperationException("Unsupported item was passed.");
				}
				uiElement.Activated += (sender, args) =>
				{
					try
					{
						command.InitiateExecution(paramsMaker());
						RefreshTrees();
						UpdateControlsAvailability();
					}
					catch (Exception ex)
					{
						ShowError(ex);
					}
				};
				_allCommands.Add(command, uiElement);
			}
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

		private class ItemToCollectionDictionary<TKey, TValue>
		{
			private Dictionary<TKey, List<TValue>> _dict = new Dictionary<TKey, List<TValue>>();
			private Dictionary<TValue, TKey> _dict2 = new Dictionary<TValue, TKey>();

			public void Add(TKey key, params TValue[] valueArr)
			{
				List<TValue> list;
				if (!_dict.TryGetValue(key, out list))
				{
					list = new List<TValue>();
					_dict[key] = list;
				}
				list.AddRange(valueArr);
				foreach (var oneValue in valueArr)
				{
					_dict2.Add(oneValue, key);
				}
			}

			public TKey GetKeyByValue(TValue value)
			{
				return _dict2[value];
			}

			public IEnumerable<KeyValuePair<TValue, TKey>> GetAllControls()
			{
				return _dict2;
			}
		}

		private interface IGenericUiElement
		{
			bool Enabled { get; set; }
			bool Visible { get; set; }
			event EventHandler Activated;
		}

		private class MenuItem : IGenericUiElement
		{
			public event EventHandler Activated;

			private readonly ToolStripItem _item;

			public MenuItem(ToolStripItem item)
			{
				_item = item;
				_item.Click += Item_Click;
			}

			private void Item_Click(object sender, EventArgs e)
			{
				var handler = Activated;
				if (handler != null)
				{
					handler(sender, e);
				}
			}
			public bool Enabled
			{
				get { return _item.Enabled; }
				set { _item.Enabled = value; }
			}

			public bool Visible
			{
				get { return _item.Visible; }
				set { _item.Visible = value; }
			}
		}
		private class ControlItem : IGenericUiElement
		{
			public event EventHandler Activated;
			private readonly Control _item;

			public ControlItem(Control item)
			{
				_item = item;
				_item.Click += Item_Click;
			}
			private void Item_Click(object sender, EventArgs e)
			{
				var handler = Activated;
				if (handler != null)
				{
					handler(sender, e);
				}
			}
			public bool Enabled
			{
				get { return _item.Enabled; }
				set { _item.Enabled = value; }
			}

			public bool Visible
			{
				get { return _item.Visible; }
				set { _item.Visible = value; }
			}
		}

		private void trvModel_AfterSelect(object sender, TreeViewEventArgs e)
		{
			UpdateControlsAvailability();
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
			UpdateControlsAvailability();
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
