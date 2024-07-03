using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableManager : MonoBehaviour
{
    //[SerializeField] private AssetReferenceGameObject[] carObject;
    //[SerializeField] private AssetReferenceGameObject[] itemObject;




    private List<GameObject> objects = new List<GameObject>();


    private void Start()
    {
        // StartCoroutine(InitAddressable());
    }

    public void AddresableObject()
    {
        var temp = Addressables.InstantiateAsync("Bus_Blue");
        temp.Completed += (obj) => temp.Result.name = "sljdifjsdf";
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
    }



    //Addressable √ ±‚»≠
    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        return init;
    }
}
