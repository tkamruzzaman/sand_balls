using UnityEditor;
using UnityEngine;

public class ServiceInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject m_ServiceObject;
    private bool isServiceInstantiated;

    private void Awake()
    {
        if (!isServiceInstantiated && IsInitializeScene())
        {
            isServiceInstantiated = true;
            GameObject serviceObj = Instantiate(m_ServiceObject, Vector3.zero, Quaternion.identity);
            serviceObj.name = "GAME_SERVICE";
        }
        else
        {
            Destroy(this);
        }
    }

    private bool IsInitializeScene()
    {
#if !UNITY_EDITOR
        return  UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
#endif
        return true;
    }
}
