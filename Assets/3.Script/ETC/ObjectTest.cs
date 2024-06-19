using UnityEngine;

public class ObjectTest : MonoBehaviour
{

    private void OnEnable()
    {

        //Debug.Log($"{gameObject.name} SetActive");
    }
    private void OnDisable()
    {
        if (transform.position.z >= -7f)
        {

            //Debug.Log($"{gameObject.name} {gameObject.transform.position} False");
        }

    }
}
