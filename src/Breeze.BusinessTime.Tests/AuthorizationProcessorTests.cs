using System;
using System.Collections.Generic;
using System.Security.Principal;
using Breeze.BusinessTime.Authorization;
using Breeze.ContextProvider;
using NUnit.Framework;

namespace Breeze.BusinessTime.Tests
{
    [TestFixture]
    public class AuthorizationProcessorTests
    {
        private IPrincipal _adminBob;
        //private IPrincipal _superRoy;
        private IPrincipal _goldMember;
        private IPrincipal _nullPrincipal;

        private const string AdminRole = "admin";
        //private const string SupervisorRole = "supervisor";
        private const string MemberRole = "goldmember";

        private Dictionary<Type, List<EntityInfo>> _entitySaveMap;
        private RegistryAuthorizationProvider _authorizationRegistry;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _nullPrincipal = createPrincipal();
            _adminBob = createPrincipal("adminBob", new[] {AdminRole});
            //_superRoy = createPrincipal("superRoy", new[] {SupervisorRole});
            _goldMember = createPrincipal("goldMember", new[] {MemberRole});

            _entitySaveMap = new Dictionary<Type, List<EntityInfo>>
            {
                {
                    typeof (Object), new List<EntityInfo>
                    {
                        new FakeEntityInfo
                        {
                            EntityState = EntityState.Added
                        }
                    }
                }
            };

            _authorizationRegistry = RegistryAuthorizationProvider
                .Create()
                .Register<Object>(AdminRole);
        }

        //[SetUp]
        //public void TestSetup()
        //{
        //}

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
            var processor = new AuthorizationProcesser(_adminBob, new DeniedAuthorizer(), AdminRole);

            Assert.DoesNotThrow(() => processor.Process(_entitySaveMap), "Whitelisted user should have bypassed authorization check.");
        }

        [Test]
        public void UnauthorizedSaveAttemptShouldBeDenied()
        {
            var processor = new AuthorizationProcesser(_goldMember, _authorizationRegistry);
            Assert.Throws<EntityErrorsException>(() => processor.Process(_entitySaveMap),
                "Processor allowed an unauthorized action.");
        }

        [Test]
        public void AuthorizedSaveAttemptShouldSucceed()
        {
            var processor = new AuthorizationProcesser(_adminBob, _authorizationRegistry);
            Assert.DoesNotThrow(() => processor.Process(_entitySaveMap),
                "Processor denied an authorized action.");

        }

        private IPrincipal createPrincipal(string userName = "", string[] roles = null)
        {
            return new GenericPrincipal(new GenericIdentity(userName), roles);
        }
    }
}
