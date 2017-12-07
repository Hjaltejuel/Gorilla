
using Gorilla.AuthenticationGorillaAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UI.Test
{
    public class AuthenticationTest
    {
        
        private readonly IAuthenticationHelper _helper;
        public AuthenticationTest(IAuthenticationHelper helper)
        {
            _helper = helper;
        }

        [Fact]
        public async Task Authenticate()
        {
            var _account = await _helper.SignInAsync();
            Assert.NotNull(_account);
        }
        
        [Fact]
        public void TestAll()
        {
            var d = 5;
            Assert.Equal(5,d);
        }
    }
}
