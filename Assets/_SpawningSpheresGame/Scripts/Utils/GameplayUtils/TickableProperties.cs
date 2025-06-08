using R3;
using Zenject;

namespace SpawningSpheresGame.Utils.GameplayUtils
{
    public class TickableProperties : ITickable, IFixedTickable, ILateTickable
    {
        private readonly Subject<Unit> _tickSubject = new Subject<Unit>();
        private readonly Subject<Unit> _fixedTickSubject = new Subject<Unit>();
        private readonly Subject<Unit> _lateTickSubject = new Subject<Unit>();

        public Observable<Unit> OnTick => _tickSubject;
        public Observable<Unit> OnFixedTick => _fixedTickSubject;
        public Observable<Unit> OnLateTick => _lateTickSubject;

        public void FixedTick()
        {
            _fixedTickSubject.OnNext(Unit.Default);
        }

        public void LateTick()
        {
            _lateTickSubject.OnNext(Unit.Default);
        }

        public void Tick()
        {
            _tickSubject.OnNext(Unit.Default);
        }
    }
}