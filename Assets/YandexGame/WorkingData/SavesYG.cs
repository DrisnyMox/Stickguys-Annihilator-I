
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения
        public int coins;
        public int gears;
        public string firstTNT = "0";
        public string secondTNT = "3";
        public string gameData = string.Empty;
        public string autosData = string.Empty;
        public bool editorIsOpen;
        public bool slowMo;
        public float distance;
        public float blood;
        public int languageIndex;
        public int tntBonus = 1;
        public string tooltipTNT = string.Empty;
        public string chmos = string.Empty;
        public string skidko = string.Empty;

        public int boneColorIndex;
        public string unlocksColor = string.Empty;
        public bool bloodActive;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
