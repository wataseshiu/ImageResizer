using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ImageResizer : EditorWindow
{
    [MenuItem("Window/ImageResizer")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImageResizer));
    }

    //選択中のGameObjectに対して、Imageがあればネイティブサイズ*リサイズ倍率に画像のサイズを変更する
    private void Resize()
    {
        float anchorXMin = 0.0f;
        float anchorXMax = 0.0f;
        float anchorYMin = 0.0f;
        float anchorYMax = 0.0f;

        foreach (GameObject obj in Selection.gameObjects)
        {
            //リサイズ倍率が負の値なら終了
            if(resizeRate < 0) { return; }

            //Imageを持っていなかったら終了
            Image image = obj.GetComponent<Image>();
            if (image == null){break;}

            RectTransform rectTransform = obj.GetComponent<RectTransform>();

            //リサイズ後に再設定が必要となるアンカー情報の取得
            anchorXMin = rectTransform.anchorMin.x;
            anchorYMin = rectTransform.anchorMin.y;
            anchorXMax = rectTransform.anchorMax.x;
            anchorYMax = rectTransform.anchorMax.y;

            //一度ネイティブサイズにする
            //エディタ上でSetNativeSizeをするとアンカー設定周りが飛んだけど、スクリプトからの指定なら飛ばないっぽい
            image.SetNativeSize();

            //SetNativeSize()でとんでしまったアンカー設定をここで戻す
            rectTransform.anchorMin = new Vector2(anchorXMin, anchorYMin);
            rectTransform.anchorMax = new Vector2(anchorXMax, anchorYMax);

            //sizeDeltaにwith,heightの値が入っているので、リサイズ倍率で乗算したらお目当てのサイズになる
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * resizeRate, rectTransform.sizeDelta.y * resizeRate);
        }
    }

    private float defaultResizeRate = 0.67f;
    private float resizeRate = 0.67f;
    private void OnGUI()
    {
        if (GUILayout.Button("選択したGameObjectのImageをリサイズ")) { Resize(); }
        GUILayout.Label("リサイズ対象のGameObjectにアタッチされているImageを、\n選択した状態でボタン押下で実行");
        GUILayout.Label("フルHD解像度のリソースを、wolf解像度にリサイズします");

        resizeRate = EditorGUILayout.FloatField("リサイズ倍率", resizeRate);
        if (GUILayout.Button("リサイズ倍率をリセット")) { resizeRate = defaultResizeRate; }
    }
}
