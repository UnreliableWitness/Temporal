using System;

namespace Temporal.Core.Exceptions
{
    [Serializable]
    public class ConventionAlreadyRegisteredException : Exception
    {
        private readonly Type _type;
        public ConventionAlreadyRegisteredException(Type type)
        {
            _type = type;
        }

        public override string Message
        {
            get { return string.Format("A convention of type {0} is already registered.", _type.FullName); }
        }
    }
}
