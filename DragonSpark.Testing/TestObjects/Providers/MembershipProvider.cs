using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.TestObjects.Providers
{
	class MembershipProvider : System.Web.Security.MembershipProvider
	{
		readonly List<MembershipUser> users = new List<MembershipUser>();

		public override MembershipUser CreateUser( string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status )
		{
			var result = new MembershipUser( "DragonSparkProvider", username, providerUserKey, email, passwordQuestion,
			                                 null, isApproved, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
			                                 DateTime.MinValue );
			users.Add( result );
			status = MembershipCreateStatus.Success;
			return result;
		}


		public override bool ChangePasswordQuestionAndAnswer( string username, string password, string newPasswordQuestion, string newPasswordAnswer )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override string GetPassword( string username, string answer )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override bool ChangePassword( string username, string oldPassword, string newPassword )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override string ResetPassword( string username, string answer )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override void UpdateUser( MembershipUser user )
		{
			DeleteUser( user.UserName, true );
			users.Add( user );
		}

		public override bool ValidateUser( string username, string password )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override bool UnlockUser( string userName )
		{
			throw new InvalidOperationException( "Not a chance." );
		}

		public override MembershipUser GetUser( object providerUserKey, bool userIsOnline )
		{
			var result = users.Find( item => item.ProviderUserKey == providerUserKey );
			return result;
		}

		public override MembershipUser GetUser( string username, bool userIsOnline )
		{
			var result = users.Find( item => item.UserName == username );
			return result;
		}

		public override string GetUserNameByEmail( string email )
		{
			var result = users.Find( item => item.Email == email );
			return result.UserName;
		}

		public override bool DeleteUser( string username, bool deleteAllRelatedData )
		{
			users.Find( item => item.UserName == username ).NotNull( item => users.Remove( item ) );
			return true;
		}

		public override MembershipUserCollection GetAllUsers( int pageIndex, int pageSize, out int totalRecords )
		{
			var result = new MembershipUserCollection();
			var source = users.Skip( pageIndex * pageSize ).Take( pageSize );
			source.Apply( result.Add );
			totalRecords = users.Count;
			return result;
		}

		public override int GetNumberOfUsersOnline()
		{
			return 0;
		}

		public override MembershipUserCollection FindUsersByName( string usernameToMatch, int pageIndex, int pageSize, out int totalRecords )
		{
			var result = new MembershipUserCollection();
			var query = from user in users where user.UserName == usernameToMatch select user;
			var source = query.Skip( pageIndex * pageSize ).Take( pageSize );
			source.Apply( result.Add );
			totalRecords = users.Count;
			return result;
		}

		public override MembershipUserCollection FindUsersByEmail( string emailToMatch, int pageIndex, int pageSize, out int totalRecords )
		{
			var result = new MembershipUserCollection();
			var query = from user in users where user.Email == emailToMatch select user;
			var source = query.Skip( pageIndex * pageSize ).Take( pageSize );
			source.Apply( result.Add );
			totalRecords = users.Count;
			return result;
		}

		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}

		public override bool EnablePasswordReset
		{
			get { return false; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return false; }
		}

		public override string ApplicationName
		{
			get { return "DragonSpark Framework"; }
			set {  }
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { return 0; }
		}

		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		public override int MinRequiredPasswordLength
		{
			get { return 1; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return 1; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return null; }
		}
	}
}
