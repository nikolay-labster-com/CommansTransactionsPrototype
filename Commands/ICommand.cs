using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandTransactionsPrototype.Transactions;

namespace CommandTransactionsPrototype.Commands
{
	public interface ICommand
	{
		bool CanBeExecutedNow { get; }
		void InitiateExecution();
		//bool CanBeExecutedOn(object obj);
	}
}
