using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Talisman
{
	public class Stats
	{
		public int totalStr = 0;
		public int numberOfStrMonsters = 0;

		public int totalCraft = 0;
		public int numberOfCraftMonsters = 0;

		public int numberOfMonstersWithBothStats = 0;
		public int numberOfQuestionmarkMonsters = 0;

		public int countAnimal = 0;
		public int countMonster = 0;
		public int countDragon = 0;
		public int countConstruct = 0;
		public int countLaw = 0;
		public int countSpirit = 0;
		public int countCultist = 0;
		public int countElemental = 0;
		public int countGoblins = 0;

		public void Print()
		{
			Console.WriteLine("totalStr = {0}", totalStr);
			Console.WriteLine("numberOfStrMonsters = {0}", numberOfStrMonsters);
			Console.WriteLine("averageStrPerMob = {0}", (float)totalStr / (float)numberOfStrMonsters);
			Console.WriteLine();

			Console.WriteLine("totalCraft = {0}", totalCraft);
			Console.WriteLine("numberOfCraftMonsters = {0}", numberOfCraftMonsters);
			Console.WriteLine("averageCraftPerMob = {0}", (float)totalCraft / (float)numberOfCraftMonsters);
			Console.WriteLine();
			Console.WriteLine("numberOfMonstersWithBoth = {0}", numberOfMonstersWithBothStats);
			Console.WriteLine("numberOfQuestionmarkMonsters = {0}", numberOfQuestionmarkMonsters);

			Console.WriteLine();
			Console.WriteLine("countAnimal = {0}", countAnimal);
			Console.WriteLine("countMonster = {0}", countMonster);
			Console.WriteLine("countDragon = {0}", countDragon);
			Console.WriteLine("countConstruct = {0}", countConstruct);
			Console.WriteLine("countLaw = {0}", countLaw);
			Console.WriteLine("countSpirit = {0}", countSpirit);
			Console.WriteLine("countCultist = {0}", countCultist);
			Console.WriteLine("countElemental = {0}", countElemental);
			Console.WriteLine("countGoblins = {0}", countGoblins);
		}
	}

	class Program
	{
		static Stats stats = new Stats();
		
		static void Main(string[] args)
		{
			var dic = new Dictionary<string, string>();
			var gobLst = new List<string>();

			using(StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\talisman.json"))
			{
				string json = reader.ReadToEnd();
				dynamic data = JObject.Parse(json);
				dynamic monsters = data["Monsters"];

				foreach(var expansion in monsters)
				{
					foreach(var mob in expansion)
					{
						foreach(var item in mob)
						{
							if(item.Name.ToString().ToLower().Contains("goblin"))
							{
								stats.countGoblins++;
								gobLst.Add(item.Name.ToString());
							}

							if(item.Description != "")
								dic.Add(item.Name.ToString(), item.Description.ToString());

							if(item.strength == "?" || item.craft == "?")
							{
								stats.numberOfQuestionmarkMonsters++;
								continue;
							}
							int str = int.Parse(item.strength.ToString());
							int craft = int.Parse(item.craft.ToString());

							CountType(item.EnemyType.ToString());

							if(str > 0 && craft > 0)
							{
								stats.numberOfMonstersWithBothStats++;
								stats.totalStr += str;
								stats.totalCraft += craft;
								continue;
							}
							if(str > 0)
							{
								stats.numberOfStrMonsters++;
								stats.totalStr += str;
							}
							if(craft > 0)
							{
								stats.numberOfCraftMonsters++;
								stats.totalCraft += craft;
							}
						}
					}
				}
				Console.WriteLine();
				stats.Print();

				Console.WriteLine();
				if(gobLst.Count > 0)
				{
					Console.WriteLine("Monsters with goblins in the name: {0}\npress 'y' to see them!", gobLst.Count);
					if(Console.ReadKey().KeyChar == 'y')
					{
						Console.WriteLine();
						foreach(var item in gobLst)
						{
							Console.WriteLine(item);
						}
					}
				}

				Console.WriteLine();
				Console.WriteLine("Monsters with special conditions: {0}\npress 'y' to see them!", dic.Count);
				if(Console.ReadKey().KeyChar == 'y')
				{
					Console.WriteLine();
					foreach(var item in dic)
					{
						Console.WriteLine();
						Console.WriteLine(item);
					}
				}
			}
			Console.ReadLine();
		}

		public static void CountType(string item)
		{
			switch(item)
			{
				case "Animal":
					stats.countAnimal++;
					break;
				case "Dragon":
					stats.countDragon++;
					break;
				case "Monster":
					stats.countMonster++;
					break;
				case "Construct":
					stats.countConstruct++;
					break;
				case "Law":
					stats.countLaw++;
					break;
				case "Spirit":
					stats.countSpirit++;
					break;
				case "Cultist":
					stats.countCultist++;
					break;
				case "Elemental":
					stats.countElemental++;
					break;
				default:
					throw new Exception("No such enemy type as " + item);
			}
		}
	}
}
//SAMPLE MONSTER
//{
//	"Name": "Wild Boar",
//	"EnemyType": "Animal",
//	"strength": "1",
//	"craft": "0",
//	"initiative": "2",
//	"Description": ""
//}