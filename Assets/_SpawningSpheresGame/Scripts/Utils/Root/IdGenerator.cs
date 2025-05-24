namespace SpawningSpheresGame.Utils
{
    public class IdGenerator
    {
        private int _currentId;


        public IdGenerator(int lastValue = -1)
        {
            _currentId = lastValue;
        }


        public int GenerateId()
        {
            return ++_currentId;
        }


        public int GetLastValue()
        {
            return _currentId;
        }


        public void SetLastValue(int lastValue)
        {
            _currentId = lastValue;
        }
    }
}