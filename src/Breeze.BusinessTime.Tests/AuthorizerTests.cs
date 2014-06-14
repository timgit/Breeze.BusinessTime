using System;
using System.Security.Principal;
using Breeze.BusinessTime.Authorization;
using Breeze.BusinessTime.Tests.Helpers;
using NUnit.Framework;

namespace Breeze.BusinessTime.Tests
{
    [TestFixture]
    public class AuthorizerTests
    {
        private Authorizer _authorizer;
        private string _adminRoleName;
        private IPrincipal _adminUser;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            _adminRoleName = PrincipalHelper.GetAdminRoleName();
            _adminUser = PrincipalHelper.CreateAdminPrincipal();
        }

        [SetUp]
        public void Setup()
        {
            _authorizer = new Authorizer();
        }

        [Test]
        public void EmptyAuthorizationAllows()
        {
            Assert.True(_authorizer.IsAuthorized());
        }

        [Test]
        public void RegisteredRoleIsAuthorized()
        {
            var user = PrincipalHelper.CreatePrincipal("user", "role1");
            _authorizer.Roles = "role1";

            Assert.True(_authorizer.IsAuthorized(user));
        }

        [Test]
        public void UnregisteredRoleIsNotAuthorized()
        {
            var user = PrincipalHelper.CreatePrincipal("user", "role1");
            _authorizer.Roles = "role99";

            Assert.False(_authorizer.IsAuthorized(user));
        }

        [Test]
        public void UserWithAtLeastOneMatchedRoleIsAuthorized()
        {
            var user = PrincipalHelper.CreatePrincipal("user", "role1", "role2", "role3");
            _authorizer.Roles = "role3";

            Assert.True(_authorizer.IsAuthorized(user));
        }

        [Test]
        public void RoleCanBeRegistered()
        {
            _authorizer.Roles = _adminRoleName;
            Assert.True(_authorizer.IsAuthorized(_adminUser));
        }

        [Test]
        public void UserCanBeRegistered()
        {
            _authorizer.Users = "bob";
            Assert.True(_authorizer.IsAuthorized("bob"));
        }

        /// <summary>
        /// Confirms that comma-delimited list of roles is working, including spaces
        /// </summary>
        [Test]
        public void MultipleRolesCanBeRegistered()
        {
            var memberRole = "member";
            var supervisorRole = "supervisor";

            var member = PrincipalHelper.CreatePrincipal(memberRole, memberRole);
            var supervisor = PrincipalHelper.CreatePrincipal(supervisorRole, supervisorRole);

            _authorizer.Roles = String.Format("{0}, {1}, {2}", memberRole, supervisorRole, _adminRoleName);

            Assert.True(_authorizer.IsAuthorized(_adminUser));
            Assert.True(_authorizer.IsAuthorized(supervisor));
            Assert.True(_authorizer.IsAuthorized(member));
        }

        /// <summary>
        /// Confirms that comma-delimited list of users is working, including spaces
        /// </summary>
        [Test]
        public void MultipleUsersCanBeRegistered()
        {
            var bob = "bob";
            var sally = "sally";
            var sue = "sue";

            _authorizer.Users = String.Format("{0}, {1}, {2}", bob, sally, sue);

            Assert.True(_authorizer.IsAuthorized(bob));
            Assert.True(_authorizer.IsAuthorized(sally));
            Assert.True(_authorizer.IsAuthorized(sue));
        }
    }
}
