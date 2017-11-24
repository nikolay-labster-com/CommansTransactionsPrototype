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

		public bool CanBeExecutedOn(params object[] obj)
		{
			return _history.CanUndo();
		}

		public void InitiateExecution(params object[] obj)
		{
			if (CanBeExecutedOn(obj))
			{
				_history.Undo();
			}
		}
	}
}
