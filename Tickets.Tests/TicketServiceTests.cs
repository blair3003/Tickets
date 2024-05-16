using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Tests
{
    public class TicketServiceTests : IClassFixture<TestEnvironment>
    {
        private readonly TestEnvironment _env;

        public TicketServiceTests(TestEnvironment env)
        {
            _env = env;
        }
    }
}
