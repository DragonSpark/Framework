using DragonSpark.Extensions;
using System.Security.Claims;

namespace DragonSpark.Security
{
    public abstract class ClaimsMapperBase<TUser> : IClaimsMapper where TUser : UserProfile
    {
        public virtual bool Matches( Claim claim, UserProfile user )
        {
            var result = claim.Type == TargetClaimType && TargetClaimValue.Transform( x => x == claim.Value, () => true ) && user.AsTo<TUser,bool>( ShouldBind );
            return result;
        }

        protected virtual bool ShouldBind( TUser user )
        {
            return user != null;
        }

        protected abstract void PerformMapping( Claim claim, TUser userProfile );

        protected abstract string TargetClaimType { get; }

        protected virtual string TargetClaimValue
        {
            get { return null; }
        }

        void IClaimsMapper.Map( Claim claim, UserProfile user )
        {
            user.As<TUser>( x => PerformMapping( claim, x ) );
        }
    }
}