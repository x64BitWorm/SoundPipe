using Microsoft.Extensions.DependencyInjection;
using SP.Domain;

namespace SP.Worker
{
    public class ConsoleWorker
    {
        public static void Main()
        {
            // Приложение SoundPipe может быть также использовано и без UI оболочки
            // Для этого импортируйте в свое приложение проект SP.Domain
            Console.WriteLine("-= STARTED =-");

            // Регистрируем все необходимые сервисы
            var sp = new ServiceCollection();
            MainModule.RegisterServices(sp);
            var services = sp.BuildServiceProvider();
            var filtersManager = services.GetService(typeof(FiltersManager)) as FiltersManager;

            // Загружаем библиотеки фильтров из указанной директории
            filtersManager.LoadFilters("C:\\Users\\Eax\\source\\repos\\SoundPipe\\Experiments\\Workspace\\Filters");
            
            // Создаем фильтры из которых будет состоять наша схема
            // В нашем случае это простая цепочка WAV Файл -> Громкость -> Динамик
            var input = filtersManager.BuildFilter("Input");
            var volume = filtersManager.BuildFilter("Volume");
            var speaker = filtersManager.BuildFilter("Speaker");

            // Каждый фильтр инициализируем необходимыми параметрами
            // Чтобы узнать типы параметров конкретного фильтра можно воспользоваться
            // Выводом filtersManager.GetFilterMetaInfo("FILTER_NAME").ConstructorParamaters()
            input.Initialize(new[] { "C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\bwhite11.wav" });
            volume.Initialize(new object[] { input, 1.0f });
            speaker.Initialize(new[] { volume });

            // Запускаем работу динамика переведя его в состояние play
            speaker.SetHotValue("state", "play");

            // Заморозим поток чтобы прослушать звук 3 секунды
            Thread.Sleep(3000);

            // Теперь поменяем громкость (сделаем ее 2x)
            volume.SetHotValue("level", 2.0f);

            // Прослушаем - звук стал вдвое громче
            Thread.Sleep(3000);

            // Высвобождаем задействованные фильтры
            // Заметим, что первыми нужно высвобождать потребители (например, динамик)
            speaker.Destroy();
            volume.Destroy();
            input.Destroy();

            // Отметим, что SoundPipe можно использовать в множестве сценариев -
            // Самый простой из них это создание сложных звуковых эффектов для игр
            // При этом инструмент сам по себе является масштабируемым и производительным
            Console.WriteLine("-= end =-");
        }
    }

    /*
            Данный закомментированный участок показывает как можно загрузить схему из файла
            (чтобы не создавать ее непосредственно в коде)
            var shemeManager = services.GetService(typeof(ShemeManager)) as ShemeManager;
            filtersManager.LoadFilters("C:\\Users\\Eax\\source\\repos\\SoundPipe\\Experiments\\Workspace\\Filters\\");
            var shemeContent = File.ReadAllText("C:\\Users\\Eax\\Desktop\\MAI\\DIPL\\Experiments\\Workspace\\shemeUIhub.json");
            var shemeConstructor = shemeManager.ReadShemeConstructor<UINodeInfo>(shemeContent);
            var sheme = shemeConstructor.ConstructSheme();
            sheme.SetVar("f3", "state", "play");
            Thread.Sleep(5000);
            sheme.Destroy();
    */
}
