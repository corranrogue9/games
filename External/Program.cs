namespace ConsoleApplication2
{
    using System;

    public static class Foo
    {
        public static void Main()
        {
            var left2 = Value
                .Create(1)
                .Prepend(3)
                .Prepend(4)
                .Prepend(-1);
            var left = Value
                .Create(1)
                .Append(3)
                .Append(4)
                .Append(-1);
            var right = Value
                .Create("asdf")
                .Append("qwer")
                .Append("1234")
                .Append("zxcv");
            var right2 = Value
                .Create(-1)
                .Append(4)
                .Append(3)
                .Append(1);
            var right3 = Value
                .Create(-1)
                .Append(4)
                .Append(3)
                .Append(1)
                .Append(1);
            var zipped = left.Zip(right2);

            var zipped2 = left.Zip2(right);

            var zipped3 = zipped2.Zip2(right2);
            Console.WriteLine(zipped3.Structure.Count);
            Console.WriteLine(right3.Structure.Count);
        }
    }
}