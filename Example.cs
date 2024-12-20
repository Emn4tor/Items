using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Zorro.Core;
using Object = UnityEngine.Object;


namespace ExampleCWPlugin
{



    [ContentWarningPlugin("CustomItemsPlugin", "1.0", false)]
    public class CustomItems
    {
        
        private static AssetBundle _assetBundle;
        
        static CustomItems()
        {
            
            Debug.Log("CustomItems plugin loaded.");
            
            DirectoryInfo? directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

            if (directory == null)
            {
                Debug.LogError("Failed to locate the plugin directory.");
                return;
            }
            
            
            // Load the main asset bundle
            string assetBundlePath = Path.Combine(directory.FullName, "customitemsassets/knife.assetbundle");
            _assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            if (_assetBundle == null)
            {
                Debug.LogError($"Failed to load asset bundle from path: {assetBundlePath}");
                return;
            }

            // Register custom items
            RegisterknifeItem();
            //add more items here
        }

        private static void RegisterknifeItem()
        {
            
                
            Item knifeItem = CreateItem("Knife", new Guid("c572e90e-25ff-40cd-8fdc-2e121b872b2b"));
            knifeItem.useAlternativeHoldingPos = true;
            knifeItem.useAlternativeHoldingRot = true;
            knifeItem.holdPos = new Vector3(0, 0.3f, 2.7f);
            knifeItem.purchasable = true;
            knifeItem.Category = (ShopItemCategory)1;
            knifeItem.price = 50;

            GameObject knifePrefab = _assetBundle.LoadAsset<GameObject>("Assets/BUNDLES/knife/baseball.prefab");
            if (knifePrefab == null)
            {
                Debug.LogError("Failed to load knifeItemPrefab.");
                return;
            }
            knifePrefab.transform.GetChild(0).gameObject.AddComponent<HandGizmo>();
            //knifePrefab.AddComponent<HandGizmo>();
            
            //knifePrefab.GetComponent<ItemInstance>().item = knifeItem;
            var itemInstance = knifePrefab.AddComponent<ItemInstance>();
            itemInstance.item = knifeItem;
            knifeItem.itemObject = knifePrefab;
            knifeItem.icon = _assetBundle.LoadAsset<Sprite>("Assets/BUNDLES/knife/knifeIcon.png");
            
            Debug.Log("Registered knife item.");
        }
        

        private static Item CreateItem(string name, Guid guid)
        {
            // Create a new Item instance
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.name = name;
            newItem.displayName = name;
            newItem.PersistentID = guid;

            // Register the item in the database
            SingletonAsset<ItemDatabase>.Instance.AddRuntimeEntry(newItem);

            Debug.Log($"Registered item: {name} with ID: {guid}");
            return newItem;
        }

    }
    
}