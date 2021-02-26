using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Demo demo = new Demo();

            //await demo.AsParallelDemo();

            //await demo.AsOrderedDemo();

            //await demo.AsSequentialDemo();

            //await demo.WithDegreeOfParallelismDemo();

            //await demo.ForAllDemo();

            AsyncLoopDemo asyncLoopDemo = new AsyncLoopDemo();

            List<string> list = new List<string>() { "Kishor", "Yogesh", "Eshaan" };

            var results = await asyncLoopDemo.LoopAsyncResult(list);

            results
                .ToList()
                .ForEach((item) => Console.WriteLine(item));
        }
    }

    public class Demo
    {
        public Task AsParallelDemo()
        {
            return Task.Run(() =>
            {
                var source = Enumerable.Range(1, 5);

                var list =
                            source
                            ?.AsParallel()
                            ?.Select((value) => value)
                            ?.ToList();

                list
                    .ForEach((value) => Console.WriteLine(value)); // Data print is not an Ordered
            });
        }

        public Task AsOrderedDemo()
        {
            return Task.Run(() =>
            {
                var source = Enumerable.Range(1, 5);

                var list =
                            source
                            ?.AsParallel()?.AsOrdered()
                            ?.Select((value) => value)
                            ?.ToList();

                list
                    .ForEach((value) => Console.WriteLine(value)); // Data print is an Ordered
            });
        }

        public Task AsSequentialDemo()
        {
            return Task.Run(() =>
            {
                var source = Enumerable.Range(1, 5);

                var list =
                            source
                            ?.AsParallel()?.AsSequential()
                            ?.Select((value) => value)
                            ?.ToList();

                list
                    .ForEach((value) => Console.WriteLine(value)); // Data print is an Ordered
            });
        }

        public Task WithDegreeOfParallelismDemo()
        {
            return Task.Run(() =>
            {
                var source = Enumerable.Range(1, 5);

                var list =
                            source
                            ?.AsParallel()?.WithDegreeOfParallelism(2)?.AsSequential()
                            ?.Select((value) => value)
                            ?.ToList();

                list
                    .ForEach((value) => Console.WriteLine(value)); // Data print is an Ordered
            });
        }

        public Task ForAllDemo()
        {
            return Task.Run(() =>
            {
                var source = Enumerable.Range(1, 5);

                var list =
                            source
                            ?.AsParallel()
                            ?.Select((value) => value);

                list
                    .ForAll((value) => Console.WriteLine(value)); // Data print is an Ordered
            });
        }
    }

    public class AsyncLoopDemo
    {
        private Task<string> DoAsyncResult(string item)
        {
            Task.Delay(1000);
            return Task.FromResult(item);
        }

        public async Task<IReadOnlyList<string>> LoopAsyncResult(IEnumerable<String> thingsToLoop)
        {
            List<Task<string>> listTasks = new List<Task<string>>();

            var result =
                 thingsToLoop
                 ?.AsParallel()
                 ?.AsSequential()
                 ?.Select((item) => this.DoAsyncResult(item))
                 ?.ToList()
                 ?.AsReadOnly();

            return await Task.WhenAll<string>(result);
        }
    }
}