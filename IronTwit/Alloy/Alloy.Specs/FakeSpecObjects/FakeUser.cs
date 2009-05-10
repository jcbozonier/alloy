using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unite.Messaging;
using Unite.Messaging.Entities;

namespace Unite.Specs.FakeSpecObjects
{
    public class FakeUser : IIdentity
    {
        public string UserName
        {
            get;
            set;
        }

        public ServiceInformation ServiceInfo
        {
            get;
            set;
        }
    }
}
