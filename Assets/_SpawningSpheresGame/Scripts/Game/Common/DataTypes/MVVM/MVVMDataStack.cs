namespace SpawningSpheresGame.Game.Common.DataTypes
{
    public class MVVMDataStack
    {
        public IModel Model { get; }
        public IViewModel ViewModel { get; }
        public IView View { get; }


        public MVVMDataStack(IModel model, IViewModel viewModel, IView view)
        {
            Model = model;
            ViewModel = viewModel;
            View = view;
        }
    }
}