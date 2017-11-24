using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandRedo : ICommand
	{
		private readonly TreeView _trvHistory;
		private readonly StateHistoryTree _history;

		public CommandRedo(TreeView trvHistory, StateHistoryTree history)
		{
			_history = history;
			_trvHistory = trvHistory;
		}

		private bool TryGetNextIndex(ref int index)
		{
			if (_history.CurrentNode.Children.Count == 1)
			{
				index = 0;
				return true;
			}
			var selectedNode = _trvHistory.SelectedNode;
			if (selectedNode != null)
			{
				var histNode = selectedNode.Tag as HistoryTreeNode;
				if (histNode != null)
				{
					if (histNode.Parent == _history.CurrentNode)
					{
						index = selectedNode.Index;
						return true;
					}
				}
			}
			return false;
		}

		public bool CanBeExecutedNow
		{
			get
			{
				int dummy = 0;
				return _history.CanRedo() && TryGetNextIndex(ref dummy);
			}
		}
		//private bool TryGetNextIndex(HistoryTreeNode histNode, ref int index)
		//{
		//	if (_history.CurrentNode.Children.Count == 1)
		//	{
		//		index = 0;
		//		return true;
		//	}
		//	var selectedNode = _trvHistory.SelectedNode;
		//	if (selectedNode != null)
		//	{
		//		var histNode = selectedNode.Tag as HistoryTreeNode;
		//		if (histNode != null)
		//		{
		//			if (histNode.Parent == _history.CurrentNode)
		//			{
		//				index = selectedNode.Index;
		//				return true;
		//			}
		//		}
		//	}
		//	return false;
		//}
		//public bool CanBeExecutedOn(object obj)
		//{
		//	int dummy = 0;
		//	return _history.CanRedo() && TryGetNextIndex((HistoryTreeNode) obj, ref dummy);
		//}

		public void InitiateExecution()
		{
			int index = 0;
			if (TryGetNextIndex(ref index))
			{
				_history.Redo(index);
			}
		}
	}
}
