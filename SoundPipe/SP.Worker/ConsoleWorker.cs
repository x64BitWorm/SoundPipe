using Microsoft.Extensions.DependencyInjection;
using SP.Domain;

namespace SP.Worker
{
    public class ConsoleWorker
    {
        public static void Main()
        {
            Console.WriteLine("-= STARTED =-");

            var sp = new ServiceCollection();
            MainModule.RegisterServices(sp);
            var services = sp.BuildServiceProvider();
            var filtersManager = services.GetService(typeof(FiltersManager)) as FiltersManager;
            var shemeManager = services.GetService(typeof(ShemeManager)) as ShemeManager;
            filtersManager.LoadFilters();

            var shemeContent = File.ReadAllText("C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\shemeUIhub.json");
            var shemeConstructor = shemeManager.ReadShemeConstructor<UINodeInfo>(shemeContent);
            var sheme = shemeConstructor.ConstructSheme();
            sheme.SetVar("f3", "state", "play");
            sheme.SetVar("f2", "state", "play");
            Thread.Sleep(5000);
            sheme.Destroy();
            
            //shemeConstructor.SetParameterValue("f2", "level", 2.0f);
            //shemeConstructor.RemoveFilter("f1");
            //shemeContent = shemeManager.WriteShemeConstructor(shemeConstructor);
            //File.WriteAllText("C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\shemeUIout.json", shemeContent);
            Console.WriteLine("-end-");
            return;
            /*var shemeContent = File.ReadAllText("C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\sheme1.json");
            var sheme = shemeManager.ConstructSheme(shemeContent);
            sheme.SetVar("f3", "state", "play");
            for(var i = 0; i < 50; i++)
            {
                Console.WriteLine(sheme.GetVar("f4", "level"));
                Thread.Sleep(100);
            }
            sheme.Destroy();
            Console.WriteLine("-end-");
            return;*/

            var input = filtersManager.BuildFilter("Input");
            var volume = filtersManager.BuildFilter("Volume");
            var speaker = filtersManager.BuildFilter("Speaker");
            input.Initialize(new[] { "C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\bwhite11.wav" });
            volume.Initialize(new object[] { input, 1.0f });
            speaker.Initialize(new[] { volume });
            Thread.Sleep(3000);
            for (float i = 1.0f; i < 3.0f; i += 0.1f)
            {
                volume.SetHotValue("volume", i);
                Thread.Sleep(100);
            }
            Thread.Sleep(1000);
            volume.SetHotValue("volume", 1.0f);
            Thread.Sleep(3000);
            speaker.Destroy();
            input.Destroy();
            Console.WriteLine("-end-");
            
            /*
            
            Conversions -
            SPF file -> UI graph
            UI graph -> SPF file
            UI graph -> sound processor json file

            */
        }
    }
}
