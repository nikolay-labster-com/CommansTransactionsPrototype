using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandUndo : ICommand
	{
		private readonly IUndoRedoManager _history;

		public CommandUndo(IUndoRedoManager history)
		{
			_history = history;
		}

		public bool CanBeExecutedNow
		{
			get { return _history.CanUndo(); }
		}

		public void InitiateExecution()
		{
			_history.Undo();
		}
	}
}
