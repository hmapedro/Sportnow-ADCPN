using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace SportNow.Model.Charts
{
	public class Attendance_Stats
	{

		public List<Attendance_Stat> Data { get; set; }


		public Attendance_Stats(List<Class_Schedule> class_schedules)
		{
			Data = new List<Attendance_Stat>();
			Attendance_Stat attendance_stat_all = new Attendance_Stat("Todas");
			Data.Add(attendance_stat_all);
			foreach (Class_Schedule class_schedule in class_schedules)
			{
				Attendance_Stat attendance_stat = checkTypeExists(class_schedule.name);
				if (class_schedule.classattendancestatus == "fechada")
				{
					attendance_stat.class_count_presente++;
					attendance_stat_all.class_count_presente++;
				}
				else
				{
					attendance_stat.class_count_ausente++;
					attendance_stat_all.class_count_ausente++;
				}
				attendance_stat.class_count_total++;
				attendance_stat_all.class_count_total++;
			}

			foreach (Attendance_Stat attendance_stat in Data)
			{
				attendance_stat.attendance_percentage = (attendance_stat.class_count_presente / attendance_stat.class_count_total)*100;
			}
			this.Print();
			//return competition_results;

		}

		public Attendance_Stat checkTypeExists(string class_type)
		{
			for (int i=0; i< Data.Count; i++)
			{
				Attendance_Stat attendance_Stat = Data[i];
				//Debug.Print("CheckExists compare " + attendance_Stat.name + " " + class_type);
				if (attendance_Stat.name==class_type) 
				{
					//Debug.Print("CheckExists true ");
					return attendance_Stat;
				}
			}
			Attendance_Stat attendance_stat = new Attendance_Stat(class_type);
			Data.Add(attendance_stat);
			return attendance_stat;

		}


		public void Print()
		{
			foreach (Attendance_Stat attendance_stat in Data)
			{
				Debug.Print("AQUII2 " + attendance_stat.name + " " + attendance_stat.class_count_presente+" " + attendance_stat.attendance_percentage);
			}


		}
	}


}
