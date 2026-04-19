
namespace YG
{
    public partial class SavesYG
    {

        // Ваши сохранения
#if DEV
        public int coins = 10000000;
#else
        public int coins;
#endif
        public int gears = 88;
        public string firstTNT = "0";
        public string secondTNT = "3";
        public string gameData = string.Empty;
        public string autosData = string.Empty;
        public bool editorIsOpen;
        public bool slowMo = true;
        public float distance = 7;
        public float blood = 88;
        public int languageIndex;
        public int tntBonus = 0;
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
        }
    }
}
