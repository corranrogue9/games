﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Animal
    {
    }

    class Dog : Animal
    {
    }

    class Dachshund : Dog
    {
    }

    internal class DoWork
    {
        public static void Stuff()
        {
        }

        public static void Stuff2(IFoo<Dog, Bar<Dog>> foo)
        {
            var animalBar = new Bar<Animal>();
            AnimalBarWork(animalBar);

            var dogBar = new Bar<Dog>();
            ////AnimalBarWork(dogBar);

            var fooBar = foo.Frob();
            ////AnimalBarWork(fooBar);
        }

        public static void AnimalBarWork(Bar<Animal> animalBar)
        {
        }

        public static void Stuff3(IGat<Dog> gat)
        {
            DogGatWork(gat);
            AnimalGatWork(gat);
        }

        public static void DogGatWork(IGat<Dog> gat)
        {
        }

        public static void AnimalGatWork(IGat<Animal> gat)
        {
        }

        public static void Stuff4(IGat2<Dog, Bar<Dog>> gat)
        {
            DogGat2Work(gat);
            AnimalGat2Work(gat); // doesn't compile because bar<dog> cant be converted to bar<animal> because bar is not covariant
        }

        public static void DogGat2Work(IGat2<Dog, Bar<Dog>> gat)
        {
        }

        public static void AnimalGat2Work(IGat2<Animal, Bar<Animal>> gat)
        {
        }

        public static void AnimalDogGat2Work(IGat2<Dachshund, Bar<Dog>> gat) // doesn't compile because T2 is expected to be bar<dachshund> and bar<dog> can't be converted to bar<dachshund> because bar is not covariant
        {
        }

        public static void AnimalDogGat2Work(IGat2<Dog, Bar<Dachshund>> gat) // doesn't compile because T2 is expected to be bar<dog> and bar<dachshund> can't be converted to bar<dog> because bar is not covariant
        {
        }
    }

    public interface IGat2<out T1, out T2> where T2 : Bar<T1>
    {
        T2 Value { get; }
    }

    public interface IGat<out T1>
    {
        T1 Value { get; }
    }

    public interface IFoo<out T1, out T2> where T2 : Bar<T1>
    {
        T2 Frob();
    }

    public class Bar<T>
    {
        public T Value { get; set; }
    }
}
