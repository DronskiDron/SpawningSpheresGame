using UnityEngine;

namespace SpawningSpheresGame.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Transform _UISceneContainer;


        private void Awake()
        {
            HideLoadingScreen();
        }


        public void ShowLoadingScreen()
        {
            _loadingScreen.SetActive(true);
        }


        public void HideLoadingScreen()
        {
            _loadingScreen.SetActive(false);
        }


        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();

            sceneUI.transform.SetParent(_UISceneContainer, false);
        }


        private void ClearSceneUI()
        {
            var childCount = _UISceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_UISceneContainer.GetChild(i).gameObject);
            }
        }
    }
}
