using R3;
using SpawningSpheresGame.Game.Common.DataTypes;

namespace PlayerController.MVVM.View.BinderComponents
{
    public class GroundCheckBinderComponent : MVVMComponent
    {
        private readonly GroundChecker _groundChecker;
        private readonly CreatureInput _creatureInput;
        private readonly Observable<Unit> _fixedTickObservable;


        public GroundCheckBinderComponent(GroundChecker groundChecker, CreatureInput creatureInput, Observable<Unit> fixedTickObservable)
        {
            _groundChecker = groundChecker;
            _creatureInput = creatureInput;
            _fixedTickObservable = fixedTickObservable;
        }


        public override void Initialize()
        {
            _fixedTickObservable.Subscribe(_ => _creatureInput.IsGrounded = _groundChecker.GetIsGrounded()).AddTo(Subscriptions);
        }
    }
}