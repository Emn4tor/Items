//SUFFER NOTICE!
//This is a very bad example of how to create a plugin for ContentWarning.
// AS OF 2024-12-18: I finally uploaded a version... Only cubes that are not droppable and dont do anything.
// AS OF 2024-12-20: I spent the whole night trying to figure out how to actually make the "Baseball Bat" look okay in the hand. I failed. I'm sorry.

//Wasted Time in total: 20 hours
//Waste Time reading through Game Code because there are no docs: 10 hours


using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Zorro.Core;
using Object = UnityEngine.Object;

namespace ItemsPlus
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
            RegisterBaseballItem();
            RegisterHockeyStickItem();
            RegisterBallItem();
        }
        
        

        private static void RegisterBaseballItem()
        {
            // Create the Baseball item
            Item baseballItem = CreateItem("Baseball", new Guid("b0124ee6-9acf-45e6-8742-4c6e232ea277"));
            baseballItem.useAlternativeHoldingPos = false;
            baseballItem.useAlternativeHoldingRot = true;
            baseballItem.alternativeHoldPos = new Vector3(-0.02f, -0.15f, 0.25f);
            baseballItem.alternativeHoldRot = new Vector3(-60f, 0.0f, 0.0f);
            baseballItem.holdPos = new Vector3(0, 0.3f, 2.7f);
            baseballItem.purchasable = true;
            baseballItem.Category = ShopItemCategory.Medical;; // Weapon category
            baseballItem.price = 50;
            
            // Load the prefab
            GameObject baseballPrefab = _assetBundle.LoadAsset<GameObject>("Assets/BUNDLES/knife/baseball.prefab");
            if (baseballPrefab == null) { 
                Debug.LogError("Failed to load Baseball prefab."); 
                return; 
            }

            baseballPrefab.transform.GetChild(0).gameObject.AddComponent<HandGizmo>();

            var itemInstance = baseballPrefab.AddComponent<ItemInstance>();
            itemInstance.item = baseballItem;
            baseballItem.itemObject = baseballPrefab;

            // Icon broken bc API is bad asf... +_+ Btw there are no docs for this API
            baseballItem.icon = _assetBundle.LoadAsset<Sprite>("Assets/BUNDLES/knife/knifeIcon.png");

            Debug.Log("Registered Baseball item.");
        }


        private static void RegisterHockeyStickItem()
        {
            // Create the Hockey Stick item
            Item hockeyStickItem = CreateItem("HockeyStick", new Guid("b0124ee6-9acf-45e6-8742-4c6e232ea278"));
            hockeyStickItem.useAlternativeHoldingPos = false;
            hockeyStickItem.useAlternativeHoldingRot = true;
            hockeyStickItem.alternativeHoldPos = new Vector3(-0.02f, -0.15f, 0.25f);
            hockeyStickItem.alternativeHoldRot = new Vector3(-60f, 0.0f, 0.0f);

            hockeyStickItem.holdPos = new Vector3(0, -0.3f, 0.7f);
            hockeyStickItem.holdRotation = new Vector3(45f, 180f, 0);

            hockeyStickItem.purchasable = true;
            hockeyStickItem.Category = ShopItemCategory.Medical;; // Weapon category
            hockeyStickItem.price = 25;

            // Load the prefab
            GameObject hockeyStickPrefab = _assetBundle.LoadAsset<GameObject>("Assets/BUNDLES/knife/hockeystick.prefab");
            if (hockeyStickPrefab == null) { 
                Debug.LogError("Failed to load Hockey Stick prefab."); 
                return; 
            }

            hockeyStickPrefab.transform.GetChild(0).gameObject.AddComponent<HandGizmo>();

            var itemInstance = hockeyStickPrefab.AddComponent<ItemInstance>();
            itemInstance.item = hockeyStickItem;
            hockeyStickItem.itemObject = hockeyStickPrefab;

            // Icon broken bc API is bad asf... +_+ Btw there are no docs for this API
            hockeyStickItem.icon = _assetBundle.LoadAsset<Sprite>("Assets/BUNDLES/knife/knifeIcon.png");

            Debug.Log("Registered Hockey Stick item.");
        }

        
        private static void RegisterBallItem()
        {
            // Create the Ball item
            Item ballItem = CreateItem("Ball", new Guid("b0124ee6-9acf-45e6-8742-4c6e232ea279"));
            ballItem.useAlternativeHoldingPos = false;
            ballItem.useAlternativeHoldingRot = true;
            ballItem.alternativeHoldPos = new Vector3(-0.02f, -0.15f, 0.25f);
            ballItem.alternativeHoldRot = new Vector3(-60f, 0.0f, 0.0f);
            ballItem.holdPos = new Vector3(0, 0.3f, 2.7f);
            ballItem.purchasable = true;
            ballItem.Category = ShopItemCategory.Medical; // Weapon category
            ballItem.price = 10;

            // Load the prefab
            GameObject ballPrefab = _assetBundle.LoadAsset<GameObject>("Assets/BUNDLES/knife/ball.prefab");
            if (ballPrefab == null) { 
                Debug.LogError("Failed to load Ball prefab."); 
                return; 
            }

            ballPrefab.AddComponent<HandGizmo>();

            var itemInstance = ballPrefab.AddComponent<ItemInstance>();
            itemInstance.item = ballItem;
            ballItem.itemObject = ballPrefab;

            // Icon broken bc API is bad asf... +_+ Btw there are no docs for this API
            ballItem.icon = _assetBundle.LoadAsset<Sprite>("Assets/BUNDLES/knife/knifeIcon.png");

            Debug.Log("Registered Ball item.");
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
