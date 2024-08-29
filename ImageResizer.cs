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

    //�I�𒆂�GameObject�ɑ΂��āAImage������΃l�C�e�B�u�T�C�Y*���T�C�Y�{���ɉ摜�̃T�C�Y��ύX����
    private void Resize()
    {
        float anchorXMin = 0.0f;
        float anchorXMax = 0.0f;
        float anchorYMin = 0.0f;
        float anchorYMax = 0.0f;

        foreach (GameObject obj in Selection.gameObjects)
        {
            //���T�C�Y�{�������̒l�Ȃ�I��
            if(resizeRate < 0) { return; }

            //Image�������Ă��Ȃ�������I��
            Image image = obj.GetComponent<Image>();
            if (image == null){break;}

            RectTransform rectTransform = obj.GetComponent<RectTransform>();

            //���T�C�Y��ɍĐݒ肪�K�v�ƂȂ�A���J�[���̎擾
            anchorXMin = rectTransform.anchorMin.x;
            anchorYMin = rectTransform.anchorMin.y;
            anchorXMax = rectTransform.anchorMax.x;
            anchorYMax = rectTransform.anchorMax.y;

            //��x�l�C�e�B�u�T�C�Y�ɂ���
            //�G�f�B�^���SetNativeSize������ƃA���J�[�ݒ���肪��񂾂��ǁA�X�N���v�g����̎w��Ȃ��΂Ȃ����ۂ�
            image.SetNativeSize();

            //SetNativeSize()�łƂ�ł��܂����A���J�[�ݒ�������Ŗ߂�
            rectTransform.anchorMin = new Vector2(anchorXMin, anchorYMin);
            rectTransform.anchorMax = new Vector2(anchorXMax, anchorYMax);

            //sizeDelta��with,height�̒l�������Ă���̂ŁA���T�C�Y�{���ŏ�Z�����炨�ړ��ẴT�C�Y�ɂȂ�
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * resizeRate, rectTransform.sizeDelta.y * resizeRate);
        }
    }

    private float defaultResizeRate = 0.67f;
    private float resizeRate = 0.67f;
    private void OnGUI()
    {
        if (GUILayout.Button("�I������GameObject��Image�����T�C�Y")) { Resize(); }
        GUILayout.Label("���T�C�Y�Ώۂ�GameObject�ɃA�^�b�`����Ă���Image���A\n�I��������ԂŃ{�^�������Ŏ��s");
        GUILayout.Label("�t��HD�𑜓x�̃��\�[�X���Awolf�𑜓x�Ƀ��T�C�Y���܂�");

        resizeRate = EditorGUILayout.FloatField("���T�C�Y�{��", resizeRate);
        if (GUILayout.Button("���T�C�Y�{�������Z�b�g")) { resizeRate = defaultResizeRate; }
    }
}
