using System;
using System.Windows.Markup;

/// <summary>
/// code from https://www.youtube.com/watch?v=Bp5LFXjwtQ0&ab_channel=BrianLagunas
/// </summary>
namespace StudyPlanner { 
	public class EnumBindingSourceExtension : MarkupExtension
	{
		public Type EnumType{get; private set;}
	
	
		public EnumBindingSourceExtension(Type enumType)
		{
			EnumType = enumType;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Enum.GetValues(EnumType);
		}
	}
}