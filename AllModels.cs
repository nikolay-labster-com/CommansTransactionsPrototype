using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandTransactionsPrototype
{
	public class Engine
	{
		public Engine()
		{
			Missions = new List<Mission>();
			Conversations = new List<Conversation>();
		}
		public List<Mission> Missions { get; set; }
		public List<Conversation> Conversations { get; set; }
	}

	public class Conversation
	{
		private static int _seqId = 1;

		public Conversation()
		{
			this.Id = _seqId.ToString();
			_seqId++;
		}

		public string Id { get; set; }

		public override string ToString()
		{
			return string.Format("conversation {0}", Id);
		}

	}

	public class Mission
	{
		private static int _seqId = 1;

		public Mission()
		{
			Tasks = new List<MissionTask>();
			this.Id = _seqId.ToString();
			_seqId++;
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<MissionTask> Tasks { get; set; }

		public override string ToString()
		{
			return string.Format("mission {0} ({1})", Id, Name);
		}
	}

	public class MissionTask
	{
		private static int _seqId = 1;

		public MissionTask()
		{
			this.Id = _seqId.ToString();
			_seqId++;
		}

		public string Id { get; set; }

		public override string ToString()
		{
			return string.Format("task {0}", Id);
		}

	}

}
