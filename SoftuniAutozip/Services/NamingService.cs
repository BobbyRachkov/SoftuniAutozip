using System;
using SoftuniAutozip.Interfaces;

namespace SoftuniAutozip.Services
{
    public class NamingService:INameResolver
    {
        public string NewName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
