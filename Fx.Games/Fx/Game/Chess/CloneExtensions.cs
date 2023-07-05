

public static class CloneExtensions
{
    public static void DoWork()
    {
        new Foo().Clone2();
        new Bar().Clone2();
    }

    private class Foo : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    private struct Bar : ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public static T Clone2<T>(this T toClone) where T : class, ICloneable
    {
        return toClone.Clone() as T;
    }

    public static T Clone2<T>(this T toClone, [System.Runtime.CompilerServices.CallerMemberName] string caller = null) where T : struct, ICloneable
    {
        return (T)toClone.Clone();
    }
}

