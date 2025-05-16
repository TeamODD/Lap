using UnityEngine;
using UnityEngine.SceneManagement;

public class goNextScene : MonoBehaviour
{

    public string sceneToLoad;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            nextScene();
        }
    }
    void nextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
        }