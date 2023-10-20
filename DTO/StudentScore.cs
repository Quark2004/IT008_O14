using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
	public class StudentScore
	{
		string subId;
		string subName;
		int numOfCredits;
		double score1;
		double score2;
		double score3;
		double score4;

		public string SubId { get => subId; set => subId = value; }
		public string SubName { get => subName; set => subName = value; }
		public int NumOfCredits { get => numOfCredits; set => numOfCredits = value; }
		public double Score1 { get => score1; set => score1 = value; }
		public double Score2 { get => score2; set => score2 = value; }
		public double Score3 { get => score3; set => score3 = value; }
		public double Score4 { get => score4; set => score4 = value; }
	}


}
