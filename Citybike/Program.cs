using System;
using System.Threading.Tasks;

namespace Citybike
{
    class Program
    {
        static void Main(string[] args)
        {
            ICityBikeDataFetcher fetcher;

            if (args.Length > 1 && args[1] == "realtime")
                fetcher = new RealTimeCityBikeDataFetcher();
            else if (args.Length > 1 && args[1] == "offline")
                fetcher = new OfflineCityBikeDataFetcher();
            else
                return;

            try
            {
                var task = Task.Run(() => fetcher.GetBikeCountInStation(args[0]));
                task.Wait();
                Console.WriteLine(task.Result);
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is ArgumentException)
                        Console.WriteLine("Invalid argument:");
                    if (e is NotFoundException)
                        Console.WriteLine("Not found:");

                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
