using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    class Resources
    {
        public int ElectricityUsed { get; set; }
        public int WaterUsed { get; set; }

        public Resources(int electricityUsed, int waterUsed)
        {
            ElectricityUsed = electricityUsed;
            WaterUsed = waterUsed;
        }
    }

    class Program
    {
        static event Action SthHappend = delegate { };

        static async Task Main(string[] args)
        {
            Console.WriteLine("Preparing breakfast");
            Task.Run(EventCaller);
            Task<Resources> eggsTask = PrepareEggsAsync();
            Task<Resources> toastsTask = PrepareToastsAsync();
            Task<Resources> teaTask = PrepareTeaAsync();
            await Task.WhenAll(eggsTask, toastsTask, teaTask);
            int waterUsed = eggsTask.Result.WaterUsed + toastsTask.Result.WaterUsed + teaTask.Result.WaterUsed;
            int electricityUsed = eggsTask.Result.ElectricityUsed + toastsTask.Result.ElectricityUsed + teaTask.Result.ElectricityUsed;
            Console.WriteLine($"Breakfast done water used: {waterUsed} electiricty used: {electricityUsed}");
        }

        static void EventCaller()
        {
            Random random = new Random();
            while (true)
            {
                if (random.Next(1, 10) == 5)
                    SthHappend?.Invoke();
                Thread.Sleep(100);
            }
        }

        static async Task<Resources> PrepareEggsAsync()
        {
            Action sthHappened = () => { Console.WriteLine("Sth happened while preparing eggs D:"); };
            SthHappend += sthHappened;
            await Task.Delay(1000);
            Console.WriteLine("eggs are ready");
            SthHappend -= sthHappened;
            return new Resources(1, 3);
        }

        static async Task<Resources> PrepareToastsAsync()
        {
            Action sthHappened = () => { Console.WriteLine("Sth happened while preparing toasts D:"); };
            SthHappend += sthHappened;
            await Task.Delay(5000);
            Console.WriteLine("toasts are ready");
            SthHappend -= sthHappened;
            return new Resources(3, 0);
        }

        static async Task<Resources> PrepareTeaAsync()
        {
            Action sthHappened = () => { Console.WriteLine("Sth happened while preparing tea D:"); };
            SthHappend += sthHappened;
            await Task.Delay(2000);
            Console.WriteLine("tea are ready");
            SthHappend -= sthHappened;
            return new Resources(1, 1);
        }
    }
}
