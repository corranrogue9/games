namespace ConsoleApplication2
{
    using System;

    public static class Foo
    {
        public static void Main()
        {
            //// TODO do you actually need to expose valueinner and valueleaf? the return types of the zips are just the valuenode base type
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

            var selected = zipped3.Select(tuple => Tuple.Create(tuple.Item1.Item1, tuple.Item1.Item2, tuple.Item2));
            Console.WriteLine(zipped3.Structure.Count);
            Console.WriteLine(right3.Structure.Count);
        }
    }
}