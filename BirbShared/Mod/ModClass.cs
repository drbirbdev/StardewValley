using System;

namespace BirbShared.Mod
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiApi : Attribute
    {
        public string UniqueID;
        public bool IsRequired = true;
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiAsset : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiCommand : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiConfig : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiContent : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiData : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SmapiInstance : Attribute
    {

    }
}
