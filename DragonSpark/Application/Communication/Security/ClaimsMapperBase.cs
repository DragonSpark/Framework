using DragonSpark.Extensions;
using System.Security.Claims;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace DragonSpark.Application.Communication.Security
{
    public abstract class ClaimsMapperBase<TUser> : IClaimsMapper where TUser : class, IUser
    {
        public virtual bool Matches( Claim claim, IUser user )
        {
            var result = claim.Type == TargetClaimType && TargetClaimValue.Transform( x => x == claim.Value, () => true ) && user.AsTo<TUser,bool>( ShouldBind );
            return result;
        }

        protected virtual bool ShouldBind( TUser user )
        {
            return user != null;
        }

        protected abstract void PerformMapping( Claim claim, TUser user );

        protected abstract string TargetClaimType { get; }

        protected virtual string TargetClaimValue
        {
            get { return null; }
        }

        void IClaimsMapper.Map( Claim claim, IUser user )
        {
            user.As<TUser>( x => PerformMapping( claim, x ) );
        }
    }
}