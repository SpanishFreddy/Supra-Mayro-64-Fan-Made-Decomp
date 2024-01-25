using System;
using System.Linq;
using System.Reflection;
using EnhancedTriggerbox.Component;

namespace EnhancedTriggerbox
{
	public class ComponentList
	{
		public string[] conditionNames;

		public string[] responseNames;

		private static ComponentList instance;

		public static ComponentList Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ComponentList();
				}
				return instance;
			}
		}

		private ComponentList()
		{
			string[] array = (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.GlobalAssemblyCache
				select a into domainAssembly
				from assemblyType in domainAssembly.GetTypes()
				where typeof(ConditionComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ConditionComponent"
				select EnhancedTriggerBox.AddSpacesToSentence(assemblyType.Name, true)).ToArray();
			string[] array2 = new string[array.Length + 1];
			array.CopyTo(array2, 1);
			array2[0] = "Select A Condition";
			conditionNames = array2;
			array = (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.GlobalAssemblyCache
				select a into domainAssembly
				from assemblyType in domainAssembly.GetTypes()
				where typeof(ResponseComponent).IsAssignableFrom(assemblyType) && assemblyType.Name != "ResponseComponent"
				select EnhancedTriggerBox.AddSpacesToSentence(assemblyType.Name, true)).ToArray();
			array2 = new string[array.Length + 1];
			array.CopyTo(array2, 1);
			array2[0] = "Select A Response";
			responseNames = array2;
		}
	}
}
