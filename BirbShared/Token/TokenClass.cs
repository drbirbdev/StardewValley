using System;

namespace BirbShared.Token
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TokenClass : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TokenMethod : Attribute
    {
    }
}
