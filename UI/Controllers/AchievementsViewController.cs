﻿using DarkBestiary.Achievements;
using DarkBestiary.Managers;
using DarkBestiary.UI.Views;

namespace DarkBestiary.UI.Controllers
{
    public class AchievementsViewController : ViewController<IAchievementsView>
    {
        private readonly AchievementManager achievementManager;

        public AchievementsViewController(IAchievementsView view, AchievementManager achievementManager) : base(view)
        {
            this.achievementManager = achievementManager;
        }

        protected override void OnInitialize()
        {
            View.DrawAchievements(this.achievementManager.Achievements);
        }
    }
}