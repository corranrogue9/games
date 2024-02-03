using System.Globalization;
using System.IO.Compression;
using System.Net.Http.Headers;

namespace ConsoleApplication2
{
    public static class ListExt
    {
        public static TAggregate Aggregate2<TElement, TAggregate>(this IEnumerable<TElement> source, TAggregate seed, Func<TAggregate, TElement, TAggregate> accumulator)
        {
            foreach (var element in source)
            {
                seed = accumulator(seed, element);
            }

            return seed;
        }

        public static IEnumerable<TElement> Prepend2<TElement>(this IEnumerable<TElement> source, TElement element)
        {
            yield return element;

            foreach (var element2 in source)
            {
                yield return element2;
            }
        }

        public static IEnumerable<TElement> Append2<TElement>(this IEnumerable<TElement> source, TElement element)
        {
            return source.Aggregate2(Enumerable.Empty<TElement>(), (aggregate, element2) => aggregate.Prepend2(element2)).Prepend2(element).Reverse();
        }

        public static IEnumerable<TElement> Concat2<TElement>(this IEnumerable<TElement> first, IEnumerable<TElement> second)
        {
            return second.Aggregate2(first, (aggregate, element) => aggregate.Append2(element));
        }

        public static IEnumerable<TResult> Select2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Aggregate2(Enumerable.Empty<TResult>(), (aggregate, element) => aggregate.Append2(selector(element)));
        }

        public static IEnumerable<TResult> SelectMany2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.Aggregate2(Enumerable.Empty<TResult>(), (aggregate, element) => aggregate.Concat2(selector(element)));
        }

        public static IEnumerable<TElement> Where2<TElement>(this IEnumerable<TElement> source, Func<TElement, bool> predicate)
        {
            return source.Aggregate2(Enumerable.Empty<TElement>(), (aggregate, element) => predicate(element) ? aggregate.Append(element) : aggregate);
        }

        public static IEnumerable<TElement> Reverse2<TElement>(this IEnumerable<TElement> source)
        {
            return source.Aggregate2(Enumerable.Empty<TElement>(), (aggregate, element) => aggregate.Prepend2(element));
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

    public interface INode
    {
        public abstract INode? Remainder { get; }

        public abstract int Count { get; }
    }

    /*public abstract class Node
    {
        public abstract Node? Remainder { get; }

        public abstract int Count { get; }
    }*/

    public struct Leaf : INode
    {
        public INode? Remainder
        {
            get
            {
                return null;
            }
        }

        public int Count
        {
            get
            {
                return 1;
            }
        }
    }

    public struct Inner<TNode> : INode where TNode : INode
    {
        public Inner(TNode node)
        {
            this.Node = node;
        }

        public INode? Remainder
        {
            get
            {
                return this.Node;
            }
        }

        public int Count
        {
            get
            {
                return this.Node.Count + 1;
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

        public static ValueInner<TValue, Leaf, ValueLeaf<TValue>> Append<TValue>(this ValueLeaf<TValue> leaf, TValue value)
        {
            return new ValueInner<TValue, Leaf, ValueLeaf<TValue>>(value, leaf, default);
        }

        public static ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNodeStart>> Append<TValue, TStructure, TValueNodeStart>(
            this ValueInner<TValue, TStructure, TValueNodeStart> inner,
            TValue value)
            where TStructure : INode
            where TValueNodeStart : ValueNode<TValue, TStructure>
        {
            var thing = inner.Node2.Aggregate(Value.Create(inner.Value) as IValueNode<TValue>, (aggregate, element) => aggregate.Prepend(element)).Prepend(value);
            var casted = thing as ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNodeStart>>;
            return casted.Reverse();
        }

        public static ValueInner<TValue, Leaf, ValueLeaf<TValue>> Prepend<TValue>(this ValueLeaf<TValue> leaf, TValue value)
        {
            return leaf.Prepend(value, leaf.Structure);
        }

        public static ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNodeStart>> Prepend<TValue, TStructure, TValueNodeStart>(
            this ValueInner<TValue, TStructure, TValueNodeStart> inner,
            TValue value)
            where TStructure : INode
            where TValueNodeStart : ValueNode<TValue, TStructure>
        {
            return inner.Prepend(value, inner.Structure);
        }

        public static ValueInner<TValue, TStructure, TValueNodePrepend> Prepend<TValue, TStructure, TValueNodePrepend>(this TValueNodePrepend node, TValue value, TStructure structure)
            where TStructure : INode
            where TValueNodePrepend : ValueNode<TValue, TStructure>
        {
            return node.Prepend(value) as ValueInner<TValue, TStructure, TValueNodePrepend>;
        }

        public static ValueInner<TValue, TStructure, TValueNode> Reverse<TValue, TStructure, TValueNode>(this ValueInner<TValue, TStructure, TValueNode> inner)
            where TStructure : INode
            where TValueNode : ValueNode<TValue, TStructure>
        {
            return inner.Node2.Aggregate(new ValueLeaf<TValue>(inner.Value) as IValueNode<TValue>, (aggregate, element) => aggregate.Prepend(element)) as ValueInner<TValue, TStructure, TValueNode>;
        }

        /*public static IEnumerable<TElement> Append2<TElement>(this IEnumerable<TElement> source, TElement element)
        {
            return source.Aggregate2(Enumerable.Empty<TElement>(), (aggregate, element2) => aggregate.Prepend2(element2)).Reverse();
        }*/
    }

    public interface IValueNode<TValue>
    {
        IValueNode<TValue> Prepend(TValue value);

        TAggregate Aggregate<TAggregate>(TAggregate seed, Func<TAggregate, TValue, TAggregate> aggregator);
    }

    public abstract class ValueNode<TValue, TStructure> : IValueNode<TValue> where TStructure : INode
    {
        public abstract TValue Value { get; }

        public abstract TStructure Structure { get; }

        public abstract IEnumerable<TValue> ToStructureless();

        public abstract ValueNode<(TValue, TValue), TStructure> Zip(ValueNode<TValue, TStructure> right);

        public abstract ValueNode<(TValue, TValueRight), TStructure> Zip2<TValueRight>(ValueNode<TValueRight, TStructure> right);

        internal abstract ValueNode<(TValue, TValueRight), TStructure2> Zip3<TValueRight, TStructure2>(ValueNode<TValueRight, TStructure2> right, TStructure2 structure)
            where TStructure2 : INode;

        internal abstract ValueNode<TValue2, TStructure3>? Node3<TValue2, TStructure3>()
            where TStructure3 : INode;

        internal abstract ValueNode<TValue3, TStructure4>? Node4<TValue3, TStructure4>(TStructure4 structure)
            where TStructure4 : INode;

        public abstract TAggregate Aggregate<TAggregate>(TAggregate seed, Func<TAggregate, TValue, TAggregate> aggregator);

        internal abstract ValueInner<TValue, TStructure, TValueNodePrepend> PrependInternal<TValueNodePrepend>(TValueNodePrepend node, TValue value)
            where TValueNodePrepend : ValueNode<TValue, TStructure>;

        public abstract IValueNode<TValue> Prepend(TValue value);
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

        public override TAggregate Aggregate<TAggregate>(TAggregate seed, Func<TAggregate, TValue, TAggregate> aggregator)
        {
            return aggregator(seed, this.Value);
        }

        internal override ValueInner<TValue, Leaf, TValueNodePrepend> PrependInternal<TValueNodePrepend>(TValueNodePrepend node, TValue value)
        {
            return new ValueInner<TValue, Leaf, TValueNodePrepend>(value, node, default);
        }

        public override IValueNode<TValue> Prepend(TValue value)
        {
            return this.PrependInternal(this, value);
        }


        /*public ValueInner<TValue, Leaf, ValueLeaf<TValue>> Prepend(TValue value)
        {
            return new ValueInner<TValue, Leaf, ValueLeaf<TValue>>(this.Value, new ValueLeaf<TValue>(value), default);
        }*/
    }

    public sealed class ValueInner<TValue, TStructure, TValueNode> : ValueNode<TValue, Inner<TStructure>> where TValueNode : ValueNode<TValue, TStructure> where TStructure : INode
    {
        /*public ValueInner<TValue, Inner<TStructure>, ValueInner<TValue, TStructure, TValueNode>> Prepend(TValue value)
        {
        }*/

        public override TAggregate Aggregate<TAggregate>(TAggregate seed, Func<TAggregate, TValue, TAggregate> aggregator)
        {
            return this.Node2.Aggregate(aggregator(seed, this.Value), aggregator);
        }

        internal override ValueInner<TValue, Inner<TStructure>, TValueNodePrepend> PrependInternal<TValueNodePrepend>(TValueNodePrepend node, TValue value)
        {
            return new ValueInner<TValue, Inner<TStructure>, TValueNodePrepend>(
                value,
                node,
                default);
        }

        public ValueInner(TValue value, TValueNode node, Inner<TStructure> structure)
        {
            this.Value = value;
            this.Node2 = node;
            ////this.Structure = structure;
        }

        public override ValueNode<(TValue, TValue), Inner<TStructure>> Zip(ValueNode<TValue, Inner<TStructure>> right)
        {
            return new ValueInner<(TValue, TValue), TStructure, ValueNode<(TValue, TValue), TStructure>>(
                (this.Value, right.Value),
                this.Node2.Zip((right as ValueInner<TValue, TStructure, TValueNode>).Node2),
                this.Structure);
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
                this.Structure);
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

            ////return zipped as ValueNode<(TValue, TValueRight), TStructure2>;

            return new ValueInner<(TValue, TValueRight), TStructure, ValueNode<(TValue, TValueRight), TStructure>>(
                (this.Value, right.Value),
                zipped,
                ////(ValueNode<(TValue, TValueRight), TStructure2>)null,
                ////this.Node2.Zip3<TValueRight, TStructure2>(rightSubNode, rightSubNode.Structure),
                this.Structure) as ValueNode<(TValue, TValueRight), TStructure2>;
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

        public override Inner<TStructure> Structure { get; } = default;

        public TValueNode Node2 { get; }

        public override IEnumerable<TValue> ToStructureless()
        {
            yield return this.Value;
            foreach (var element in this.Node2.ToStructureless())
            {
                yield return element;
            }
        }

        public override IValueNode<TValue> Prepend(TValue value)
        {
            return this.PrependInternal(this, value);
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
