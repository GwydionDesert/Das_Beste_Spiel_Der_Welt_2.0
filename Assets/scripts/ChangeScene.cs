using UnityEngine;
using UnityEditor.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string nextScene;

	public void changeScene()
    {
        EditorSceneManager.LoadScene(nextScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
