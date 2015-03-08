using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class UriMappingContainerBehavior : RegionBehaviorBase<Frame>
	{
		public class UriMapper : UriMapperBase
		{
			readonly System.Windows.Navigation.UriMapper mapper = new System.Windows.Navigation.UriMapper();
			readonly IList<UriMappingResult> results = new List<UriMappingResult>();

			readonly ReadOnlyCollection<UriMappingResult> history;

			public UriMapper()
			{
				history = new ReadOnlyCollection<UriMappingResult>( results );
			}
			public Collection<UriMapping> UriMappings
			{
				get { return mapper.UriMappings; }
			}

			public override Uri MapUri( Uri uri )
			{
				var result = mapper.MapUri( uri );
				var mappingResult = new UriMappingResult( uri, result );
				if ( mappingResult != results.LastOrDefault() )
				{
					results.Add( mappingResult );
				}
				return result;
			}

			public IEnumerable<UriMappingResult> History
			{
				get { return history; }
			}

			public class UriMappingResult
			{
				readonly Uri to;
				readonly Uri from;

				public UriMappingResult( Uri from, Uri to )
				{
					this.from = from;
					this.to = to;
				}

				public Uri To
				{
					get { return to; }
				}

				public Uri From
				{
					get { return from; }
				}

				public bool Equals( UriMappingResult other )
				{
					if ( ReferenceEquals( null, other ) )
					{
						return false;
					}
					if ( ReferenceEquals( this, other ) )
					{
						return true;
					}
					return Equals( other.@from, @from ) && Equals( other.to, to );
				}

				public override bool Equals( object obj )
				{
					if ( ReferenceEquals( null, obj ) )
					{
						return false;
					}
					if ( ReferenceEquals( this, obj ) )
					{
						return true;
					}
					if ( obj.GetType() != typeof(UriMappingResult) )
					{
						return false;
					}
					return Equals( (UriMappingResult)obj );
				}

				public override int GetHashCode()
				{
					unchecked
					{
						return ( ( @from != null ? @from.GetHashCode() : 0 ) * 397 ) ^ ( to != null ? to.GetHashCode() : 0 );
					}
				}
			}
		}

		public bool Contains( Uri uri )
		{
			var result = Mapper.UriMappings.Any( x => x.Uri == uri || x.MappedUri == uri || Mapper.History.LastOrDefault().Transform( y => x.MapUri( y.From ) == uri ) );
			return result;
		}
		
		public UriMapper Mapper { get; private set; }

		protected override void OnAttach()
		{
			base.OnAttach();
			Mapper = AssociatedControl.UriMapper.As<UriMapper>() ?? new UriMapper();
			AssociatedControl.UriMapper = Mapper;
		}
	}
}