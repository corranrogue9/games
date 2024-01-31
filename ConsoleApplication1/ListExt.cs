using System.IO.Compression;
using System.Net.Http.Headers;

namespace ConsoleApplication2
{
    public static class ListExt
    {
        public static void DoWork()
        {
            var left = Value
                .Create(1)
                .Add(3)
                .Add(4)
                .Add(-1);
            var right = Value
                .Create("asdf")
                .Add("qwer")
                .Add("1234")
                .Add("zxcv");
            //// TODO uncomment block comments and get ti implemented
            var zipped = left.Zip(right);
        }

        /*public static ValueNode<(TValueLeft, TValueRight), Leaf> Zip<TValueLeft, TValueRight>(this ValueLeaf<TValueLeft> left, ValueLeaf<TValueRight> right)
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that
            return Value.Create((left.Value, right.Value));
        }*/

        public static ValueNode<(TValueLeft, TValueRight), TStructure> Zip<TValueLeft, TValueRight, TStructure>(
            this ValueNode<TValueLeft, TStructure> left,
            ValueNode<TValueRight, TStructure> right)
            where TStructure : Node
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that

            using (var leftEnumerator = left.ToStructureless().GetEnumerator())
            using (var rightEnumerator = right.ToStructureless().GetEnumerator())
            {
                if (!(leftEnumerator.MoveNext() && rightEnumerator.MoveNext()))
                {
                    throw new InvalidOperationException("TOdO");
                }

                var leaf = Value.Create((leftEnumerator.Current, rightEnumerator.Current));
                leaf.Add((leftEnumerator.Current, rightEnumerator.Current));
            }
        }
    }

    public abstract class Node
    {
    }

    public sealed class Leaf : Node
    {
    }

    public sealed class Inner<TNode> : Node where TNode : Node
    {
        public Inner(TNode node)
        {
            this.Node = node;
        }

        public TNode Node { get; }
    }

    public static class Value
    {
        public static ValueLeaf<TValue> Create<TValue>(TValue value)
        {
            return new ValueLeaf<TValue>(value);
        }

        public static ValueInner<TValue, Leaf, ValueLeaf<TValue>> Add<TValue>(this ValueLeaf<TValue> leaf, TValue value)
        {
            return new ValueInner<TValue, Leaf, ValueLeaf<TValue>>(value, leaf, new Inner<Leaf>(leaf.Structure));
        }

        public static ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNodeStart>> Add<TValue, TStructure, TValueNodeStart>(
            this ValueInner<TValue, TStructure, TValueNodeStart> inner,
            TValue value)
            where TStructure : Node
            where TValueNodeStart : ValueNode<TValue, TStructure>
        {
            return new ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNodeStart>>(
                value, 
                inner, 
                new Inner<Inner<TStructure>>(inner.Structure));
        }
    }

    public abstract class ValueNode<TValue, TStructure> where TStructure : Node
    {
        public abstract TValue Value { get; }

        public abstract TStructure Structure { get; }

        public abstract IEnumerable<TValue> ToStructureless();
    }

    public sealed class ValueLeaf<TValue> : ValueNode<TValue, Leaf>
    {
        public ValueLeaf(TValue value)
        {
            this.Value = value;
            this.Structure = new Leaf();
        }

        public override TValue Value { get; }

        public override Leaf Structure { get; }

        public override IEnumerable<TValue> ToStructureless()
        {
            yield return this.Value;
        }
    }

    public sealed class ValueInner<TValue, TStructure, TValueNode> : ValueNode<TValue, Inner<TStructure>> where TValueNode : ValueNode<TValue, TStructure> where TStructure : Node
    {
        public ValueInner(TValue value, TValueNode node, Inner<TStructure> structure)
        {
            this.Value = value;
            this.Node2 = node;
            this.Structure = structure;
        }

        public override TValue Value { get; }

        public override Inner<TStructure> Structure { get; }

        public TValueNode Node2 { get; }

        public override IEnumerable<TValue> ToStructureless()
        {
            yield return this.Value;
            foreach (var element in this.Node2.ToStructureless())
            {
                yield return element;
            }
        }
    }

    public abstract class StructurelessNode<TValue>
    {
        public abstract TValue Value { get; }
    }

    public sealed class StructurelessLeaf<TValue> : StructurelessNode<TValue>
    {
        public StructurelessLeaf(TValue value)
        {
            this.Value = value;
        }

        public override TValue Value { get; }
    }

    public sealed class StructurelessInner<TValue, TNode> : StructurelessNode<TValue>
        where TNode : StructurelessNode<TValue>
    {
        public StructurelessInner(TValue value, TNode node)
        {
            this.Value = value;
            this.Node = node;
        }

        public override TValue Value { get; }

        public TNode Node { get; }
    }
}
