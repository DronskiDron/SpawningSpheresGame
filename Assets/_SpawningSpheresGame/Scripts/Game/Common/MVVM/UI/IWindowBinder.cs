﻿namespace SpawningSpheresGame.Game.Common.MVVM.UI
{
    public interface IWindowBinder
    {
        void Bind(WindowViewModel viewModel);
        void Close();
    }
}