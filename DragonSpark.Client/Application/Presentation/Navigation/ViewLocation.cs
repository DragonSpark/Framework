using System;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Infrastructure;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class ViewLocation : ViewObject, IRegionContract
	{
		public string Title
		{
			get { return title; }
			set { SetProperty( ref title, value, () => Title ); }
		}	string title;

		public Uri Icon
		{
			get { return icon; }
			set { SetProperty( ref icon, value, () => Icon ); }
		}	Uri icon;

		public Uri Location
		{
			get { return location; }
			set { SetProperty( ref location, value, () => Location ); }
		}	Uri location;

		public string ContractName
		{
			get { return contractName; }
			set { SetProperty( ref contractName, value, () => ContractName ); }
		}	string contractName;

		public string TargetName
		{
			get { return targetName; }
			set { SetProperty( ref targetName, value, () => TargetName ); }
		}	string targetName;
	}
}