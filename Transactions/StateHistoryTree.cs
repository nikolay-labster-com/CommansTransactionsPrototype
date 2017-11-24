using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTransactionsPrototype.Transactions
{
	public interface ITransactionProvider // interface for Commands to get transaction
	{
		TranScope GetCurrentTransaction();
	}

	public interface IUndoRedoManager
	{
		bool CanUndo();
		bool CanRedo();
		void Undo();
		void Redo(int historyIndex);
	}

	public class StateHistoryTree : ITransactionProvider, IUndoRedoManager
	{
		// we have one transaction always open even no changes initiated
		// just because it is possible that one of the provider (dependency property change observer)
		// monitors changes constantly.		
		private TranScope _currentTransaction;

		private bool _initialized;
		private HistoryTreeNode _rootNode;
		private HistoryTreeNode _currentNode;

		public void Init()
		{
			if (_initialized)
				throw new InvalidOperationException("Already initialized.");

			CreateNewCurrentTransaction();

			_rootNode = new HistoryTreeNode { Label = "Initial state" };
			_currentNode = _rootNode;

			_initialized = true;
		}

		private void CreateNewCurrentTransaction()
		{
			_currentTransaction = TransationFactory.CreateNewTransactionScope();
			_currentTransaction.TransactionCompleted += TransactionCompleted;
		}

		private void DestroyTransaction(TranScope tran)
		{
			tran.TransactionCompleted -= TransactionCompleted;
			tran.Dispose();
		}

		void TransactionCompleted(object sender, TransactionCompletedEventArgs e)
		{
			var tranScope = (TranScope)sender;
			if (tranScope != _currentTransaction)
				throw new InvalidOperationException("Completed transaction must be the current one!");

			CreateNewCurrentTransaction();

			ProduceHistoryNodes(e.Transitions);

			DestroyTransaction(tranScope);
		}

		private void ProduceHistoryNodes(List<ITransition> transitions)
		{
			var newNode = new HistoryTreeNode
			{
				Label = GetLabelForNode(transitions),
				Parent = _currentNode,
				Transitions = transitions
			};
			_currentNode.Children.Add(newNode);
			_currentNode = newNode;
		}

		private static string GetLabelForNode(List<ITransition> transitions)
		{
			ITransition keyTransition = transitions.First(t => t.State1.IsMacroState);
			if (keyTransition == null)
			{
				keyTransition = transitions.First();
				if (keyTransition == null)
				{
					return "Null node";
				}
			}
			if (string.IsNullOrEmpty(keyTransition.State1.Label))
			{
				return keyTransition.State1.DateOfState.ToString("yyyy-MM-dd HH:mm:ss",
					System.Globalization.CultureInfo.InvariantCulture);
			}
			return keyTransition.State1.Label;
		}

		public TranScope GetCurrentTransaction()
		{
			return _currentTransaction;
		}

		public HistoryTreeNode RootNode
		{
			get { return _rootNode; }
		}

		public HistoryTreeNode CurrentNode
		{
			get { return _currentNode; }
		}




		public bool CanUndo()
		{
			return _currentNode != _rootNode;
		}

		public bool CanRedo()
		{
			return _currentNode.Children.Count > 0;
		}

		public void Undo()
		{
			if (!CanUndo())
				throw new InvalidOperationException("Cannot undo!");

			var curTran = _currentTransaction;
			_currentTransaction = null; // no transaction at moment of undo
			DestroyTransaction(curTran);

			var transitions = _currentNode.Transitions;
			for (int i = transitions.Count - 1; i >= 0; i--)
			{
				var oneTransition = transitions[i];
				oneTransition.ExecuteBackward();
			}

			_currentNode = _currentNode.Parent;

			CreateNewCurrentTransaction();
		}

		public void Redo(int historyIndex)
		{
			if (!CanRedo())
				throw new InvalidOperationException("Cannot redo!");

			var curTran = _currentTransaction;
			_currentTransaction = null; // no transaction at moment of redo
			DestroyTransaction(curTran);

			var nextNode = _currentNode.Children[historyIndex];
			foreach (var oneTransition in nextNode.Transitions)
			{
				oneTransition.ExecuteForward();
			}

			_currentNode = nextNode;

			CreateNewCurrentTransaction();
		}
	}

	public class HistoryTreeNode
	{
		public List<ITransition> Transitions { get; set; }
		public string Label { get; set; }

		public HistoryTreeNode()
		{
			Children = new List<HistoryTreeNode>();
		}
		public List<HistoryTreeNode> Children { get; private set; }
		public HistoryTreeNode Parent { get; set; }

		public bool IsLastNodeOfItsParent()
		{
			var parentNode = this.Parent;
			if (parentNode == null)
			{
				throw new InvalidOperationException("Cannot determine order of root node!");
			}
			return parentNode.Children.IndexOf(this) == parentNode.Children.Count - 1;
		}
	}

	public static class TransationFactory
	{
		public static TranScope CreateNewTransactionScope()
		{
			var microCommandProvider = new MicroCommandTransitionProvider();
			var tranScope = new TranScope(microCommandProvider);
			tranScope.Helper = new MicroCommandHelper(microCommandProvider);
			return tranScope;
		}
	}
}
