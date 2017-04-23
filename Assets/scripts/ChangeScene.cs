using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string nextScene;

	public void changeScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
