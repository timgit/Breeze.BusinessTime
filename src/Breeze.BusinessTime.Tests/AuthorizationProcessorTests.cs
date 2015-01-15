using System;
using System.Collections.Generic;
using System.Security.Principal;
using Breeze.BusinessTime.Authorization;
using Breeze.BusinessTime.Tests.Helpers;
using Breeze.ContextProvider;
using NUnit.Framework;

namespace Breeze.BusinessTime.Tests
{
    [TestFixture]
    public class AuthorizationProcessorTests
    {
        private IPrincipal _adminBob;
        private IPrincipal _goldMember;
        private IPrincipal _nullPrincipal;

        private string _adminRole;

        private Dictionary<Type, List<EntityInfo>> _entitySaveMap;
        private RegistryAuthorizationProvider _authorizationRegistry;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _adminRole = PrincipalHelper.GetAdminRoleName();
            _nullPrincipal = PrincipalHelper.CreatePrincipal();
            _adminBob = PrincipalHelper.CreateAdminPrincipal("adminBob");
            _goldMember = PrincipalHelper.CreatePrincipal("goldMember", "member");

            _entitySaveMap = new Dictionary<Type, List<EntityInfo>>();
            _authorizationRegistry = new RegistryAuthorizationProvider();
        }

        [SetUp]
        public void Setup()
        {
            _entitySaveMap.Add(typeof(Object), new List<EntityInfo> { new FakeEntityInfo { EntityState = EntityState.Added }});
            _authorizationRegistry.Register<Object>(_adminRole);
        }

        [TearDown]
        public void TearDown()
        {
            _entitySaveMap.Clear();
            _authorizationRegistry.Registry.Clear();            
        }

        [Test]
        public void MissingEntityRegistrationDenies()
        {
            _entitySaveMap.Add(typeof(FakeEntity), new List<EntityInfo> { new FakeEntityInfo { EntityState = EntityState.Added } });

            var processor = CreateAuthorizationProcesser(_adminBob);

            Assert.Throws<EntityErrorsException>(() => processor.Process(_entitySaveMap), "Unregistered entity type should be denied.");
        }

        [Test]
        public void AllowedProcessorShouldAlwaysSucceed()
        {
            var processor = new AuthorizationProcesser(_nullPrincipal, new AllowedAuthorizer());

            Assert.DoesNotThrow(() => processor.Process(_entitySaveMap), "Allowed Processor didn't allow.");
        }

        [Test]
        public void DeniedProcessorShouldAlwaysDeny()
        {
            var processor = new AuthorizationProcesser(_nullPrincipal, new DeniedAuthorizer());

            Assert.Throws<EntityErrorsException>(() => processor.Process(_entitySaveMap), "Denied Processor didn't deny.");
        }

        [Test]
        public void WhitelistMembershipShouldBypassAuthorization()
        {
            var processor = new AuthorizationProcesser(_adminBob, new DeniedAuthorizer(), _adminRole);

            Assert.DoesNotThrow(() => processor.Process(_entitySaveMap), "Whitelisted user should have bypassed authorization check.");
        }

        [Test]
        public void UnauthorizedSaveAttemptShouldBeDenied()
        {
            var processor = CreateAuthorizationProcesser(_goldMember);
            Assert.Throws<EntityErrorsException>(() => processor.Process(_entitySaveMap),
                "Processor allowed an unauthorized action.");
        }

        [Test]
        public void AuthorizedSaveAttemptShouldSucceed()
        {
            var processor = CreateAuthorizationProcesser(_adminBob);
            Assert.DoesNotThrow(() => processor.Process(_entitySaveMap),
                "Processor denied an authorized action.");

        }

        private IProcessBreezeRequests CreateAuthorizationProcesser(IPrincipal principal, params string[] allowedRoles)
        {
            return new AuthorizationProcesser(principal, _authorizationRegistry, allowedRoles);
        }
    }
}
