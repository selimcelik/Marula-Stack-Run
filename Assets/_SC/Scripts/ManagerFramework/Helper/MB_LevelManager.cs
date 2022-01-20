using UnityEngine;

namespace ManagerActorFramework.Utils
{
    public abstract class MB_LevelManager<TManager> : Manager<TManager> where TManager : Manager<TManager>
    {
        [Header("# Levels #")]
        [SerializeField] private Level[] Levels;

        public Level CurrentLevelData
        {
            get;
            private set;
        }

        public int CurrentLevel
        {
            get;
            private set;
        }

        public int LevelCount()
        {
            return Levels.Length;
        }

        // Private variables
        private int currentLevelIndex;
        private int randomLevelIndex = -1;

        public Level LoadLevelData(int level)
        {
            CurrentLevel = level;

            if (randomLevelIndex == -1)
            {
                randomLevelIndex = level;
            }

            if (CurrentLevel < Levels.Length)
            {
                currentLevelIndex = level;
                return LoadLevelIndex(currentLevelIndex);
            }
            else
            {
                return LoadRandomLevel();
            }
        }

        public Level LoadRandomLevel()
        {
            if (Levels.Length <= 1)
            {
                return LoadLevelIndex(0);
            }

            randomLevelIndex++;
            int random = randomLevelIndex % Levels.Length;
            if (random == currentLevelIndex)
            {
                return LoadRandomLevel();
            }
            else
            {
                return LoadLevelIndex(random);
            }
        }

        public Level RestartLevel()
        {
            return LoadLevelIndex(currentLevelIndex);
        }

        private Level LoadLevelIndex(int index)
        {
            currentLevelIndex = index;
            CurrentLevelData = Levels[currentLevelIndex];
            return CurrentLevelData;
        }
    }
}
