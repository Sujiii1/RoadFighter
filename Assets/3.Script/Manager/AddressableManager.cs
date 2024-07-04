using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject[] carObjectReferences;

    private void Start()
    {
        StartCoroutine(InitAddressable());
    }

    /* public void AddresableObject()
     {
         //  var temp = Addressables.InstantiateAsync("Bus_Blue");
         //temp.Completed += (obj) => temp.Result.name = "sljdifjsdf";

         //objects.Add(temp.Result);
         //temp.Result.name = "sljdifjsdf";
         //for (int i = 0; i < carObject.Length; i++)
         //{
         //    carObject[i].InstantiateAsync().Completed += (obj) =>
         //    {
         //        objects.Add(obj.Result);
         //        Debug.Log("obj");
         //    };
         //}
     }*/

    public void AddresableObject()
    {
        foreach (var carObjectReference in carObjectReferences)
        {
            carObjectReference.InstantiateAsync().Completed += OnObjectLoaded;
        }
    }


    //로드된 오브젝트를 ObjectPool에 추가
    private void OnObjectLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = obj.Result;
            CarObject carObjectComponent = loadedObject.GetComponent<CarObject>();
            if (carObjectComponent != null)
            {
                ObjectPool.Instance.AddToPool(loadedObject, carObjectComponent.CarType);
                Debug.Log("Object loaded and added to pool: " + loadedObject.name);
            }
            else
            {
                Debug.LogError("OnObjectLoaded failed carObjectComponent");
            }
        }
        else
        {
            Debug.LogError("Failed to load object.");
        }
    }



    //Addressable 초기화
    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        return init;
    }
}
