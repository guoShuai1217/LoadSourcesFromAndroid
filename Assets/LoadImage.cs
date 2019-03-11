/*
 * 		Description: 安卓环境 , 加载图片和AB包
 *
 *  	CreatedBy:  国帅
 *
 *  	DataTime:  2019.03.11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{

    private Image ima; // 用于显示的图片

    private Text txt; // 用于显示的文字

    public List<Sprite> sprList = new List<Sprite>(); // 存放图片的集合

    public List<GameObject> objList = new List<GameObject>(); // 存放GameObject的集合

    private List<string> strList = new List<string>(); // 静态数据 , 存放图片的名字的集合

    private List<string> abList = new List<string>();  // 静态数据 , 存放AB包的名字的集合

    public Transform parent; // 父物体

    private void Awake()
    {
       
        strList.Add("vn");
        strList.Add("hanbing");
        strList.Add("jj");
        strList.Add("kt");
        strList.Add("jin");
        strList.Add("erke");

       
        abList.Add("xianshiqi");
        abList.Add("zhuozi_1");


        ima = transform.Find("Image").GetComponent<Image>();
        txt = transform.Find("Text").GetComponent<Text>();
        parent = GameObject.Find("Parent").transform;
        GetPath();
    }

    private void Start()
    {
        StartCoroutine(DownLoadSprite()); // 加载AB包的话 , 这里调用DownLoadAssetBundle就可以
    }

    /// <summary>
    /// 加载AB包
    /// </summary>
    /// <returns></returns>
    private IEnumerator DownLoadAssetBundle()
    {
        txt.text = "1";
        foreach (var item in abList)
        {
            txt.text = txt.text + "2";
            // 安卓路径(应该就是这里不对)
            WWW www = new WWW(filePath + "Prefabs/" + item + ".assetbundle");
            txt.text = txt.text + "3";
            while (!www.isDone)
            {
                Debug.Log(www.progress);
                txt.text = txt.text + "4";
            }
           
            if (string.IsNullOrEmpty(www.error))
            {
                txt.text = txt.text + "5";
                yield return www;

                AssetBundle ab = www.assetBundle;
                GameObject cube = ab.LoadAsset<GameObject>(item);
                Instantiate(cube);

                cube.name = item;
                objList.Add(cube);
                cube.SetActive(false);
              
                txt.text = txt.text + "6";
                Debug.Log("下载完成");
                
            }
            else
            {
                Debug.Log("下载出错");
                txt.text = txt.text + "7";
            }

        }

        GameObject obj = objList[1];
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(true);
        txt.text = txt.text + objList[1].name;
        txt.text = txt.text + "8";
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    private IEnumerator DownLoadSprite()
    {
        txt.text = "1";
        foreach (var item in strList)
        {
            txt.text = txt.text + "2";
            // 安卓路径(应该就是这里不对)
            WWW www = new WWW(filePath + "Sprites/" + item + ".jpg");
            txt.text = txt.text + "3";
            while (!www.isDone)
            {
                Debug.Log(www.progress);
                txt.text = txt.text + "4";
            }
            if (string.IsNullOrEmpty(www.error))
            {
                txt.text = txt.text + "5";
                yield return www;

                Texture2D tempTex = new Texture2D(www.texture.width, www.texture.height);
                www.LoadImageIntoTexture(tempTex);

                Sprite sprite = Sprite.Create(tempTex, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(1f, 1f));
                sprite.name = item;

                sprList.Add(sprite);
                txt.text = txt.text + "6";
                Debug.Log("下载完成");
                ima.sprite = sprite;
            }
            else
            {
                Debug.Log("下载出错");
                txt.text = txt.text + "7";
            }

        }

        //ima.sprite = sprList[2];
        // txt.text = txt.text + ima.sprite.name;
        txt.text = txt.text + "8";
    }

    string filePath = string.Empty;
    /// <summary>
    /// 获取平台路径
    /// </summary>
    void GetPath()
    {
        filePath =
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.streamingAssetsPath + "/";
#elif UNITY_STANDLONE_WIN || UNITY_EDITOR
        "file://" + Application.streamingAssetsPath + "/";
#else
        string.Empty;
#endif
    }

}