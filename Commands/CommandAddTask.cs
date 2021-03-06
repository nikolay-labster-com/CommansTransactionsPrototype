﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public class CommandAddTask : ICommand
	{
		private readonly Engine _engine;
		private readonly ITransactionProvider _transactionProvider;

		public CommandAddTask(Engine engine, ITransactionProvider transactionProvider)
		{
			_engine = engine;
			_transactionProvider = transactionProvider;
		}

		public bool CanBeExecutedOn(params object[] obj)
		{
			var engine = obj[0] as Mission;
			return engine != null;
		}

		public void InitiateExecution(params object[] obj)
		{
			if (!CanBeExecutedOn(obj))
			{
				throw new InvalidOperationException("Cannot be executed!");
			}

			var tran = _transactionProvider.GetCurrentTransaction();

			tran.CaptureMacroState("CommandAddTask");

			var mission = (Mission)obj[0];

			var task = new MissionTask();

			tran.Helper.AddItem(mission.Tasks, task);

			tran.Commit();
		}
	}
}
