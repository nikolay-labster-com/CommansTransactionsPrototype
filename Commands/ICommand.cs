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
		// it seems instead of obj we may have several string/int parameters in real app
		bool CanBeExecutedOn(params object[] obj);
		void InitiateExecution(params object[] obj);
	}
}
