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
            var right2 = Value
                .Create(-1)
                .Add(4)
                .Add(3)
                .Add(1);
            //// TODO this doesn't quite work, but it's super close
            //// TODO even if you get it working, you're still missing the actual tuple part because right now it's required that the left and right are the same type
            var zipped = left.Zip(right2);

            var zipped2 = left.Zip2(right);
        }

        /*public static ValueNode<(TValueLeft, TValueRight), Leaf> Zip<TValueLeft, TValueRight>(this ValueLeaf<TValueLeft> left, ValueLeaf<TValueRight> right)
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that
            return Value.Create((left.Value, right.Value));
        }*/

        /*public static ValueInner<(TValueLeft, TValueRight), Inner<TStructureTheRest>, ValueNode<(TValueLeft, TValueRight), Inner<TStructureTheRest>>> Zip<
            TValueLeft,
            TValueRight, 
            TStructureTheRest,
            TValueNodeLeft,
            TValueNodeRight>(
            this ValueInner<TValueLeft, Inner<TStructureTheRest>, TValueNodeLeft> left,
            ValueInner<TValueRight, Inner<TStructureTheRest>, TValueNodeRight> right)
            where TStructureTheRest : Node
            where TValueNodeLeft : ValueNode<TValueLeft, Inner<TStructureTheRest>>
            where TValueNodeRight : ValueNode<TValueRight, Inner<TStructureTheRest>>
        {
            //// TODO in your node and value definitions you are certainly not having gauranteed type safety; please fix that

            var leftNode = left.Node2 as ValueInner<TValueLeft, TStructureTheRest, ValueNode<TValueLeft, TStructureTheRest>>;

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

            return null;
        }*/
    }

    public abstract class Node
    {
        public abstract Node? Remainder { get; }
    }

    public sealed class Leaf : Node
    {
        public override Node? Remainder
        {
            get
            {
                return null;
            }
        }
    }

    public sealed class Inner<TNode> : Node where TNode : Node
    {
        public Inner(TNode node)
        {
            this.Node = node;
        }

        public override Node? Remainder
        {
            get
            {
                return this.Node;
            }
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

        public abstract ValueNode<(TValue, TValue), TStructure> Zip(ValueNode<TValue, TStructure> right);

        public abstract ValueNode<(TValue, TValueRight), TStructure> Zip2<TValueRight>(ValueNode<TValueRight, TStructure> right);

        internal abstract ValueNode<(TValue, TValueRight), TStructure2> Zip3<TValueRight, TStructure2>(ValueNode<TValueRight, TStructure2> right, TStructure2 structure)
            where TStructure2 : Node;

        internal abstract ValueNode<TValue2, TStructure3>? Node3<TValue2, TStructure3>()
            where TStructure3 : Node;

        internal abstract ValueNode<TValue3, TStructure4>? Node4<TValue3, TStructure4>(TStructure4 structure)
            where TStructure4 : Node;
    }

    public sealed class ValueLeaf<TValue> : ValueNode<TValue, Leaf>
    {
        public ValueLeaf(TValue value)
        {
            this.Value = value;
            this.Structure = new Leaf();
        }

        public override ValueNode<(TValue, TValue), Leaf> Zip(ValueNode<TValue, Leaf> right)
        {
            return new ValueLeaf<(TValue, TValue)>((this.Value, right.Value));
        }

        public override ValueNode<(TValue, TValueRight), Leaf> Zip2<TValueRight>(ValueNode<TValueRight, Leaf> right)
        {
            return new ValueLeaf<(TValue, TValueRight)>((this.Value, right.Value));
        }

        internal override ValueNode<(TValue, TValueRight), TStructure2> Zip3<TValueRight, TStructure2>(ValueNode<TValueRight, TStructure2> right, TStructure2 structure)
        {
            return new ValueLeaf<(TValue, TValueRight)>((this.Value, right.Value)) as ValueNode<(TValue, TValueRight), TStructure2>;
        }

        internal override ValueNode<TValue2, TStructure3>? Node3<TValue2, TStructure3>()
        {
            return null;
        }

        internal override ValueNode<TValue3, TStructure4>? Node4<TValue3, TStructure4>(TStructure4 structure)
        {
            return null;
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

        public override ValueNode<(TValue, TValue), Inner<TStructure>> Zip(ValueNode<TValue, Inner<TStructure>> right)
        {
            return new ValueInner<(TValue, TValue), TStructure, ValueNode<(TValue, TValue), TStructure>>(
                (this.Value, right.Value),
                this.Node2.Zip((right as ValueInner<TValue, TStructure, TValueNode>).Node2),
                null);
        }

        public override ValueNode<(TValue, TValueRight), Inner<TStructure>> Zip2<TValueRight>(ValueNode<TValueRight, Inner<TStructure>> right)
        {
            ValueNode<TValueRight, TStructure> rightSubNode;
            if (typeof(TStructure) == typeof(Leaf))
            {
                rightSubNode = (right as ValueInner<TValueRight, Leaf, ValueLeaf<TValueRight>>).Node2 as ValueNode<TValueRight, TStructure>;
            }
            else
            {
                rightSubNode = right.Node3<TValueRight, TStructure>();
            }

            return new ValueInner<(TValue, TValueRight), TStructure, ValueNode<(TValue, TValueRight), TStructure>>(
                (this.Value, right.Value),
                this.Node2.Zip3(rightSubNode, this.Structure.Node),
                null);
        }

        internal override ValueNode<(TValue, TValueRight), TStructure2> Zip3<TValueRight, TStructure2>(ValueNode<TValueRight, TStructure2> right, TStructure2 structure)
        {
            ValueNode<TValueRight, TStructure> rightSubNode;
            if (typeof(TStructure) == typeof(Leaf))
            {
                rightSubNode = (right as ValueInner<TValueRight, Leaf, ValueLeaf<TValueRight>>).Node2 as ValueNode<TValueRight, TStructure>;
            }
            else
            {
                rightSubNode = right.Node4<TValueRight, TStructure>(this.Structure.Node);
            }

            var zipped = this.Node2.Zip3(rightSubNode as ValueNode<TValueRight, TStructure>, rightSubNode.Structure);

            return zipped as ValueNode<(TValue, TValueRight), TStructure2>;

            /*return new ValueInner<(TValue, TValueRight), TStructure2, ValueNode<(TValue, TValueRight), TStructure2>>(
                (this.Value, right.Value),
                (ValueNode<(TValue, TValueRight), TStructure2>)null,
                ////this.Node2.Zip3<TValueRight, TStructure2>(rightSubNode, rightSubNode.Structure),
                null);*/
        }

        internal override ValueNode<TValue2, TStructure3>? Node3<TValue2, TStructure3>()
        {
            return this.Node2 as ValueNode<TValue2, TStructure3>;
        }

        internal override ValueNode<TValue3, TStructure4>? Node4<TValue3, TStructure4>(TStructure4 structure)
        {
            return this.Node2 as ValueNode<TValue3, TStructure4>;
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
