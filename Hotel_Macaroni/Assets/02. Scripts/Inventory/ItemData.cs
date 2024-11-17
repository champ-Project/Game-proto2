using UnityEngine;

/*public enum ItemType
{
    interaction, item, etc
}*/

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemDataAsset")]
public class ItemData : ScriptableObject
{
    public string itemCode;
    //public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public string iteminfo;

    /*    [System.Serializable]
        public struct ItemData
        {


        }*/
}
