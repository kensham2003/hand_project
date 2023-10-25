using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 使用されない：SceneChangeを使う
/// </summary>
public class ChangeScene : MonoBehaviour
{
    //遷移先
    [SerializeField] string changeScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPushButton()
    {
        SceneManager.LoadScene(changeScene);
    }
}
